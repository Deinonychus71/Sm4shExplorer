Sm4shExplorer
===========
Sm4shExplorer is a tool for managing the file-system of Super Smash Bros. for Wii U. It uses a game dump to build upon and create modified patch files for the game to use.

Please keep in mind that this program is still under development and may contain bugs.

The tool is originally developed by Deinonychus71, and has had improvements made by thefungus, Dr-HyperCake, jugeeya and jam1garner.

For help on using this tool, you can:
- Join the [Sm4sh Modding Discord](https://discord.gg/EUZJhUJ)
- Check this [written guide](https://gamebanana.com/tools/6294)
- Check [other tutorials on GameBanana](https://gamebanana.com/tuts/games/5547)
- Check video tutorials on YouTube (make sure they are up to date)

### What does it do?
- Allow viewing of the whole file-system of SSBU (including regional partitions) in a tree-view by utilizing [DTLSExtractor from Sammi-Husky](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/DTLS). The tree-view will show update files as well; base-game files are in black text, patched files are in blue, and user-modified files are in green.
- View useful information about any file, such as name, path, size, flags and source.
- Extract any file or folder to be viewed and modified.
- Replace files and add new ones. If a file is modified or added in a packed file, the whole packed file will be rebuilt automatically during exporting. A modified file will appear with green text in the tree-view.
- Allow removal of original files from the game, so that they will be deleted in the build.
- Automatically rebuild the changes you've made into a mod build, so that they can be loaded by the game from an SD card or USB.
- Prompt to automatically send the mod build to an SD card or USB device (FAT32), for use with SDCafiine.
- For localized partitions, "unlocalize" a folder or a file so that the game loads the unlocalized file instead.
- Ignore unneeded regional partitions within the application by specifying them in the configuration xml. See the [wiki](https://github.com/thefungus/Sm4shExplorer/wiki#using-partition-ignoring) for more info.
- Quickly open files in a hex editor from within the program (requires configuration).
- Various keyboard shortcuts for convenience. See the [wiki](https://github.com/thefungus/Sm4shExplorer/wiki#keyboard-shortcuts) for a list.
- Plugin system to add more features to the program.
- Plugin Sm4shMusic to manage the game's songs, add new songs, and assign songs to different stages (utilizes [vgmstream from kode54](https://github.com/kode54/vgmstream)).
- Plugin Sm4shParamEditor to use [Sammi-Husky's Param Editor](https://github.com/Sammi-Husky/Sm4sh-Tools/tree/master/PARAM) directly in Sm4shExplorer.

### What do I need?
- A dump of the game on your computer (including folders "content", "code" and "meta"). This folder is only read by Sm4shExplorer to build upon and will not be modified.
- A dump of the latest patch in the game dump. The latest version of the game is v304 (1.1.7), you need at least v208 (1.1.4).
- Backup your previous mod build before replacing it, just in case something ends up broken in it.

### What do I need? (compiling)
- Visual Studio 2015 and .NET Framework 4.5
- Libs zlib32.dll, zlib64.dll, zlibnet.dll and DTLS.exe for the main software.
- Libs libg719_decode.dll, libg7221_decode.dll, libmpg123-0.dll, libvorbis.dll, NAudio.dll and libvgmstream.dll from Deinonychus71's repository.

### Future plans
- Pack any folder, including added folders.
- Manually choose which files need to be automatically compressed.
- Extract an unpatched file even when it is patched.
- Extract a vanilla file even when it is replaced by a modded one.
- Only rebuild files that have changed since the last build.
