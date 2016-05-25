using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shFileExplorer.DB
{
    public class CharsDB
    {
        #region Members
        private static Dictionary<int, string> _CharsDB;
        #endregion

        #region Properties
        public static Dictionary<int, string> Chars { get { return _CharsDB; } }
        #endregion

        #region Main Methods
        internal static void InitializeCharsDB(int gameVersion, bool is3DS)
        {
            if (is3DS)
                InitializeChars3DSDB(gameVersion);
            else
                InitializeCharsWiiUDB(gameVersion);
        }
        private static void InitializeCharsWiiUDB(int gameVersion)
        {
            if (gameVersion < 208)
                throw new Exception(string.Format("CharsDB: Version {0} for Wii U not supported.", gameVersion));
            _CharsDB = GenerateCharsDatabaseWiiU208();
        }
        private static void InitializeChars3DSDB(int gameVersion)
        {
            throw new Exception(string.Format("CharsDB: Version {0} for 3DS not supported.", gameVersion));
        }
        #endregion

        #region Wii U
        private static Dictionary<int, string> GenerateCharsDatabaseWiiU208()
        {
            Dictionary<int, string> chars = new Dictionary<int, string>();

            chars[0x0] = "Omakase";
            chars[0x1] = "Miifighter";
            chars[0x2] = "Miiswordsman";
            chars[0x3] = "Miigunner";
            chars[0x4] = "Mario";
            chars[0x5] = "Donkey";
            chars[0x6] = "Link";
            chars[0x7] = "Samus";
            chars[0x8] = "Yoshi";
            chars[0x9] = "Kirby";
            chars[0xa] = "Fox";
            chars[0xb] = "Pikachu";
            chars[0xc] = "Luigi";
            chars[0xd] = "Captain";
            chars[0xe] = "Ness";
            chars[0xf] = "Peach";
            chars[0x10] = "Koopa";
            chars[0x11] = "Zelda";
            chars[0x12] = "Sheik";
            chars[0x13] = "Marth";
            chars[0x14] = "Gamewatch";
            chars[0x15] = "Ganon";
            chars[0x16] = "Falco";
            chars[0x17] = "Wario";
            chars[0x18] = "Metaknight";
            chars[0x19] = "Pit";
            chars[0x1a] = "Szerosuit";
            chars[0x1b] = "Pikmin";
            chars[0x1c] = "Diddy";
            chars[0x1d] = "Dedede";
            chars[0x1e] = "Ike";
            chars[0x1f] = "Lucario";
            chars[0x20] = "Robot";
            chars[0x21] = "Toonlink";
            chars[0x22] = "Lizardon";
            chars[0x23] = "Sonic";
            chars[0x24] = "Drmario";
            chars[0x25] = "Rosetta";
            chars[0x26] = "Wiifit";
            chars[0x27] = "Littlemac";
            chars[0x28] = "Murabito";
            chars[0x29] = "Palutena";
            chars[0x2a] = "Reflet";
            chars[0x2b] = "Duckhunt";
            chars[0x2c] = "KoopaJr";
            chars[0x2d] = "Shulk";
            chars[0x2e] = "Purin";
            chars[0x2f] = "Lucina";
            chars[0x30] = "Pitb";
            chars[0x31] = "Gekkouga";
            chars[0x32] = "Pacman";
            chars[0x33] = "Rockman";
            chars[0x34] = "Mewtwo";
            chars[0x35] = "Ryu";
            chars[0x36] = "Lucas";
            chars[0x37] = "Roy";
            chars[0x38] = "Cloud";
            chars[0x39] = "Bayonetta";
            chars[0x3a] = "Kamui";
            chars[0xc8] = "Miiall";
            chars[0xc9] = "Koopag";
            chars[0xca] = "Littlemacg";
            chars[0xcb] = "Question";
            chars[0xcc] = "Unknown";
            chars[0xd2] = "Master";
            chars[0xd3] = "Mcore";
            chars[0xd4] = "Crazy";
            chars[0xd5] = "MasterCrazy";
            chars[0xd6] = "Bakudan";
            chars[0xd7] = "Sandbag";
            chars[0xd8] = "Ridley";
            chars[0xd9] = "MetalFace";
            chars[0xda] = "YellowDevil";
            chars[0xdb] = "Miienemyf";
            chars[0xdc] = "Miienemys";
            chars[0xdd] = "Miienemyg";
            chars[0xde] = "Miienemyall";
            chars[0xdf] = "Virus";
            chars[0xe0] = "Narration";
            chars[0xe1] = "Assist";
            chars[0xe2] = "Pokemon";
            chars[0xe3] = "Enemy";

            return chars;
        }
        #endregion
    }
}
