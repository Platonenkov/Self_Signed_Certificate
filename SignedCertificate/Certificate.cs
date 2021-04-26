using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SignedCertificate
{
    public static class Certificate
    {
        /// <summary>
        /// Create self-signed certificate
        /// </summary>
        /// <param name="options">Certificate options</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificate(CertificateOptionsBase options)
        {
            return options.Password is null
                ? CreateCertificate(options.CommonName, options.Years)
                : CreateCertificate(options.CommonName, options.Password, options.Years);
        }
        /// <summary>
        /// Create self-signed certificate
        /// </summary>
        /// <param name="CommonName">certificate name</param>
        /// <param name="Years">lifespan</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificate(string CommonName, int Years)
        {
            var rsa = RSA.Create(4096);
            var req = new CertificateRequest($"cn={CommonName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(Years));
        }

        /// <summary>
        /// Create self-signed certificate
        /// </summary>
        /// <param name="CommonName">certificate name</param>
        /// <param name="Password">Password</param>
        /// <param name="Years">lifespan</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificate(string CommonName, string Password, int Years)
        {
            var rsa = RSA.Create(4096);
            var req = new CertificateRequest($"cn={CommonName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(Years));

            return new X509Certificate2(cert.Export(X509ContentType.Pfx, Password), Password);
        }

        /// <summary>
        /// Create self-signed certificate from file
        /// </summary>
        /// <param name="PFXFilePath">file path to certificate pfx file</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificateFromFileWithoutPassword(string PFXFilePath)
            => new(PFXFilePath);

        /// <summary>
        /// Create self-signed certificate from file
        /// </summary>
        /// <param name="PFXFilePath">file path to certificate pfx file</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificateFromFile(string PFXFilePath, string password)
            => new(PFXFilePath, password);

        /// <summary>
        /// Create self-signed certificate from file
        /// </summary>
        /// <param name="options">Certificate options</param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificate(CertificateOptions options)
            => CreateCertificate(options.CommonName, options.Password, options.Years);

        /// <summary>
        /// Generate and create certificate file
        /// </summary>
        /// <param name="options">certificate options</param>
        public static void MakeCert(CertificateOptions options)
        {
            var rsa = RSA.Create(4096);
            var req = new CertificateRequest($"cn={options.CommonName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(options.Years));

            if (options.Password is null)
            {
                // Create PFX (PKCS #12) with private key
                File.WriteAllBytes(options.FullFilePathPFX, cert.Export(X509ContentType.Pfx));

                // Create Base 64 encoded CER (public key only)
                File.WriteAllText(options.FullFilePathCER,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
            else
            {
                // Create PFX (PKCS #12) with private key
                File.WriteAllBytes(options.FullFilePathPFX, cert.Export(X509ContentType.Pfx, options.Password));

                // Create Base 64 encoded CER (public key only)
                File.WriteAllText(options.FullFilePathCER,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
        }

        public static void MakeCert(string CommonName, string PathToSave, string Password, string CertificateFileName, int Years)
            => MakeCert(new CertificateOptions(PathToSave, CommonName, CertificateFileName, Password, Years));

    }
}
