using CommandLine;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace License.Audit
{
    class Options
    {
        [Option('p', "path", Required = true, HelpText = "Path to .net project directory")]
        public string Path { get; set; }

        [Option('o', "output", Required = false, HelpText = "Outputs results to console, on by default")]
        public bool Output { get; set; } = true;
    }

    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();
        static readonly SourceCacheContext sourceCacheContext = new SourceCacheContext();
        static readonly SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

        static List<License> Licenses { get; } = new List<License>
        {
            new License("MIT", "mit"),
            new License("Apache License 2.0", "apache-2.0", "apache license 2.0"),
            new License("Apache License", "apache license"),
            new License("GNU General Public License v3", "gpl-3.0"),
            new License("GNU Lesser General Public License v3", "lgpl-3.0"),
            new License("GNU General Public License v2.0", "gpl-2.0"),
            new License("GNU Lesser General Public License v2.1", "lgpl-2.1"),
            new License("BSD 2-Clause", "bsd-2-clause"),
            new License("BSD 3-Clause","bsd-3-clause"),
            new License("BSD License", "bsd license"),
            new License("Creative Commons Zero v1.0 Universal","cc0-1.0"),
            new License("Eclipse Public License 2.0", "epl-2.0"),
            new License("Mozilla Public License 2.0", "mpl-2.0"),
            new License("Microsoft Software License", "microsoft software license", "ms-pl"),
            new License("ANTLR 3 License", "antlr 3"),
            new License("LGPL v3 License", "lgpl v3"),
            new License("Zlib License", "zlib")
        };

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(RunOptions);
        }

        static void RunOptions(Options options)
        {
            var output = Run(options.Path);

            if (options.Output)
            {
                Console.Write(OutputConsole(output));
            }

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}license.audit.csv", OutputCsv(output));
        }

        static NugetPackage[] Run(string rootProjectPath)
        {
            var projectsPath = Directory.GetFiles(rootProjectPath, "*.csproj", SearchOption.AllDirectories);

            var tasks = new List<Task>();
            var nugetPackages = new ConcurrentDictionary<string, NugetPackage>();

            foreach (var projectPath in projectsPath)
            {
                var project = new Project(projectPath);

                foreach (var nugetPackage in project.Packages)
                {
                    tasks.Add(GetPackageMetadataAsync(nugetPackage)
                        .ContinueWith(x =>
                        {
                            if (nugetPackages.ContainsKey(nugetPackage.Key))
                                return null;

                            return FetchLicenseAsync(x.Result).Result;
                        })
                        .ContinueWith(x =>
                        {
                            Console.Write(".");

                            NugetPackage nugetPackageMetadata = x.Result;

                            if (nugetPackageMetadata != null)
                                nugetPackages.TryAdd(nugetPackageMetadata.Key, nugetPackageMetadata);
                        }));
                }
            }

            Task.WaitAll(tasks.ToArray());

            return nugetPackages.Values.OrderBy(x => x.ProjectName).ToArray();
        }

        static async Task<NugetPackage> GetPackageMetadataAsync(NugetPackage nugetPackage)
        {
            var resource = await repository.GetResourceAsync<PackageMetadataResource>();

            var packageMetadata = await resource.GetMetadataAsync(
                new PackageIdentity(nugetPackage.Id, new NuGetVersion(nugetPackage.Version)),
                sourceCacheContext,
                NullLogger.Instance,
                CancellationToken.None);

            nugetPackage.SetPackageMetadata(packageMetadata);
            return nugetPackage;
        }

        static async Task<NugetPackage> FetchLicenseAsync(NugetPackage nugetPackage)
        {
            if (nugetPackage.PackageMetadata != null)
            {
                if (nugetPackage.PackageMetadata.LicenseMetadata?.License != null)
                {
                    nugetPackage.LicenseType = nugetPackage.PackageMetadata.LicenseMetadata.License;
                }
                else if (!string.IsNullOrEmpty(nugetPackage.PackageMetadata.LicenseUrl?.AbsoluteUri))
                {
                    var licenseFile = await Fetch(nugetPackage.PackageMetadata.LicenseUrl.AbsoluteUri);
                    if (!string.IsNullOrEmpty(licenseFile))
                    {
                        var licenseMatch = Licenses.FirstOrDefault(y => y.IsMatch(licenseFile));
                        if (licenseMatch != null)
                        {
                            nugetPackage.LicenseType = licenseMatch.Name;
                        }
                    }
                }
            }
            return nugetPackage;
        }

        static async Task<string> Fetch(string url)
        {
            try
            {
                using var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch { }
            return string.Empty;
        }

        static string OutputConsole(NugetPackage[] nugetPackages)
        {
            var format = "| {0,-50} | {1,-60} | {2,-40} | {3,-150} |";

            var output = new StringBuilder();
            output.AppendLine();
            output.AppendLine(string.Format(format, "Project", "Package", "License", "License URL"));

            foreach (var nugetPackage in nugetPackages)
            {
                if (nugetPackage.PackageMetadata == null)
                {
                    output.AppendLine(string.Format(format, nugetPackage.ProjectName, nugetPackage.Key, "Package not found", "N/A"));
                    continue;
                }

                output.AppendLine(string.Format(format, nugetPackage.ProjectName, nugetPackage.Key, nugetPackage.LicenseType, nugetPackage.PackageMetadata?.LicenseUrl?.AbsoluteUri));
            }

            return output.ToString();
        }

        static string OutputCsv(NugetPackage[] nugetPackages)
        {
            var output = new StringBuilder();
            output.AppendLine("Project, Package, License, License URL");

            foreach (var nugetPackage in nugetPackages)
            {
                if (nugetPackage.PackageMetadata == null)
                {
                    output.AppendLine($"{nugetPackage.ProjectName}, {nugetPackage.Key}, Package not found, N/A");
                    continue;
                }

                output.AppendLine($"{nugetPackage.ProjectName}, {nugetPackage.Key}, {nugetPackage.LicenseType}, {nugetPackage.PackageMetadata?.LicenseUrl?.AbsoluteUri}");
            }

            return output.ToString();
        }
    }
}
