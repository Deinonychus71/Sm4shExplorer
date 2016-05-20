using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shSound.DB
{
    public class BGMStageDB
    {
        #region Members
        private static Dictionary<uint, BGMStage> _BGMMyMusicDB;
        #endregion

        #region Properties
        public static Dictionary<uint, BGMStage> BGMMyMusics { get { return _BGMMyMusicDB; } }
        #endregion

        #region Main Methods
        public static void InitializeBGMMyMusicDB(int gameVersion, bool is3DS)
        {
            if (is3DS)
                InitializeBGMMyMusic3DSDB(gameVersion);
            else
                InitializeBGMMyMusicWiiUDB(gameVersion);
        }
        private static void InitializeBGMMyMusicWiiUDB(int gameVersion)
        {
            if (gameVersion < 208)
                throw new Exception(string.Format("Version {0} for Wii U not supported.", gameVersion));
            _BGMMyMusicDB = GenerateBGMMyMusicDatabaseWiiU208();
        }
        private static void InitializeBGMMyMusic3DSDB(int gameVersion)
        {
            throw new Exception(string.Format("Version {0} for Wii U not supported.", gameVersion));
        }
        #endregion

        #region Wii U
        private static Dictionary<uint, BGMStage> GenerateBGMMyMusicDatabaseWiiU208()
        {
            Dictionary<uint, BGMStage> BGMStages = new Dictionary<uint, BGMStage>();

            BGMStages[0x0] = new BGMStage("BattleField_f", 0x0, 0x1);
            BGMStages[0x1] = new BGMStage("BattleFieldL_f", 0x1, 0x2f);
            BGMStages[0x2] = new BGMStage("End_f", 0x2, 0x2);
            BGMStages[0x3] = new BGMStage("MarioU", 0x3, 0x4);
            BGMStages[0x4] = new BGMStage("Galaxy", 0x4, 0x3);
            BGMStages[0x5] = new BGMStage("XDolpic", 0x5, 0x7);
            BGMStages[0x6] = new BGMStage("MarioKart", 0x6, 0x5);
            BGMStages[0x7] = new BGMStage("XKart", 0x7, 0x8);
            BGMStages[0x8] = new BGMStage("XMansion", 0x8, 0x6);
            BGMStages[0x9] = new BGMStage("DKReturns", 0x9, 0x9);
            BGMStages[0xa] = new BGMStage("64Jungle", 0xa, 0xa);
            BGMStages[0xb] = new BGMStage("Skyward", 0xb, 0xb);
            BGMStages[0xc] = new BGMStage("XOldin", 0xc, 0xc);
            BGMStages[0xd] = new BGMStage("DxShrine", 0xd, 0x26);
            BGMStages[0xe] = new BGMStage("Metroid", 0xe, 0xd);
            BGMStages[0xf] = new BGMStage("XNorfair", 0xf, 0xe);
            BGMStages[0x10] = new BGMStage("XFzero", 0x10, 0x17);
            BGMStages[0x11] = new BGMStage("YoshiU", 0x11, 0xf);
            BGMStages[0x12] = new BGMStage("DxYorster", 0x12, 0x10);
            BGMStages[0x13] = new BGMStage("Cave", 0x13, 0x11);
            BGMStages[0x14] = new BGMStage("XHalberd", 0x14, 0x12);
            BGMStages[0x15] = new BGMStage("Assault", 0x15, 0x13);
            BGMStages[0x16] = new BGMStage("XStarfox", 0x16, 0x14);
            BGMStages[0x17] = new BGMStage("Kalos", 0x17, 0x15);
            BGMStages[0x18] = new BGMStage("XStadium", 0x18, 0x16);
            BGMStages[0x19] = new BGMStage("DxOnett", 0x19, 0x1c);
            BGMStages[0x1a] = new BGMStage("Colloseum_f", 0x1a, 0x19);
            BGMStages[0x1b] = new BGMStage("XEmblem", 0x1b, 0x1a);
            BGMStages[0x1c] = new BGMStage("Gw3", 0x1c, 0x2a);
            BGMStages[0x1d] = new BGMStage("Angeland", 0x1d, 0x1f);
            BGMStages[0x1e] = new BGMStage("XPalutena", 0x1e, 0x20);
            BGMStages[0x1f] = new BGMStage("Gamer", 0x1f, 0x1b);
            BGMStages[0x20] = new BGMStage("PikminU", 0x20, 0x18);
            BGMStages[0x21] = new BGMStage("Village2", 0x21, 0x1d);
            BGMStages[0x22] = new BGMStage("XVillage", 0x22, 0x1e);
            BGMStages[0x23] = new BGMStage("WiiFit", 0x23, 0x23);
            BGMStages[0x24] = new BGMStage("PunchOut", 0x24, 0x24);
            BGMStages[0x25] = new BGMStage("PunchOut2", 0x25, 0x24);
            BGMStages[0x26] = new BGMStage("XenoBlade", 0x26, 0x25);
            BGMStages[0x27] = new BGMStage("DuckHunt", 0x27, 0x27);
            BGMStages[0x28] = new BGMStage("XDonkey", 0x28, 0x29);
            BGMStages[0x29] = new BGMStage("Wrecking", 0x29, 0x28);
            BGMStages[0x2a] = new BGMStage("Pilotwings", 0x2a, 0x22);
            BGMStages[0x2b] = new BGMStage("Wufu", 0x2b, 0x21);
            BGMStages[0x2c] = new BGMStage("Windyhill", 0x2c, 0x2d);
            BGMStages[0x2d] = new BGMStage("Wily", 0x2d, 0x2b);
            BGMStages[0x2e] = new BGMStage("Pacland", 0x2e, 0x2c);
            BGMStages[0x2f] = new BGMStage("Event_Map", 0x2f, null);
            BGMStages[0x30] = new BGMStage("Order_CrazySide", 0x30, null);
            BGMStages[0x31] = new BGMStage("Order_CrazySide_Success", 0x31, null);
            BGMStages[0x32] = new BGMStage("Order_Reward_Get", 0x32, null);
            BGMStages[0x33] = new BGMStage("Order_Reward_Lost", 0x33, null);
            BGMStages[0x34] = new BGMStage("Order_MasterSide", 0x34, null);
            BGMStages[0x35] = new BGMStage("Allstar_f", 0x35, null);
            BGMStages[0x36] = new BGMStage("OptionSound", 0x36, null);
            BGMStages[0x37] = new BGMStage("OnlineTraining_f", 0x37, null);
            BGMStages[0x38] = new BGMStage("SimpleIntroScene", 0x38, null);
            BGMStages[0x39] = new BGMStage("SimpleResultScene_Lose", 0x39, null);
            BGMStages[0x3a] = new BGMStage("SimpleResultScene", 0x3a, null);
            BGMStages[0x3b] = new BGMStage("SimpleResultScene_Final", 0x3b, null);
            BGMStages[0x3c] = new BGMStage("SimpleResultScene_Failure", 0x3c, null);
            BGMStages[0x3d] = new BGMStage("SimpleRootmapScene", 0x3d, null);
            BGMStages[0x3e] = new BGMStage("Playable_roll_f", 0x3e, null);
            BGMStages[0x3f] = new BGMStage("Bomb_f", 0x3f, null);
            BGMStages[0x40] = new BGMStage("BombResult_f", 0x40, null);
            BGMStages[0x41] = new BGMStage("Tournament_Result", 0x41, null);
            BGMStages[0x42] = new BGMStage("Tournament_Table", 0x42, null);
            BGMStages[0x43] = new BGMStage("Tournament_Table_Watching", 0x43, null);
            BGMStages[0x44] = new BGMStage("FigShopScene", 0x44, null);
            BGMStages[0x45] = new BGMStage("Rush_f", 0x45, null);
            BGMStages[0x46] = new BGMStage("EndingFigGetScene", 0x46, null);
            BGMStages[0x47] = new BGMStage("FigListScene", 0x47, null);
            BGMStages[0x48] = new BGMStage("Homerun_f", 0x48, null);
            BGMStages[0x49] = new BGMStage("Menu", 0x49, 0xc8);
            BGMStages[0x4a] = new BGMStage("AlbumScene", 0x4a, null);
            BGMStages[0x4b] = new BGMStage("World_Boss_YellowDevil", 0x4b, null);
            BGMStages[0x4c] = new BGMStage("World_Boss_Face", 0x4c, null);
            BGMStages[0x4d] = new BGMStage("World_Boss_Ridley", 0x4d, null);
            BGMStages[0x4e] = new BGMStage("World_Map", 0x4e, null);
            BGMStages[0x4f] = new BGMStage("World_Map_5Turns", 0x4f, null);
            BGMStages[0x50] = new BGMStage("World_Map_Battle", 0x50, null);
            BGMStages[0x51] = new BGMStage("Battlefieldk_f", 0x51, null);
            BGMStages[0x52] = new BGMStage("VSResult", 0x52, null);
            BGMStages[0x53] = new BGMStage("Tournament_Entry", 0x53, null);
            BGMStages[0x54] = new BGMStage("Tournament_FighterSelect", 0x54, null);
            BGMStages[0x55] = new BGMStage("Miiverse", 0x55, 0x2e);
            BGMStages[0x56] = new BGMStage("StreetFighter_f", 0x56, 0x31);
            BGMStages[0x57] = new BGMStage("Pupupuland64_f", 0x57, 0x30);
            BGMStages[0x58] = new BGMStage("PeachCastle64_f", 0x58, 0x32);
            BGMStages[0x59] = new BGMStage("Hyrule64_f", 0x59, 0x33);
            BGMStages[0x5a] = new BGMStage("XPirates_f", 0x5a, 0x35);
            BGMStages[0x5b] = new BGMStage("MarioMaker_f", 0x5b, 0x34);
            BGMStages[0x5c] = new BGMStage("Midgar_f", 0x5c, 0x36);
            BGMStages[0x5d] = new BGMStage("Umbra_f", 0x5d, 0x37);

            return BGMStages;
        }
        #endregion
    }
}
