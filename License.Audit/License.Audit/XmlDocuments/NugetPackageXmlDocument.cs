using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace License.Audit
{
    public class NugetPackageXmlDocument : XmlDocument
    {
        private readonly string[] packagesToIgnore = new string[] { "NuGet.", "System.", "Microsoft." };

        public NugetPackageXmlDocument(string filePath)
        {
            LoadXml(File.ReadAllText(filePath));
        }

        public IEnumerable<NugetPackage> GetPackages(string projectName, string projectPath, string xpath)
        {
            var nodes = SelectNodes(xpath);
            foreach (XmlElement element in nodes)
            {
                var rootDirectory = Directory.GetParent(projectPath);
                var relativeFilePath = element.InnerText;

                var filePath = Path.Combine(rootDirectory.FullName, relativeFilePath);

                if (!File.Exists(filePath))
                    continue;

                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                var version = $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}";

                if (packagesToIgnore.Any(packageToIgnore => fileName.StartsWith(packageToIgnore)))
                    continue;

                yield return new NugetPackage(fileName, version, projectName);
            }
        }

        public IEnumerable<NugetPackage> GetPackages(string projectName, string xpath, string attributeId, string attributeVersion)
        {
            var nodes = SelectNodes(xpath);
            foreach (XmlElement element in nodes)
            {
                var id = element.GetAttribute(attributeId);

                if (packagesToIgnore.Any(packageToIgnore => id.StartsWith(packageToIgnore)))
                    continue;

                yield return new NugetPackage(id, element.GetAttribute(attributeVersion), projectName);
            }
        }
    }
}
