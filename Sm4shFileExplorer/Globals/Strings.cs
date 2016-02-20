using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.Globals
{
    public class Strings
    {
        public const string CAPTION_ERROR_LOADING_GAME_FOLDER = "Error loading game folder";
        public const string ERROR_LOADING_GAME_FOLDER = "It seems that the game folder is missing a few important element. The following files must exist:\r\n{0}\r\n{1}\r\n{2}";
        public const string ERROR_LOADING_GAME_PATCH_FOLDER = "Warning. It seems that the game folder selected does not contain a patch folder.";
        public const string ERROR_LOADING_GAME_LOAD_FOLDER = "Error, the path '{0}' can't be found anymore. Please modify the config file sm4shmod.xml to fix this.";

        public const string CAPTION_CREATE_PROJECT = "Create project";
        public const string CREATE_PROJECT_SUCCESS = "Project created!";
        public const string CREATE_PROJECT_FIND_FOLDER = "First, please indicate the folder where Sm4shExplorer can find the latest version of the game. This folder must contain the folders 'code', 'content' and 'meta'.\r\n\r\nThis folder will not be modified by this program.";

        public const string CAPTION_PACK_FOLDER = "Packing folder";
        public const string WARNING_PACK_FOLDER = "Warning! This is a highly experimental feature that should not be used on original content! Proceed?";

        public const string CAPTION_FILE_MODIFIED = "File modified";
        public const string INFO_FILE_MODIFIED = "The resource {0} seems to have been modified, do you want to include it to the project?";

        public const string CAPTION_FILE_HEX = "Opening file";
        public const string INFO_FILE_HEX = "You must first set up the path to your hex editor.";

        public const string CAPTION_PACK_REBUILD = "Rebuild resources";
        public const string INFO_PACK_REBUILD = "This feature will rebuild the resource files and patchfile. \r\nThis will ensure that the game takes your changes into account. When it's done, you can find the file in this folder:\r\n{0}";

        public const string INFO_THANKS = 
            "- Sammi Husky for his DTLSExtractor\r\n"+
            "- Soneek for introducing me to very nice, motivated people\r\n" +
            "- The Brawltools team for their amazing work";
    }
}
