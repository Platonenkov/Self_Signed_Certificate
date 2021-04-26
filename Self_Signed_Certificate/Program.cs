using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Self_Signed_Certificate
{
    class Program
    {
        // enable nullable
        public static void Main(string[] args)
        {
            Console.WriteLine("Read arguments");
            var settings_row = args.Length > 0 ? args.Aggregate((current, s) => current + s) : string.Empty;
            var settings = string.IsNullOrWhiteSpace(settings_row)
                ? new Dictionary<string, string>()
                : settings_row.Split('-').Select(r => r.Trim()).ToDictionary(k => k.Split(' ')[0], v => v.Split(' ')[1]);

            var has_pathToSave = settings.ContainsKey("s");
            var pathToSave = has_pathToSave ? settings["s"] : Path.Combine(Environment.CurrentDirectory, "Result");
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            var has_commonName = settings.ContainsKey("n");
            var commonName = has_commonName ? settings["n"] : "SelfSignedCertificate";

            var has_fileName = settings.ContainsKey("f");
            var fileName = has_fileName ? settings["f"] : "SelfSignedCertificate";

            var has_password = settings.ContainsKey("p");
            var password = has_password ? settings["p"] : "P@55w0rd";

            int years;
            var has_years = settings.ContainsKey("y");
            if (has_years)
            {
                if(!int.TryParse(settings["y"], out years))
                    years = 5;
            }
            else
                years = 5;

            var options = new CertificateOptions
            (
                pathToSave: pathToSave,
                commonName: commonName,
                fileName: fileName,
                password: password,
                years: years
            );

            Console.WriteLine($"Saving in directory -> {pathToSave}");
            Console.WriteLine($"Certificate file name -> {fileName}");

            Console.WriteLine("Generating certificate");
            options.MakeCert();

            Console.WriteLine($"{Environment.NewLine}FINISHED");
            Console.ReadKey();
        }
    }
}
