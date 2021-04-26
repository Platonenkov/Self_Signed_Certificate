using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Self_Signed_Certificate
{
    public static class Certificate
    {
        public static void MakeCert(CertificateOptions options)
        {
            var rsa = RSA.Create(4096);
            var req = new CertificateRequest($"cn={options.CommonName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(options.Years));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes(options.FullFilePathPFX, cert.Export(X509ContentType.Pfx, options.Password));

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText(options.FullFilePathCER,
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }

        public static void MakeCert(string CommonName, string PathToSave, string Password, string CertificateFileName, int Years)
            => MakeCert(new CertificateOptions(PathToSave, CommonName, CertificateFileName, Password, Years));
    }
}
