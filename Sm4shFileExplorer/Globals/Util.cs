using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shFileExplorer.Globals
{
    public static class Util
    {
        public static byte[] ReadFilePartially(string file, int start, int end)
        {
            using (FileStream fstream = new FileStream(file, FileMode.Open))
            {
                BinaryReader breader = new BinaryReader(fstream);
                breader.BaseStream.Position = start;
                return breader.ReadBytes(end);
            }
        }
    }
}
