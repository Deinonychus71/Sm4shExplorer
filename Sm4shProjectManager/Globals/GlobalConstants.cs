using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Globals
{
    public class GlobalConstants
    {
        public const string VERSION = "0.04";

        //TO BE USED FOR DEBUG PURPOSE
        public const bool FORCE_ACCURATE_LOCALIZATION_FLAG = false; //Default is false, true will add a few more rules to set the 0x800 flag (useless if FORCE_ORIGINAL_FLAGS is set to true)
        public const bool SORT_RESOURCE = true; //Default value is true, false will prevent the program  from sorting the resources before rebuilding the files (might break new files)
        public const bool FORCE_ORIGINAL_COMPRESSION = true; //Default value is true, false will let the program choose whether a file should be compressed or not when adding/replacing it (default is > 1Kb = compress)

        //Utils
        public const string FOLDER_SEPARATOR = "\\";

        public const int SIZE_FILE_COMPRESSION_MIN = 1024;
    }
}
