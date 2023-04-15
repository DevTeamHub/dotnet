using System.Security.Cryptography.X509Certificates;

namespace DevTeam.Helpers;

public static class CertificateHelper
{
    public static X509Certificate2 FindCertificate(string thumbnail)
    {
        var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadOnly);

        try
        {
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbnail, validOnly: false);
            return certCollection[0];
        }
        finally
        {
            store.Close();
        }
    }
}
