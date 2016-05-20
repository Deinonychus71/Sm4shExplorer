using DTLS;
using System;
using System.Collections.Generic;

namespace Sm4shFileExplorer.Objects
{
    public class ResourceItem : ICloneable
    {
        private bool _IsPackage;

        public uint OriginalFlags { get; set; }

        public ResourceCollection ResourceCollection { get; set; }
        public ResourceItem OriginalResourceItem { get; set; }
        public List<ResourceItem> Nodes { get; set; } //To be used only for treeview, is not cloned for now.

        public PatchFileItem PatchItem { get; set; }
        public FileSource Source { get; set; }
        public LSEntry LSEntryInfo { get; set; }
        public string RelativePath { get; private set; }
        public string AbsolutePath { get; private set; }
        public bool IsFolder { get { return Filename.EndsWith("/"); } }

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
        public uint FolderDepth { get { return (uint)this.RelativePath.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length; } }

        public ResourceItem(ResourceCollection resCol, string filename, uint offinpack, uint cmpsize, uint decsize, bool ispackage, string relativePath)
        {
            ResourceCollection = resCol;
            Filename = filename;
            OffInPack = offinpack;
            CmpSize = cmpsize;
            DecSize = decsize;
            IsAPackage = ispackage;
            RelativePath = relativePath;
            AbsolutePath = ResourceCollection.PartitionName + "/" + relativePath;
            Nodes = new List<ResourceItem>();
        }

        private uint CalculateFlags()
        {
            uint flag = 0x00000000;

            flag |= FolderDepth;

            if (this.IsAPackage)
                flag |= 0x400;

            if (!this.ResourceCollection.IsRegion) //Everything in the main resource has the flag
                flag |= 0x800;
            else if (this.ResourceCollection.Resources.ContainsKey(this.RelativePath) && this.ResourceCollection.Resources[this.RelativePath].Source != FileSource.NotFound) //Everything physically present in region partition has the flag
                flag |= 0x800;

            /*if (GlobalConstants.FORCE_ACCURATE_LOCALIZATION_FLAG)
            {
                //NOT SETTING THOSE FLAGS DOESNT BREAK THE GAME, THEY DON'T REALLY MAKE SENSE ANYWAY...
                if (this.OffInPack != 0 || (this.IsFolder && !this.IsAPackage)) //Everything in package or folder has the flag 0x800
                    flag |= 0x800;
                else if (this.RelativePath.StartsWith("ui/replace/customize/") || this.RelativePath.StartsWith("ui/replace/clear/")) //Everything in those folders has the flag (I think spi_10 and clear_11 meant to be packages)
                    flag |= 0x800;
                else if(this.RelativePath.StartsWith("model/enemy/") || this.RelativePath.StartsWith("model/miihat/")) //model enemies and hats have all the flag
                    flag |= 0x800;
                //else if (this.IsAPackage && this.IsFolder && this.Path.StartsWith("fighter/")) //EXCEPT for the characters that are potentially localized (ike, purin, ryu...), char
                //    flag |= 0x800;
            }*/

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
