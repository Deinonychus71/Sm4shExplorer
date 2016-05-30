using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.UI.Objects
{
    internal class UIConstants
    {
        //Images
        public const string ICON_PACKED = "packed";
        public const string ICON_FOLDER = "folder";
        public const string ICON_FILE = "file";

        //Node colors
        public static Color NODE_LS = Color.Black;
        public static Color NODE_PATCH = Color.Blue;
        public static Color NODE_MOD = Color.Green;
        public static Color NODE_MOD_PACKED = Color.DarkOrange;
        public static Color NODE_MOD_UNLOCALIZED = Color.Red;
        public static Color NODE_MOD_DELETED = Color.Gray;
    }
}
