using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Objects
{
    public class ResourceExtension
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public int Flag { get; set; }
        public int MinFileSizeForCompression { get; set; }

        public ResourceExtension(string name, int flag)
        {
            this.Name = name;
            this.Flag = flag;
            this.MinFileSizeForCompression = 1024;
        }

        public ResourceExtension(string name, int flag, int minFileSizeForCompression)
        {
            this.Name = name;
            this.Flag = flag;
            this.MinFileSizeForCompression = minFileSizeForCompression;
        }
    }
}
