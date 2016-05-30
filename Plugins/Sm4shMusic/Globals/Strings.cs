using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shMusic.Globals
{
    public class Strings
    {
        public const string CAPTION_HELP = "Help";

        public const string HELP_BGM_MANAG_TITLE = "Edit the title of the song that will be displayed in MyMusic.";
        public const string HELP_BGM_MANAG_TITLE_SOUNDTEST = "Edit the title of the song that will be displayed in Sound Test.";
        public const string HELP_BGM_MANAG_DESCRIPTION = "Edit the description of the song.";
        public const string HELP_BGM_MANAG_DESCRIPTION2 = "Edit the second description of the song (Sound test).";
        public const string HELP_BGM_MANAG_SOURCE = "Edit the source of the song.";
        public const string HELP_BGM_MANAG_BGM = "- Choose a BGM that will be used in associated with this SOUND ID. In order for a SOUND ID to be valid, at least one song must be associated.\r\n- The game accepts up to 5 BGM per Sound (for stages like Mario Maker).\r\n- Custom BGMs must be located in 'content\\sound\\bgm' in the workspace folder. Do NOT copy the original BGM in the workspace!\r\n- To find new songs, you can either create your own or go to 'http://smashcustommusic.com/'.\r\n- A BGM must start with 'snd_bgm_' and ends with '.nus3bank'.\r\n- A nus3bank is NOT A MP3.";
        public const string HELP_BGM_MANAG_SOUND_SOURCE = "- CoreGameSound: This sound is from the core game.\r\n- DLCSound: This sound is from a DLC (probably locked)\r\n- UpdateSound: This sound came with a free update.\r\n- TournamentSong: Special value for tournament songs (?)\r\n\r\nJust leave it to CoreGameSound. It will work.";
        public const string HELP_BGM_MANAG_SOUND_MIXTYPE = "- Original: Will show an 'Original' logo.\r\n- Remix: Will show a 'Remix' logo in white.\r\n- SmashOriginal: Will not show anything.\r\n- RemixRed: Will show a 'Remix' logo in Red.";
        public const string HELP_BGM_MANAG_SOUND_ICON = "Will show a specific icon in Sound Test for this music.";
        public const string HELP_BGM_MANAG_ROTATION_BEHAVIOR = "- NULL: No value set. You don't want that.\r\n- RosterRandom: Any of the unlocked character can appear in the background in Sound Test.\r\n - CharacterRandom: Any of the character selected in Background Rotation can appear in the background in Sound Test.\r\n - CharacterFirst: Only the first character of the list will appear in the background Sound Test.\r\n- UnlockableCharacterFirst: If character unlocked, will appear in the background, if not, then the others.\r\n\r\nThe last 2 options are quite weird. Just use CharacterRandom if you don't know.";
        public const string HELP_BGM_MANAG_ROTATION_BACKGROUND = "Choose up to 8 characters that can be used as a background image while selecting a song in Sound Test.";
        public const string HELP_BGM_MANAG_ORDER_SOUND_TEST = "Opens a panel to reorder the list of songs for Sound Test.";
        public const string HELP_BGM_MANAG_ORDER_STAGE_CREATION = "Opens a panel to reorder the list of songs for Stage Creation.";
        public const string HELP_BGM_MANAG_GROUP_STAGE_CREATION = "Choose in which group a music must be associated in Stage Creation.";
        public const string HELP_BGM_MANAG_INSOUNDTEST = "Check to show the music in Sound Test.";
        public const string HELP_BGM_MANAG_REGIONJAP = "Check if the song is available in the japanese region. This value change with a few limited songs, like Lost in Thoughts All Alone.";
        public const string HELP_BGM_MANAG_REGIONUSAEUR = "Check if the song is available in the us/(eur?) region. This value change with a few limited songs, like Lost in Thoughts All Alone.";
        public const string HELP_BGM_MANAG_UNKNOWN = "The behavior of this value is unknown! If you can figure it out, please tell us :)";

        public const string HELP_PROPERTIESBGM_BGMID = "ID of the BGM.";
        public const string HELP_PROPERTIESBGM_BGMNAME = "Internal name of the BGM.";
        public const string HELP_PROPERTIESBGM_BGMFILENAME = "Filename of the BGM.";
        public const string HELP_PROPERTIESBGM_MENUCHECKPOINT = "Values used only in a few BGMs of the main menu. Apparently used to return to a certain time of the music after a match.\r\n\r\nLeave the default value for all the stage songs.";
        public const string HELP_PROPERTIESBGM_LOOPSTARTSAMPLE = "ID of the first sample of the loop. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_LOOPENDSAMPLE = "ID of the last sample of the loop. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_TOTALSAMPLES = "Total number of samples. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_LOOPSTARTTIME = "Start time of the loop in milliseconds. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_LOOPENDTIME = "End time of the loop in milliseconds. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_TOTALTIME = "Total time in milliseconds. Normally automatically setted when adding a new BGM. If VGMStream failed to initialize, use the vgmstream plugin in foobar to retrieve this value manually.";
        public const string HELP_PROPERTIESBGM_UNKNOWN = "The behavior of this value is unknown! If you can figure it out, please tell us :)";

        public const string HELP_MUSIC_STAGE_MYMUSIC_ID = "ID of the stage in MyMusic DB.";
        public const string HELP_MUSIC_STAGE_SOUNDDB_ID = "ID of the stage in SOUND DB.";
        public const string HELP_MUSIC_STAGE_INDEX = "Order in which a music will appear in the UI.";
        public const string HELP_MUSIC_STAGE_SUBINDEX = "Usually used for Sounds that have more than 1 BGM. Probably used internally by the game as an ID to know when to load what.";
        public const string HELP_MUSIC_STAGE_RARITY = "From 0 to 100, indicate the odds to have one specific song played in a stage.\r\n\r\nWhile loading the game, if the rarity level stays at 0, try resetting the settings for this stage. It's probably the save file that didn't know about a new song you just added.";
        public const string HELP_MUSIC_STAGE_MYMUSIC = "List of the MyMusic songs associated to this stage. In order for a song to be visible and playable. They usually need to appear in both lists (with some not-so-well-known exceptions).";
        public const string HELP_MUSIC_STAGE_SOUND = "List of the SoundDB songs associated to this stage. This list represents the list you will see in the UI of the game. In order for a song to be visible and playable. They usually need to appear in both lists (with some not-so-well-known exceptions).";
        public const string HELP_MUSIC_STAGE_PLAY_DELAY = "This value determines how long to wait before playing the song in a match. -1 apparently means that the song has to start during the 'GO'.\r\nThank you Pib from gbatemp :-)";
        public const string HELP_MUSIC_STAGE_UNKNOWN = "The behavior of this value is unknown! If you can figure it out, please tell us :)";

        public const string CAPTION_WARNING = "Warning";
        public const string WARNING_RESTORE_DATA = "Warning : Using this feature will undo your changes and restore the data from the project file, continue?";
        public const string WARNING_COPY_SOUND = "Add the BGM(s) in MyMusic as well ?";
        public const string WARNING_COPY_SOUND_REMOVE = "Remove the BGM(s) from MyMusic as well ?";
        public const string WARNING_COPY_MYMUSIC = "Warning : In order for a BGM to be played for a stage, it must also be added in the Sound DB, proceed?";
        public const string WARNING_VGMSTREAM_LOAD_BGM = "Warning: VGMStream can't load '{0}'. Either the file isn't a real nus3bank (bad!) or VGMStream failed to load (won't matter).";
        public const string WARNING_VGMSTREAM_CREATE_BGMENTRY = "Warning: VGMStream can't load '{0}'. You will need to set up the properties of the BGM on manually.";

        public const string DEBUG_EXPORT = "The export of the debug csv '{0}' was successful.";
        public const string DEBUG_LIST_ORPHAN_BGMS = "List of orphan BGMs ({0} orphans) : {1}";

        public const string CAPTION_ERROR = "Error";
        public const string ERROR_STAGE_ITEM_MAX = "You cannot add more than {0} items.";
        public const string ERROR_SOUND_ADD = "Error : This sound already exists in the list!";

        public const string CAPTION_INFO = "Info";
        public const string INFO_COMPILED = "The modifications were successfully saved in the workspace folder. If you wish to restore the original file, simply remove 'sound/config/bgm_property.mpb', 'param/ui/ui_sound_db.bin', 'sound/config/bgm_mymusic.mmb' and 'ui/message/sound.msbt' from the workspace folder.";

        public const string INFO_CONFIRM_DELETE = "Are you sure you want to remove '{0}'? If deleting one of the first 521 'official' IDs it could mess with how songs are unlocked. This action cannot be undone.";

        public const string INFO_THANKS =
            "- Research on SOUND DB, MyMusic, NUS3BANK and for http://smashcustommusic.com/: Soneek\r\n" +
            "- MSBT Label hash: Kinnay - http://rhcafe.us.to/?page=wiki&id=MSBT_%28File_Format%29\r\n" +
            "- VGMStream: halleyscometsw, kode54, lioncash for C# wrapper";

        public const string DEFAULT_SENTRY_TITLE = "BLANK";
        public const string DEFAULT_SENTRY_TITLE2 = "BLANK";
    }
}
