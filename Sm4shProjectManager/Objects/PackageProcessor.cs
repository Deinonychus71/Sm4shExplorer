using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Objects
{
    public class PackageProcessor
    {
        public string PackedPath { get; set; }
        public string BaseToExclude { get; set; }
        public string ExportFolder { get; set; }
        public ResourceItem TopResource { get; set; }
        public List<string> FilesToAdd { get; set; }

        public PackageProcessor()
        {
            FilesToAdd = new List<string>();
        }
    }
}
