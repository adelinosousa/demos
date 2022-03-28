using NuGet.Protocol.Core.Types;

namespace License.Audit
{
    public class NugetPackage
    {
        public string Id { get; }

        public string Version { get; }

        public string ProjectName { get; }

        public string LicenseType { get; set; } = "Unknown";

        public IPackageSearchMetadata PackageMetadata { get; private set; }

        public string Key => $"{Id} v{Version}";

        public NugetPackage(string id, string version, string projectName)
        {
            Id = id;
            Version = version;
            ProjectName = projectName;
        }

        public void SetPackageMetadata(IPackageSearchMetadata packageMetadata)
        {
            PackageMetadata = packageMetadata;
        }
    }
}
