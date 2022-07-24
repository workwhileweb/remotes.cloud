using mRemoteNG.Properties;

namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            var provider =
                new CryptoProviderFactory(OptionsSecurityPage.Default.EncryptionEngine, OptionsSecurityPage.Default.EncryptionBlockCipherMode).Build();
            provider.KeyDerivationIterations = OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
            return provider;
        }
    }
}