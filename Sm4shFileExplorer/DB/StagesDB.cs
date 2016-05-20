using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shFileExplorer.DB
{
    public class StagesDB
    {
        #region Members
        private static List<Stage> _StagesDB;
        #endregion

        #region Properties
        public static List<Stage> Stages { get { return _StagesDB; } }
        #endregion

        #region Main Methods
        internal static void InitializeStageDB(int gameVersion, bool is3DS)
        {
            if (is3DS)
                InitializeStages3DSDB(gameVersion);
            else
                InitializeStagesWiiUDB(gameVersion);
        }
        private static void InitializeStagesWiiUDB(int gameVersion)
        {
            if (gameVersion < 208)
                throw new Exception(string.Format("StageDB: Version {0} for Wii U not supported.", gameVersion));
            _StagesDB = GenerateStagesDatabaseWiiU208();
        }
        private static void InitializeStages3DSDB(int gameVersion)
        {
            throw new Exception(string.Format("StageDB: Version {0} for 3DS not supported.", gameVersion));
        }
        #endregion

        #region Wii U
        private static List<Stage> GenerateStagesDatabaseWiiU208()
        {
            List<Stage> stages = new List<Stage>();

            stages.Add(new Stage(0x0, "BattleField_f", StageType.Melee));
            stages.Add(new Stage(0x1, "End_f", StageType.Melee));
            stages.Add(new Stage(0x2, "Galaxy", StageType.Melee));
            stages.Add(new Stage(0x3, "MarioU", StageType.Melee));
            stages.Add(new Stage(0x4, "MarioKart", StageType.Melee));
            stages.Add(new Stage(0x5, "Kalos", StageType.Melee));
            stages.Add(new Stage(0x6, "Skyward", StageType.Melee));
            stages.Add(new Stage(0x7, "DKReturns", StageType.Melee));
            stages.Add(new Stage(0x8, "Metroid", StageType.Melee));
            stages.Add(new Stage(0x9, "YoshiU", StageType.Melee));
            stages.Add(new Stage(0xa, "Cave", StageType.Melee));
            stages.Add(new Stage(0xb, "Village2", StageType.Melee));
            stages.Add(new Stage(0xc, "Assault", StageType.Melee));
            stages.Add(new Stage(0xd, "Colloseum_f", StageType.Melee));
            stages.Add(new Stage(0xe, "PikminU", StageType.Melee));
            stages.Add(new Stage(0xf, "Angeland", StageType.Melee));
            stages.Add(new Stage(0x10, "Gamer", StageType.Melee));
            stages.Add(new Stage(0x11, "DuckHunt", StageType.Melee));
            stages.Add(new Stage(0x12, "Wrecking", StageType.Melee));
            stages.Add(new Stage(0x13, "XenoBlade", StageType.Melee));
            stages.Add(new Stage(0x14, "PunchOut", StageType.Melee));
            stages.Add(new Stage(0x15, "PunchOut2", StageType.Melee));
            stages.Add(new Stage(0x16, "Wufu", StageType.Melee));
            stages.Add(new Stage(0x17, "Miiverse", StageType.Melee));
            stages.Add(new Stage(0x18, "WiiFit", StageType.Melee));
            stages.Add(new Stage(0x19, "Pilotwings", StageType.Melee));
            stages.Add(new Stage(0x1a, "Windyhill", StageType.Melee));
            stages.Add(new Stage(0x1b, "Pacland", StageType.Melee));
            stages.Add(new Stage(0x1c, "Wily", StageType.Melee));
            stages.Add(new Stage(0x1d, "Gw3", StageType.Melee));
            stages.Add(new Stage(0x1e, "XDolpic", StageType.Melee));
            stages.Add(new Stage(0x1f, "XKart", StageType.Melee));
            stages.Add(new Stage(0x20, "XStadium", StageType.Melee));
            stages.Add(new Stage(0x21, "XOldin", StageType.Melee));
            stages.Add(new Stage(0x22, "64Jungle", StageType.Melee));
            stages.Add(new Stage(0x23, "XNorfair", StageType.Melee));
            stages.Add(new Stage(0x24, "DxYorster", StageType.Melee));
            stages.Add(new Stage(0x25, "XHalberd", StageType.Melee));
            stages.Add(new Stage(0x26, "XVillage", StageType.Melee));
            stages.Add(new Stage(0x27, "XStarfox", StageType.Melee));
            stages.Add(new Stage(0x28, "XEmblem", StageType.Melee));
            stages.Add(new Stage(0x29, "XPalutena", StageType.Melee));
            stages.Add(new Stage(0x2a, "XFzero", StageType.Melee));
            stages.Add(new Stage(0x2b, "DxOnett", StageType.Melee));
            stages.Add(new Stage(0x2c, "XDonkey", StageType.Melee));
            stages.Add(new Stage(0x2d, "DxShrine", StageType.Melee));
            stages.Add(new Stage(0x2e, "XMansion", StageType.Melee));
            stages.Add(new Stage(0x2f, "BattleField_f", StageType.End));
            stages.Add(new Stage(0x30, "Galaxy", StageType.End));
            stages.Add(new Stage(0x31, "MarioU", StageType.End));
            stages.Add(new Stage(0x32, "MarioKart", StageType.End));
            stages.Add(new Stage(0x33, "Kalos", StageType.End));
            stages.Add(new Stage(0x34, "Skyward", StageType.End));
            stages.Add(new Stage(0x35, "DKReturns", StageType.End));
            stages.Add(new Stage(0x36, "Metroid", StageType.End));
            stages.Add(new Stage(0x37, "YoshiU", StageType.End));
            stages.Add(new Stage(0x38, "Cave", StageType.End));
            stages.Add(new Stage(0x39, "Village2", StageType.End));
            stages.Add(new Stage(0x3a, "Assault", StageType.End));
            stages.Add(new Stage(0x3b, "Colloseum_f", StageType.End));
            stages.Add(new Stage(0x3c, "PikminU", StageType.End));
            stages.Add(new Stage(0x3d, "Angeland", StageType.End));
            stages.Add(new Stage(0x3e, "Gamer", StageType.End));
            stages.Add(new Stage(0x3f, "DuckHunt", StageType.End));
            stages.Add(new Stage(0x40, "Wrecking", StageType.End));
            stages.Add(new Stage(0x41, "XenoBlade", StageType.End));
            stages.Add(new Stage(0x42, "PunchOut", StageType.End));
            stages.Add(new Stage(0x43, "PunchOut2", StageType.End));
            stages.Add(new Stage(0x44, "Wufu", StageType.End));
            stages.Add(new Stage(0x45, "Miiverse", StageType.End));
            stages.Add(new Stage(0x46, "WiiFit", StageType.End));
            stages.Add(new Stage(0x47, "Pilotwings", StageType.End));
            stages.Add(new Stage(0x48, "Windyhill", StageType.End));
            stages.Add(new Stage(0x49, "Pacland", StageType.End));
            stages.Add(new Stage(0x4a, "Wily", StageType.End));
            stages.Add(new Stage(0x4b, "Gw3", StageType.End));
            stages.Add(new Stage(0x4c, "XDolpic", StageType.End));
            stages.Add(new Stage(0x4d, "XKart", StageType.End));
            stages.Add(new Stage(0x4e, "XStadium", StageType.End));
            stages.Add(new Stage(0x4f, "XOldin", StageType.End));
            stages.Add(new Stage(0x50, "64Jungle", StageType.End));
            stages.Add(new Stage(0x51, "XNorfair", StageType.End));
            stages.Add(new Stage(0x52, "DxYorster", StageType.End));
            stages.Add(new Stage(0x53, "XHalberd", StageType.End));
            stages.Add(new Stage(0x54, "XVillage", StageType.End));
            stages.Add(new Stage(0x55, "XStarfox", StageType.End));
            stages.Add(new Stage(0x56, "XEmblem", StageType.End));
            stages.Add(new Stage(0x57, "XPalutena", StageType.End));
            stages.Add(new Stage(0x58, "XFzero", StageType.End));
            stages.Add(new Stage(0x59, "DxOnett", StageType.End));
            stages.Add(new Stage(0x5a, "XDonkey", StageType.End));
            stages.Add(new Stage(0x5b, "DxShrine", StageType.End));
            stages.Add(new Stage(0x5c, "XMansion", StageType.End));
            stages.Add(new Stage(0x5d, "Battlefieldk_f", StageType.Other));
            stages.Add(new Stage(0x5e, "Allstar_f", StageType.Other));
            stages.Add(new Stage(0x5f, "Homerun_f", StageType.Other));
            stages.Add(new Stage(0x60, "Bomb_f", StageType.Other));
            stages.Add(new Stage(0x61, "Rush_f", StageType.Other));
            stages.Add(new Stage(0x62, "OnlineTraining_f", StageType.Other));
            stages.Add(new Stage(0x63, "PrePlay_f", StageType.Other));
            stages.Add(new Stage(0x64, "StageEdit", StageType.Other));
            stages.Add(new Stage(0x65, "Playable_roll_f", StageType.Other));
            stages.Add(new Stage(0x66, "fig_get", StageType.Other));
            stages.Add(new Stage(0x67, "fig_disp_f", StageType.Fig));
            stages.Add(new Stage(0x68, "fig_photo1", StageType.Fig));
            stages.Add(new Stage(0x69, "fig_photo2", StageType.Fig));
            stages.Add(new Stage(0x6a, "fig_photo3", StageType.Fig));
            stages.Add(new Stage(0x6b, "fig_photo4", StageType.Fig));
            stages.Add(new Stage(0x6c, "BattleFieldL_f", StageType.Melee));
            stages.Add(new Stage(0x6d, "RushL_f", StageType.Other));
            stages.Add(new Stage(0x6e, "StreetFighter_f", StageType.Melee));
            stages.Add(new Stage(0x6f, "StreetFighter_f", StageType.End));
            stages.Add(new Stage(0x70, "Pupupuland64_f", StageType.Melee));
            stages.Add(new Stage(0x71, "Pupupuland64_f", StageType.End));
            stages.Add(new Stage(0x72, "PeachCastle64_f", StageType.Melee));
            stages.Add(new Stage(0x73, "PeachCastle64_f", StageType.End));
            stages.Add(new Stage(0x74, "Hyrule64_f", StageType.Melee));
            stages.Add(new Stage(0x75, "Hyrule64_f", StageType.End));
            stages.Add(new Stage(0x76, "MarioMaker_f", StageType.Melee));
            stages.Add(new Stage(0x77, "MarioMaker_f", StageType.End));
            stages.Add(new Stage(0x78, "XPirates_f", StageType.Melee));
            stages.Add(new Stage(0x79, "XPirates_f", StageType.End));
            stages.Add(new Stage(0x7a, "", StageType.Melee));
            stages.Add(new Stage(0x7b, "", StageType.End));
            stages.Add(new Stage(0x7c, "Midgar_f", StageType.Melee));
            stages.Add(new Stage(0x7d, "Midgar_f", StageType.End));
            stages.Add(new Stage(0x7e, "Umbra_f", StageType.Melee));
            stages.Add(new Stage(0x7f, "Umbra_f", StageType.End));

            return stages;
        }
        #endregion
    }

    public enum StageType
    {
        Melee = 0,
        End = 1,
        Other = 3,
        Fig = 4
    }

    public class Stage
    {
        private int _StageID;
        private StageType _StageType;
        private string _StageLabel;

        public int ID { get { return _StageID; } }
        public StageType Type { get { return _StageType; } }
        public string Label { get { return _StageLabel; } }

        public Stage(int id, string label, StageType type)
        {
            _StageID = id;
            _StageLabel = label;
            _StageType = type;
        }
    }
}
