using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.Globals
{
    internal class UIConstants
    {
        //Configuration
        public const string CONFIG_FILE = "sm4shmod.xml";

        //PROJECT CREATION
        public const string PROJECT_FOLDER = "Sm4shProjects";

        //Images
        public const string ICON_PACKED = "packed";
        public const string ICON_FOLDER = "folder";
        public const string ICON_FILE = "file";

        public static Color NODE_LS = Color.Black;
        public static Color NODE_PATCH = Color.Blue;
        public static Color NODE_MOD = Color.Green;
        public static Color NODE_MOD_UNLOCALIZED = Color.Red;
        public static Color NODE_MOD_DELETED = Color.Gray;
    }
}
