using System.Collections.Generic;

namespace Sm4shFileExplorer.Objects
{
    internal class PackageProcessor
    {
        public string PackedRelativePath { get; set; }
        public string PackedAbsolutePath { get { return TopResource.ResourceCollection.PartitionName + "/" + PackedRelativePath; } }
        public string ExportFolder { get; set; }
        public ResourceItem TopResource { get; set; }
        public List<string> FilesToAdd { get; set; }

        public PackageProcessor()
        {
            FilesToAdd = new List<string>();
        }
    }
}
