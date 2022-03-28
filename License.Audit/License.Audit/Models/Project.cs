using System.IO;
using System.Linq;

namespace License.Audit
{
    public class Project
    {
        private readonly ProjectXmlDocument xmlDocument;
        private NugetPackage[] packages;

        public string Path { get; }

        public string Name { get; }

        public string PackagesConfigPath => @$"{Directory.GetParent(Path)}\packages.config";

        public bool DoesHavePackagesConfig => File.Exists(PackagesConfigPath);

        public bool IsCoreProject => xmlDocument.IsCoreProject;

        public NugetPackage[] Packages
        {
            get 
            {
                if (packages == null)
                {
                    packages = new NugetPackage[0];

                    if (IsCoreProject)
                    {
                        packages = new NugetPackageXmlDocument(Path).GetPackages(Name, "/Project/ItemGroup/PackageReference", "Include", "Version").ToArray();
                    }
                    else if (DoesHavePackagesConfig)
                    {
                        var configPackages = new NugetPackageXmlDocument(PackagesConfigPath).GetPackages(Name, "/packages/package", "id", "version").ToArray();

                        var referencePackages = new NugetPackageXmlDocument(Path).GetPackages(Name, Path, "/*/*/*[local-name()='Reference']/*[local-name()='HintPath']").ToArray();

                        packages = configPackages.Concat(referencePackages).GroupBy(x => x.Key).Select(x => x.First()).ToArray();
                    }
                }

                return packages;
            }
        }

        public Project(string projectPath) 
        {
            Path = projectPath;

            Name = System.IO.Path.GetFileNameWithoutExtension(Path);

            xmlDocument = new ProjectXmlDocument(File.ReadAllText(Path));
        }
    }
}
