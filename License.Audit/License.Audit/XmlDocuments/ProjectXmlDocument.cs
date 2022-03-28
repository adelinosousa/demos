using System.Xml;

namespace License.Audit
{
    public class ProjectXmlDocument : XmlDocument
    {
        public bool IsCoreProject
        {
            get
            {
                return SelectSingleNode("/Project/PropertyGroup/TargetFramework") != null;
            }
        }

        public ProjectXmlDocument(string projectFileContent)
        {
            LoadXml(projectFileContent);
        }
    }
}
