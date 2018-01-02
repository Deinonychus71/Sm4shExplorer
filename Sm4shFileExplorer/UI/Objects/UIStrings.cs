using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.UI.Objects
{
    internal class UIStrings
    {
        public const string CAPTION_ERROR_LOADING_GAME_FOLDER = "Error loading game folder";
        public const string ERROR_LOADING_GAME_FOLDER = "It seems that the game folder is missing a few important elements. In order for Sm4shExplorer to work, make sure the folder 'content' exists. This folder should contain the latest patch data as well as the files 'ls', 'dt00' and 'dt01'.";
        public const string ERROR_LOADING_GAME_PATCH_FOLDER = "Warning: the game folder selected does not contain a patch folder.";
        public const string ERROR_LOADING_GAME_LOAD_FOLDER = "The game path can no longer be found. Please modify the 'GamePath' value in sm4shmod.xml to fix this.";
        public const string ERROR_LOADING_PROJECT = "There was an error loading the project, please consult the logs.";

        public const string CAPTION_CREATE_PROJECT = "Create project";
        public const string CREATE_PROJECT_SUCCESS = "Project created!";
        public const string CREATE_PROJECT_FIND_FOLDER = "Please select the root folder of your game dump. This is the folder that *contains* the folder called \"content\". For example, 'desktop/Dump/vol/content/...' you would select 'vol'. This \"content\" folder must contain the files dt00, dt01 and ls. For help with dumping Sm4sh, please refer to the tutorials section of the Sm4sh GameBanana page.";

        public const string CAPTION_PACK_FOLDER = "Packing folder";
        public const string WARNING_PACK_FOLDER = "Warning! This is a highly experimental feature that should not be used on original content! Proceed?";

        public const string CAPTION_FILE_MODIFIED = "File modified";
        public const string INFO_FILE_MODIFIED = "The resource {0} seems to have been modified, do you want to include it to the project?";

        public const string CAPTION_OPERATION = "Working";
        public const string INFO_WORKING = "Sm4shExplorer is currently performing an operation. Are you sure you want to exit?";

        public const string INFO_FILE_HEX = "You must first set up the path to your hex editor.";

        public const string CAPTION_PACK_REBUILD = "Rebuild resources";
        public const string WARN_EXPORT_FOLDER_EXISTS = "Warning. The folder '{0}' already exists. If you wish to continue, all content from this folder will be deleted. Proceed?";
        public const string INFO_PACK_REBUILD = "Ready to build.\r\nYou can find the build in this folder:\r\n{0}";

        public const string INFO_PACK_SEND_SD = "Do you want to copy the build to your SD card?";

        public const string INFO_THANKS = 
            "- Sammi Husky for his DTLSExtractor\r\n"+
            "- Soneek for introducing me to very nice, motivated people\r\n" +
            "- The Brawltools team for their amazing work";
    }
}
