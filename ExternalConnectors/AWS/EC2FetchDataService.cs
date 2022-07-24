using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Microsoft.Win32;

namespace ExternalConnectors.AWS
{
    public class Ec2FetchDataService
    {
        private static DateTime _lastFetch;
        private static List<InstanceInfo>? _lastData;

        // input must be in format "AWSAPI:instanceid" where instanceid is the ec2 instance id, e.g. i-066f750a76c97583d
        public static async Task<string> GetEc2InstanceDataAsync(string input, string region)
        {
            // get secret id
            if (!input.StartsWith("AWSAPI:"))
                throw new Exception("calling this function requires AWSAPI: input");
            var instanceId = input[7..];

            // init connection credentials, display popup if necessary
            AwsConnectionData.Init();
            var alldata = await GetEc2IpDataAsync(region);
            var found = alldata.Where(x => x.InstanceId == instanceId).SingleOrDefault();
            return (found == null) ? "" : found.PublicIp;
        }

        private static async Task<List<InstanceInfo>> GetEc2IpDataAsync(string region)
        {
            // caching
            var timeSpan = DateTime.Now - _lastFetch;
            if (timeSpan.TotalMinutes < 1 && _lastData != null)
                return _lastData;

            //AWSConfigs.AWSRegion = AWSConnectionData.region;
            AWSConfigs.AWSRegion = region;
            var awsAccessKeyId = AwsConnectionData.AwsKeyId;
            var awsSecretAccessKey = AwsConnectionData.AwsKey;

            var client = new AmazonEC2Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
            var done = false;

            List<InstanceInfo> instanceList = new();
            var request = new DescribeInstancesRequest();
            while (!done)
            {
                var response = await client.DescribeInstancesAsync(request);

                foreach (var reservation in response.Reservations)
                {
                    foreach (var instance in reservation.Instances)
                    {
                        var vmname = "";
                        foreach (var tag in instance.Tags)
                        {
                            if (tag.Key == "Name")
                            {
                                vmname = tag.Value;
                            }
                        }
                        InstanceInfo inf = new(instance, vmname);
                        instanceList.Add(inf);
                    }
                }

                request.NextToken = response.NextToken;

                if (response.NextToken == null)
                {
                    done = true;
                }
            }

            _lastData = instanceList.OrderBy(x => x.Name).ToList();
            _lastFetch = DateTime.Now;
            return _lastData;
        }


        public static class AwsConnectionData
        {
            private static readonly RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteAWSInterface");

            public static string AwsKeyId = "";
            public static string AwsKey = "";
            //public static string _region = "eu-central-1";

            public static void Init()
            {
                if (AwsKey != "")
                    return;
                // display gui and ask for data
                AwsConnectionForm f = new();
                f.tbAccesKeyID.Text = "" + Key.GetValue("KeyID");
                f.tbAccesKey.Text = "" + Key.GetValue("Key");
                //f.tbRegion.Text = "" + key.GetValue("Region");
                //if (f.tbRegion.Text == null || f.tbRegion.Text.Length < 2)
                //    f.tbRegion.Text = region;
                _ = f.ShowDialog();

                if (f.DialogResult != DialogResult.OK)
                    return;

                // store values to memory
                AwsKeyId = f.tbAccesKeyID.Text;
                AwsKey = f.tbAccesKey.Text;
                //region = f.tbRegion.Text;


                // write values to registry
                Key.SetValue("KeyID", AwsKeyId);
                Key.SetValue("Key", AwsKey);
                //key.SetValue("Region", region);
                Key.Close();
            }
        }

    }
}
