Sm4shExplorer
===========
Sm4shExplorer is a WIP tool to add and replace any file for Smash Bros for Wii U. It uses the patch folder and modify the resource files of the game.

Please keep in mind that this is a early version, made purposely to try messing around with the game files and see what can be done with it. It probably contains bugs / performance issues.

##What does it do?##
- It will let you see the whole filesystem of Smash (main data + regions) in a treeview using [DTLSExtractor from Sammi-Husky](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/DTLS). The treeview will show the updated version of the game (core files in black + patch files in blue)
- Give you a few informations about any file (name, path, size, flags, source...)
- Extract any file/folder to a specific "extract" folder
- Let you open any file with a hex editor (needs to be configured)
- Replace/Add files. If a file is modified/added in a packed file, the whole packed file will be rebuilt automatically during export. A modified "mod file will appear in green.
- For region folders, "unlocalize" a folder or a file so that the game loads the unlocalized (english) file instead. (WORKS ONLY WITH US VERSION FOR NOW)
- Repack files and rebuild resources and patchlist into a specific "export" folder.
 
##What do I need?##
- The latest patch, unmodified (v208, it might work for earlier versions, but you will need at least the patch folder to make it work)
- An extration of the game on your computer (folders "content", "code" and "meta"). The will folder will have to remain untouched at all time to avoid issues.
- On your SD: Backup your 'content/patch' folder before doing ANY CHANGE.
- Visual Studio 2012 and .NET Framework 4.5

##Future plans##
- Pack any folder, including added folders.
- Manually choose what file needs to be automatically compressed
- A plugin system for certain file/extensions to help modifying them
