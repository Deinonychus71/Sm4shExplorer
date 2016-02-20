using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Objects
{
    public class PatchFileItem
    {
        public string RegionPath { get; set; }
        public string Path { get; set; }
        public bool Packed { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
