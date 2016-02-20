using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Objects
{
    public unsafe class PatchListFile
    {
        private byte[] _Header;
        private PatchFileItem[] _Files;

        public PatchFileItem[] Files
        {
            get { return _Files; }
        }

        public byte[] Header { get { return _Header; } }

        public PatchListFile(string patchFile)
        {
            DataSource patchFileSource = new DataSource(FileMap.FromFile(patchFile));
            _Header = patchFileSource.Slice(0, 0x80);
            VoidPtr addr = patchFileSource.Address + 4;

            _Files = new PatchFileItem[*(uint*)addr];
            for (int i = 0; i < _Files.Length; i++)
            {
                string path = Encoding.ASCII.GetString(patchFileSource.Slice(0x80 + (i * 0x80), 0x80));
                if (path.Contains('\0'))
                    path = path.Substring(0, path.IndexOf('\0'));
                if (path.Contains("packed"))
                {
                    string pathWithoutPacked = path.Replace("packed", string.Empty);
                    _Files[i] = new PatchFileItem() { RegionPath = pathWithoutPacked, Path = pathWithoutPacked.Substring(pathWithoutPacked.IndexOf("/") + 1), Packed = true };
                }
                else
                {
                    string pathWithoutRegion = path;
                    if (pathWithoutRegion.Contains("/"))
                        pathWithoutRegion = path.Substring(path.IndexOf("/") + 1);
                    _Files[i] = new PatchFileItem() { RegionPath = path, Path = pathWithoutRegion, Packed = false };
                }
            }
            patchFileSource.Close();
        }

        public PatchFileItem GetRegionPath(string path, string region)
        {
            return _Files.FirstOrDefault(p => p.RegionPath == "data" + region + "/" + path);
        }

        public PatchFileItem GetPath(string path)
        {
            return _Files.FirstOrDefault(p => p.Path == path);
        }
    }
}
