Sm4shExplorer
===========
Sm4shExplorer is a tool for managing the file-system of Super Smash Bros. for Wii U. It uses a game dump to build upon and create modified patch files for the game to use.

Please keep in mind that this program is still under development and may contain bugs.

The tool is originally developed by @Deinonychus71, whose repository for it can be found [here](https://github.com/Deinonychus71/Sm4shExplorer).

This fork contains changes made by @thefungus and @Dr-Hypercake.

For help on using this tool, join the [Sm4sh Modding Discord](https://discord.gg/EUZJhUJ), check this [written guide](https://gamebanana.com/tools/6294), check video tutorials on YouTube (make sure they are up to date), or check [other tutorials on Gamebanana](https://gamebanana.com/tuts/games/5547).

## Changes made by this fork
- Fix a long-standing issue with packing externally-patched files. Previously, certain stage folders would invariably crash the game when modified in a build through Sm4shExplorer. Now, they will not crash as long as the user includes all the affected externally patched files for a package when modifying it. See the [wiki](https://github.com/Dr-HyperCake/Sm4shExplorer/wiki#including-files-with-stage-folders-to-prevent-crashes) for more info and a list of files.
  - These aforementioned externally-patched files will also now extract decompressed, like other files do.
- Add the option to ignore regional partitions of the user's choosing. By default, Sm4shExplorer loads and uses all regional partitions, including those of languages that the user does not need. There is now the option to not load or use partitions by specifying them in the configuration xml. See the [wiki](https://github.com/Dr-HyperCake/Sm4shExplorer/wiki#using-partition-ignoring) for more info.
- Update a lot of UI and message strings. This is to fix grammar, terminology and wording.
- Send to SD for newer SDCafiine versions
- Send to SD now supports USB devices (FAT32) (SDCafiine 1.4+)
- Added keyboard shortcuts for a few things
 - F5 to refresh treeview
 - Ctrl+b to build the modpack
 - Ctrl+Shift+b to build the modpack with no packing
 - Ctrl+r to Send to SD/USB, Ctrl+d to Send to SD/USB (no packing)
- Send to SD asks what you want to name your modpack, uses current workspace folder name as default

## Base info
### What does it do?
- Allow viewing of the whole file-system of SSBU (including regional partitions) in a tree-view by utilizing [DTLSExtractor from Sammi-Husky](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/DTLS). The tree-view will show update files as well; base-game files are in black text, patched files are in blue, and user-modified files are in green.
- View useful information about any file, such as name, path, size, flags and source.
- Extract any file or folder to be viewed and modified.
- Quickly open files in a hex editor from within the program (requires configuration).
- Replace files and add new ones. If a file is modified or added in a packed file, the whole packed file will be rebuilt automatically during exporting. A modified file will appear with green text in the tree-view.
- For localized partitions, "unlocalize" a folder or a file so that the game loads the unlocalized file instead.
- Lets you remove original files from the game, so that they will be ignored when re-building.
- Automatically rebuild the changes you've made into a mod build, so that they can be loaded by the game from an SD card or USB.
- Plugin system to add more features to the program.
- Plugin Sm4shMusic to manage the game's songs, add new songs, and assign songs to different stages (utilizes [vgmstream from kode54](https://github.com/kode54/vgmstream)).
- Plugin Sm4shParamEditor to use [Sammi-Husky's Param Editor](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/PARAM) directly in Sm4shExplorer.
 
### What do I need?
- A dump of the game on your computer (including folders "content", "code" and "meta"). This folder is only read by Sm4shExplorer to build upon and will not be modified.
- A dump of the latest patch in the game dump. The latest version of the game is v304 (1.1.7), you need at least v208 (1.1.4).
- On your SD: Backup your 'content/patch' folder before doing ANY CHANGE.

### What do I need? (compiling)
- Visual Studio 2015 and .NET Framework 4.5
- Libs zlib32.dll, zlib64.dll, zlibnet.dll and DTLS.exe for the main software.
- Libs libg719_decode.dll, libg7221_decode.dll, libmpg123-0.dll, libvorbis.dll, NAudio.dll and libvgmstream.dll from Deinonychus71's repository.

### Future plans
- Pack any folder, including added folders.
- Manually choose which files need to be automatically compressed.
- Command line interface
- Extract vanilla file instead of modded file
- Only rebuild files that changed since last build
