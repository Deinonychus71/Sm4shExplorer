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
- For region folders, "unlocalize" a folder or a file so that the game loads the unlocalized (english) file instead.
- Let you "remove" original resources from the game (experimental, works on stage models)
- Repack files and rebuild resources and patchlist into a specific "export" folder.
- Plugin system to add more features
- Plugin Sm4shMusic to manage the list of musics and assign them to different stages (use [vgmstream from kode54](https://github.com/kode54/vgmstream)
- Plugin Sm4shParamEditer to use [Sammi-Husky's Param Editor](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/PARAM) directly in Sm4shExplorer.
 
##What do I need?##
- An extration of the game on your computer (folders "content", "code" and "meta"). The will folder will have to remain untouched at all time to avoid issues.
- The latest patch, unmodified (current is v288, you need at least v208) in the same directory (so that "content" > "patch")
- On your SD: Backup your 'content/patch' folder before doing ANY CHANGE.
- Visual Studio 2015 and .NET Framework 4.5
- Libs zlib32.dll/zlib64.dll/zlibnet.dll/DTLS.exe for the main soft
- Libs libg719_decode.dll/libg7221_decode.dll/libmpg123-0.dll/libvorbis.dll/NAudio.dll and libvgmstream.dll from my repo for Sm4shMusic

##Future plans##
- Pack any folder, including added folders.
- Manually choose what file needs to be automatically compressed
