using System;

namespace Self_Signed_Certificate
{
    public class CertificateOptions
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
            int years = 1)
        {
            CommonName = commonName;
            if (string.IsNullOrEmpty(CommonName))
            {
                throw new ArgumentNullException(nameof(CommonName));
            }

            PathToSave = pathToSave;
            if (string.IsNullOrEmpty(PathToSave))
            {
                throw new ArgumentNullException(nameof(PathToSave));
            }

            Password = password;
            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentNullException(nameof(Password));
            }

            CertificateFileName = fileName;
            if (string.IsNullOrEmpty(CertificateFileName))
            {
                throw new ArgumentNullException(nameof(CertificateFileName));
            }

            Years = years;
            if (Years <= 0)
            {
                Years = 1;
            }
        }

        #region Свойства

        public string CommonName { get; }
        public string PathToSave { get; }
        public string Password { get; }
        public string CertificateFileName { get; }
        public int Years { get; }

        #endregion

    }

    public static class CertificateOptionsExtensions
    {
        /// <summary>
        /// Создать сертификат
        /// </summary>
        /// <param name="options">настройки</param>
        public static void MakeCert(this CertificateOptions options) => Certificate.MakeCert(options);

    }

}
