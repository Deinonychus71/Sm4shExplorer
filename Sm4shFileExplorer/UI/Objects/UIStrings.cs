using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sm4shFileExplorer.UI.Objects
{
    internal class UIStrings
    {
        public const string CAPTION_ERROR_LOADING_GAME_FOLDER = "Error loading game folder";
        public const string ERROR_LOADING_GAME_FOLDER = "It seems that the game folder is missing a few important element. In order for sm4shexplorer to work. Make sure the folder 'content' exists. This folder should contain the latest patch as well as the files 'ls' and 'dt00'/'dt01'.";
        public const string ERROR_LOADING_GAME_PATCH_FOLDER = "Warning. It seems that the game folder selected does not contain a patch folder.";
        public const string ERROR_LOADING_GAME_LOAD_FOLDER = "The game path can't be found anymore. Please modify the 'GamePath' value in sm4shmod.xml to fix this.";
        public const string ERROR_LOADING_PROJECT = "There was an error loading the project, please consult the logs.";

        public const string CAPTION_CREATE_PROJECT = "Create project";
        public const string CREATE_PROJECT_SUCCESS = "Project created!";
        public const string CREATE_PROJECT_FIND_FOLDER = "Please select the folder where your game dump exists. This is the folder that *contains* the folder called \"content\". For example, desktop/Dump/content/... you would select Dump. This \"content\" folder must contain the files dt00, dt01 and ls. For help with dumping Sm4sh, please refer to the tutorials section of the Sm4sh Gamebanana page.";

        public const string CAPTION_PACK_FOLDER = "Packing folder";
        public const string WARNING_PACK_FOLDER = "Warning! This is a highly experimental feature that should not be used on original content! Proceed?";

        public const string CAPTION_FILE_MODIFIED = "File modified";
        public const string INFO_FILE_MODIFIED = "The resource {0} seems to have been modified, do you want to include it to the project?";

        public const string CAPTION_OPERATION = "Working";
        public const string INFO_WORKING = "Sm4shExplorer is currently performing an operation. If you exit now, your changes won't be saved. Process?";

        public const string INFO_FILE_HEX = "You must first set up the path to your hex editor.";

        public const string CAPTION_PACK_REBUILD = "Rebuild resources";
        public const string WARN_EXPORT_FOLDER_EXISTS = "Warning. The folder '{0}' already exists. If you wish to continue, all content from this folder will be deleted. Proceed?";
        public const string INFO_PACK_REBUILD = "This feature will rebuild the resource files and patchfile. \r\nThis will ensure that the game takes your changes into account. When it's done, you can find the file in this folder:\r\n{0}";

        public const string INFO_PACK_SEND_SD = "Do you want to copy the newly exported mod to SD?";

        public const string INFO_THANKS = 
            "- Sammi Husky for his DTLSExtractor\r\n"+
            "- Soneek for introducing me to very nice, motivated people\r\n" +
            "- The Brawltools team for their amazing work";
    }
}
