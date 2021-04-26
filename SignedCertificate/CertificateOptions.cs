using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SignedCertificate
{

    public class CertificateOptions: CertificateOptionsBase
    {
        /// <summary>
        /// Генератор сертификата
        /// </summary>
        /// <param name="pathToSave">путь куда сохранить</param>
        /// <param name="commonName">имя сертификата</param>
        /// <param name="fileName">имя файла</param>
        /// <param name="password">пароль</param>
        /// <param name="years">период действия сертификата</param>
        public CertificateOptions(
            string pathToSave,
            string commonName,
            string fileName,
            string password,
            int years = 1) : base(commonName, password, years)
        {
            PathToSave = pathToSave;
            if (string.IsNullOrEmpty(PathToSave))
            {
                throw new ArgumentNullException(nameof(PathToSave));
            }

            CertificateFileName = fileName;
            if (string.IsNullOrEmpty(CertificateFileName))
            {
                throw new ArgumentNullException(nameof(CertificateFileName));
            }

        }

        #region Свойства

        public string PathToSave { get; }
        public string CertificateFileName { get; }

        #endregion

        public string FullFilePathPFX => Path.Combine(PathToSave, FileNamePFX);
        public string FileNamePFX => $"{CertificateFileName}.pfx";
        public string FullFilePathCER => Path.Combine(PathToSave, FileNameCER);
        public string FileNameCER => $"{CertificateFileName}.cer";
    }

    public class CertificateOptionsBase
    {
        public CertificateOptionsBase(
            string commonName,
            string password = null,
            int years = 1)
        {
            CommonName = commonName;
            if (string.IsNullOrEmpty(CommonName))
            {
                throw new ArgumentNullException(nameof(CommonName));
            }

            Password = password;

            Years = years;
            if (Years <= 0)
            {
                Years = 1;
            }

        }
        public string CommonName { get; protected set; }
        public string Password { get; protected set; }
        public int Years { get; protected set; }
    }

    public static class CertificateOptionsExtensions
    {
        /// <summary>
        /// Создать сертификат
        /// </summary>
        /// <param name="options">настройки</param>
        public static void MakeCert(this CertificateOptions options) => Certificate.MakeCert(options);
        /// <summary>
        /// Создать сертификат
        /// </summary>
        /// <param name="options">настройки</param>
        public static X509Certificate2 CreateCertificate(this CertificateOptions options) => Certificate.CreateCertificate(options);
        /// <summary>
        /// Создать сертификат
        /// </summary>
        /// <param name="options">настройки</param>
        public static X509Certificate2 CreateCertificate(this CertificateOptionsBase options) => Certificate.CreateCertificate(options);

    }

}
