using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Self_Signed_Certificate
{
    class Program
    {
        private static string _DefaultDirectory => Path.Combine(Environment.CurrentDirectory, "Result");
        private static string _DefaultName => "SelfSignedCertificate";
        private static string _DefaultFileName => "SelfSignedCertificate";
        private static string _DefaultPassword => "P@55w0rd";
        private static int _DefaultYears => 5;
        // enable nullable
        public static void Main(string[] args)
        {
            //args = new[] {"-s D:\Temp -n Cert -f my_new_certificate -p 123qwe"};
            Console.WriteLine("Read arguments");
            var settings_row = args.Length > 0 ? args.Aggregate((current, s) => current + $" {s}") : string.Empty;
            var settings = string.IsNullOrWhiteSpace(settings_row)
                ? new Dictionary<string, string>()
                : settings_row.Trim().Trim('-').Split('-')
                   .Select(r => r.Trim()).ToDictionary(k => k.Split(' ')[0], v =>
                    {
                        try
                        {
                            return v.Split(' ')[1];
                        }
                        catch (IndexOutOfRangeException )
                        {
                            return null;
                        }
                        
                    });

            if (settings.ContainsKey("h") || settings.ContainsKey("help"))
            {
                Help();

            }
            settings.TryGetValue("s", out var pathToSave);
            pathToSave ??= _DefaultDirectory;

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            settings.TryGetValue("n", out var commonName);
            commonName ??= _DefaultName;

            settings.TryGetValue("f", out var fileName);
            fileName ??= _DefaultFileName;

            settings.TryGetValue("p", out var password);
            password ??= _DefaultPassword;

            int years;
            var has_years = settings.ContainsKey("y");
            if (has_years)
            {
                if (!int.TryParse(settings["y"], out years) || years == 0)
                    years = _DefaultYears;
            }
            else
                years = _DefaultYears;

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
        static void Help()
        {
            Console.WriteLine();
            Console.WriteLine($"-s Path to directory where certificate will save.{Environment.NewLine}Default path - {_DefaultDirectory}");
            Console.WriteLine($"-n Name of certificate,{Environment.NewLine}\tDefault certificate name - {_DefaultName}");
            Console.WriteLine($"-f The file name of certificate,{Environment.NewLine}\tDefault file name - {_DefaultFileName}");
            Console.WriteLine($"-p Password key to certificate,{Environment.NewLine}\tDefault password - {_DefaultPassword}");
            Console.WriteLine($"-y How long certificate will work,{Environment.NewLine}\tDefault years - {_DefaultYears}");
            Console.WriteLine();
        }

    }
}
