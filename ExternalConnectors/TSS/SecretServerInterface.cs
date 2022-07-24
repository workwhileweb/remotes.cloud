using Microsoft.Win32;
using SecretServerAuthentication.TSS;
using SecretServerRestClient.TSS;

namespace ExternalConnectors.TSS
{
    public class SecretServerInterface
    {
        private static class SsConnectionData
        {
            public static string SsUsername = "";
            public static string SsPassword = "";
            public static string SsUrl = "";
            public static string SsOtp = "";
            public static bool SsSso = false;
            public static bool Initdone = false;

            //token 
            public static string SsTokenBearer = "";
            public static DateTime SsTokenExpiresOn = DateTime.UtcNow;
            public static string SsTokenRefresh = "";

            public static void Init()
            {
                if (Initdone == true)
                    return;

                var key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteSSInterface");
                try
                {
                    // display gui and ask for data
                    var f = new SsConnectionForm();
                    var un = key.GetValue("Username") as string;
                    f.tbUsername.Text = un ?? "";
                    f.tbPassword.Text = SsPassword;    // in OTP refresh cases, this value might already be filled

                    if (key.GetValue("URL") is not string url || !url.Contains("://"))
                        url = "https://cred.domain.local/SecretServer";
                    f.tbSSURL.Text = url;

                    var b = key.GetValue("SSO");
                    if (b == null || (string)b != "True")
                        SsSso = false;
                    else
                        SsSso = true;
                    f.cbUseSSO.Checked = SsSso;
                    
                    // show dialog
                    while (true)
                    {
                        _ = f.ShowDialog();

                        if (f.DialogResult != DialogResult.OK)
                            return;

                        // store values to memory
                        SsUsername = f.tbUsername.Text;
                        SsPassword = f.tbPassword.Text;
                        SsUrl = f.tbSSURL.Text;
                        SsSso = f.cbUseSSO.Checked;
                        SsOtp = f.tbOTP.Text;
                        // check connection first
                        try
                        {
                            if (TestCredentials() == true)
                            {
                                Initdone = true;
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Test Credentials failed - please check your credentials");
                        }
                    }


                    // write values to registry
                    key.SetValue("Username", SsUsername);
                    key.SetValue("URL", SsUrl);
                    key.SetValue("SSO", SsSso);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    key.Close();
                }

            }
        }

        private static bool TestCredentials()
        {
            if (SsConnectionData.SsSso)
            {
                // checking creds doesn't really make sense here, as we can't modify them anyway if something is wrong
                return true;
            }
            else
            {

                if (!String.IsNullOrEmpty(GetToken()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static void FetchSecret(int secretId, out string secretUsername, out string secretPassword, out string secretDomain)
        {
            var baseUrl = SsConnectionData.SsUrl;

            SecretModel secret;
            if (SsConnectionData.SsSso)
            {
                // REQUIRES IIS CONFIG! https://docs.thycotic.com/ss/11.0.0/api-scripting/webservice-iwa-powershell
                var handler = new HttpClientHandler() { UseDefaultCredentials = true };
                using (var httpClient = new HttpClient(handler))
                {
                    // Call REST API:
                    var client = new SecretsServiceClient($"{baseUrl}/winauthwebservices/api", httpClient);
                    secret = client.GetSecretAsync(false, true, secretId, null).Result;
                }
            }
            else
            {
                using (var httpClient = new HttpClient())
                {

                    var token = GetToken();
                    // Set credentials (token):
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // Call REST API:
                    var client = new SecretsServiceClient($"{baseUrl}/api", httpClient);
                    secret = client.GetSecretAsync(false, true, secretId, null).Result;
                }
            }

            // clear return variables
            secretDomain = "";
            secretUsername = "";
            secretPassword = "";

            // parse data and extract what we need
            foreach (var item in secret.Items)
            {
                if (item.FieldName.ToLower().Equals("domain"))
                    secretDomain = item.ItemValue;
                else if (item.FieldName.ToLower().Equals("username"))
                    secretUsername = item.ItemValue;
                else if (item.FieldName.ToLower().Equals("password"))
                    secretPassword = item.ItemValue;
            }

        }

        private static string GetToken()
        {
            // if there is no token, fetch a fresh one
            if (String.IsNullOrEmpty(SsConnectionData.SsTokenBearer))
            {
                return GetTokenFresh();
            }
            // if there is a token, check if it is valid
            if (SsConnectionData.SsTokenExpiresOn >= DateTime.UtcNow)
            {
                return SsConnectionData.SsTokenBearer;
            }
            else
            {
                // try using refresh token
                using (var httpClient = new HttpClient())
                {
                    var tokenClient = new OAuth2ServiceClient(SsConnectionData.SsUrl, httpClient);
                    TokenResponse token = new();
                    try
                    {
                        token = tokenClient.AuthorizeAsync(Grant_type.Refresh_token, null, null, SsConnectionData.SsTokenRefresh, null).Result;
                        var tokenResult = token.Access_token;

                        SsConnectionData.SsTokenBearer = tokenResult;
                        SsConnectionData.SsTokenRefresh = token.Refresh_token;
                        SsConnectionData.SsTokenExpiresOn = token.Expires_on;
                        return tokenResult;
                    }
                    catch (Exception)
                    {
                        // refresh token failed. clean memory and start fresh
                        SsConnectionData.SsTokenBearer = "";
                        SsConnectionData.SsTokenRefresh = "";
                        SsConnectionData.SsTokenExpiresOn = DateTime.Now;
                        // if OTP is required we need to ask user for a new OTP
                        if (!String.IsNullOrEmpty(SsConnectionData.SsOtp))
                        {
                            SsConnectionData.Initdone = false;
                            // the call below executes a connection test, which fetches a valid token
                            SsConnectionData.Init();
                            // we now have a fresh token in memory. return it to caller
                            return SsConnectionData.SsTokenBearer;
                        }
                        else
                        {
                            // no user interaction required. get a fresh token and return it to caller
                            return GetTokenFresh();
                        }
                    }
                }
            }
        }
        static string GetTokenFresh()
        {
            using (var httpClient = new HttpClient())
            {
                // Authenticate:
                var tokenClient = new OAuth2ServiceClient(SsConnectionData.SsUrl, httpClient);
                // call below will throw an exception if the creds are invalid
                var token = tokenClient.AuthorizeAsync(Grant_type.Password, SsConnectionData.SsUsername, SsConnectionData.SsPassword, null, SsConnectionData.SsOtp).Result;
                // here we can be sure the creds are ok - return success state                   
                var tokenResult = token.Access_token;

                SsConnectionData.SsTokenBearer = tokenResult;
                SsConnectionData.SsTokenRefresh = token.Refresh_token;
                SsConnectionData.SsTokenExpiresOn = token.Expires_on;
                return tokenResult;
            }
        }



        // input must be in form "SSAPI:xxxx" where xxx is the secret id to fetch
        public static void FetchSecretFromServer(string input, out string username, out string password, out string domain)
        {
            // get secret id
            if (!input.StartsWith("SSAPI:"))
                throw new Exception("calling this function requires SSAPI: input");
            var secretId = Int32.Parse(input.Substring(6));

            // init connection credentials, display popup if necessary
            SsConnectionData.Init();

            // get the secret
            FetchSecret(secretId, out username, out password, out domain);
        }
    }
}
