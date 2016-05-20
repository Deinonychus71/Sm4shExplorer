namespace Sm4shFileExplorer.Objects
{
    public class PatchFileItem
    {
        public string AbsolutePath { get; private set; }
        public string RelativePath { get; private set; }
        public bool Packed { get; private set; }

        public PatchFileItem(string relativePath, string absolutePath, bool packed)
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
            Packed = packed;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
