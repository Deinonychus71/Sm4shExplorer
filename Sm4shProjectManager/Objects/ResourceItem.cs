using DTLS;
using Sm4shProjectManager.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sm4shProjectManager.Objects
{
    public class ResourceItem : ICloneable
    {
        private bool _IsPackage;

        public uint OriginalFlags { get; set; }

        public ResourceCollection ResourceCollection { get; set; }
        public ResourceItem OriginalResourceItem { get; set; }

        public PatchFileItem PatchItem { get; set; }
        public FileSource Source { get; set; }
        public LSEntryObject LSEntryInfo { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get { if (Filename.Contains(".")) return false; return true; } }

        public uint OffInPack { get; set; }
        public uint CmpSize { get; set; }
        public uint DecSize { get; set; }
        public bool IsAPackage {
            get
            {
                if (_IsPackage)
                    return _IsPackage;
                if (!IsFolder && OffInPack == 0)
                    return true;
                return false;
            }
            set { _IsPackage = value; }
        }
        public string Filename { get; set; }
        public uint Flags { get { return CalculateFlags(); } }
        public uint FolderDepth { get { return (uint)this.Path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length; } }

        public ResourceItem(string filename, uint offinpack, uint cmpsize, uint decsize, bool ispackage)
        {
            Filename = filename;
            OffInPack = offinpack;
            CmpSize = cmpsize;
            DecSize = decsize;
            IsAPackage = ispackage;
        }

        public ResourceItem()
        {
        }

        private uint CalculateFlags()
        {
            uint flag = 0x00000000;

            flag |= FolderDepth;

            if (this.IsAPackage)
                flag |= 0x400;

            if (!this.ResourceCollection.IsRegion) //Everything in the main resource has the flag
                flag |= 0x800;
            else if (this.ResourceCollection.Resources.ContainsKey(this.Path) && this.ResourceCollection.Resources[this.Path].Source != FileSource.NotFound) //Everything physically present in region partition has the flag
                flag |= 0x800;

            if (GlobalConstants.FORCE_ACCURATE_LOCALIZATION_FLAG)
            {
                //NOT SETTING THOSE FLAGS DOESNT BREAK THE GAME, THEY DON'T REALLY MAKE SENSE ANYWAY...
                if (this.OffInPack != 0 || (this.IsFolder && !this.IsAPackage)) //Everything in package or folder has the flag 0x800
                    flag |= 0x800;
                else if (this.Path.StartsWith("ui/replace/customize/") || this.Path.StartsWith("ui/replace/clear/")) //Everything in those folders has the flag (I think spi_10 and clear_11 meant to be packages)
                    flag |= 0x800;
                else if(this.Path.StartsWith("model/enemy/") || this.Path.StartsWith("model/miihat/")) //model enemies and hats have all the flag
                    flag |= 0x800;
                //else if (this.IsAPackage && this.IsFolder && this.Path.StartsWith("fighter/")) //EXCEPT for the characters that are potentially localized (ike, purin, ryu...), char
                //    flag |= 0x800;
            }

            if((this.OriginalFlags & 0x4000) == 0x4000)
                flag |= 0x4000;

            if (this.IsFolder)
            {
                flag |= 0x200;

                if (this.IsAPackage || this.OffInPack != 0)
                    flag |= 0x1000;
            }

            //if((this.OriginalFlags & 0x800) == 0x800)
            //    flag |= 0x800; //DEBUG


            return flag;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
