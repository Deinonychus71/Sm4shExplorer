using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.Objects
{
    internal unsafe class PatchListFile
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
                string absolutePath = Encoding.ASCII.GetString(patchFileSource.Slice(0x80 + (i * 0x80), 0x80));
                if (absolutePath.Contains('\0'))
                    absolutePath = absolutePath.Substring(0, absolutePath.IndexOf('\0'));
                string relativePath = absolutePath;
                if (relativePath.Contains("/"))
                    relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);
                if (absolutePath.Contains("packed"))
                    _Files[i] = new PatchFileItem(relativePath.Replace("packed", string.Empty), absolutePath.Replace("packed", string.Empty), true);
                else
                    _Files[i] = new PatchFileItem(relativePath, absolutePath, false);
            }
            patchFileSource.Close();
        }

        public PatchFileItem GetPatchFileItem(string absolutePath)
        {
            return _Files.FirstOrDefault(p => p.AbsolutePath == absolutePath);
        }
    }
}
