using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.Globals
{
    internal class CustomStringComparer : IComparer<string>
    {
        public CustomStringComparer()
        {
        }

        public int Compare(string x, string y)
        {
            if (x == null)
                return y == null ? 0 : -1;
            if (y == null)
                return 1;

            int result = x.Length - y.Length;
            int comparisons = result > 0 ? y.Length : x.Length;
            int difference, index = 0;
            while (index < comparisons)
            {
                int xval = (int)x[index];
                int yval = (int)y[index];
                if (xval == 92 || xval == 47)
                    xval = 0;
                if (yval == 92 || yval == 47)
                    yval = 0;
                difference = (int)(xval - yval);
                if (difference != 0)
                {
                    result = difference;
                    break;
                }
                index++;
            }
            return result;
        }
    }
}
