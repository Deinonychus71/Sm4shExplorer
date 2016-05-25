using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shFileExplorer.DB
{
    public class IconsDB
    {
        #region Members
        private static Dictionary<int, string> _IconsDB;
        #endregion

        #region Properties
        public static Dictionary<int, string> Icons { get { return _IconsDB; } }
        #endregion

        #region Main Methods
        internal static void InitializeIconsDB(int gameVersion, bool is3DS)
        {
            if (is3DS)
                InitializeIcons3DSDB(gameVersion);
            else
                InitializeIconsWiiUDB(gameVersion);
        }
        private static void InitializeIconsWiiUDB(int gameVersion)
        {
            if (gameVersion < 208)
                throw new Exception(string.Format("IconsDB: Version {0} for Wii U not supported.", gameVersion));
            _IconsDB = GenerateIconsDatabaseWiiU208();
        }
        private static void InitializeIcons3DSDB(int gameVersion)
        {
            throw new Exception(string.Format("IconsDB: Version {0} for 3DS not supported.", gameVersion));
        }
        #endregion

        #region Wii U
        private static Dictionary<int, string> GenerateIconsDatabaseWiiU208()
        {
            Dictionary<int, string> smashIcons = new Dictionary<int, string>();

            smashIcons[0x0] = "smashbros";
            smashIcons[0x1] = "donkeykong";
            smashIcons[0x2] = "starfox";
            smashIcons[0x3] = "kirby";
            smashIcons[0x4] = "fzero";
            smashIcons[0x5] = "metroid";
            smashIcons[0x6] = "mother";
            smashIcons[0x7] = "pokemon";
            smashIcons[0x8] = "zelda";
            smashIcons[0x9] = "mario";
            smashIcons[0xa] = "yoshi";
            smashIcons[0xb] = "fireemblem";
            smashIcons[0xc] = "gamewatch";
            smashIcons[0xd] = "pikmin";
            smashIcons[0xe] = "wario";
            smashIcons[0xf] = "palutena";
            smashIcons[0x11] = "famicomrobot";
            smashIcons[0x12] = "sonic";
            smashIcons[0x13] = "plankton";
            smashIcons[0x14] = "touch";
            smashIcons[0x15] = "doubutsu";
            smashIcons[0x16] = "wiifit";
            smashIcons[0x17] = "xenoblade";
            smashIcons[0x18] = "punchout";
            smashIcons[0x19] = "duckhunt";
            smashIcons[0x1a] = "rhythm";
            smashIcons[0x1b] = "rockman";
            smashIcons[0x1c] = "nintendogs";
            smashIcons[0x1d] = "miiplaza";
            smashIcons[0x1e] = "pacman";
            smashIcons[0x1f] = "tomodachi";
            smashIcons[0x20] = "wreckingcrew";
            smashIcons[0x21] = "wuhuisland";
            smashIcons[0x22] = "miiverse";
            smashIcons[0x23] = "lightplane";
            smashIcons[0x24] = "braintraining";
            smashIcons[0x25] = "balloonfight";
            smashIcons[0x26] = "diary";
            smashIcons[0x27] = "streetfighter";
            smashIcons[0x28] = "finalfantasy";
            smashIcons[0x29] = "bayonetta";
            smashIcons[0x63] = "etc";
            smashIcons[0x190] = "cs_all";
            smashIcons[0x191] = "cs_equip";
            smashIcons[0x192] = "cs_special";
            smashIcons[0x193] = "cs_miiparts";
            smashIcons[0x194] = "cs_miibody";
            smashIcons[0x195] = "figure";
            smashIcons[0x196] = "money";
            smashIcons[0x197] = "stage";
            smashIcons[0x198] = "sticket";
            smashIcons[0x199] = "cd";
            smashIcons[0x19a] = "soft";
            smashIcons[0x19b] = "mb";
            smashIcons[0x1f4] = "fsi_moveup";
            smashIcons[0x1f5] = "fsi_jumpup";
            smashIcons[0x1f6] = "fsi_atkup";
            smashIcons[0x1f7] = "fsi_specialup";
            smashIcons[0x1f9] = "fsi_itemup";
            smashIcons[0x1fa] = "fsi_defup";
            smashIcons[0x1fb] = "fsi_allup";
            smashIcons[0x1fc] = "fg_fighter";
            smashIcons[0x1fd] = "fg_companion";
            smashIcons[0x1fe] = "fg_finalarts";
            smashIcons[0x1ff] = "fg_item";
            smashIcons[0x200] = "fg_assistfigure";
            smashIcons[0x201] = "fg_pokeball";
            smashIcons[0x202] = "fg_ws";
            smashIcons[0x203] = "fg_fs";
            smashIcons[0x204] = "fg_enemy";
            smashIcons[0x205] = "fg_stage";
            smashIcons[0x206] = "fg_series";
            smashIcons[0x207] = "fg_etc";
            smashIcons[0x208] = "fs_item";
            smashIcons[0x2bd] = "m_retnormal";
            smashIcons[0x2be] = "m_rethold";
            smashIcons[0x2bf] = "m_x0001";
            smashIcons[0x2c0] = "m_reset";
            smashIcons[0x2f3] = "m_stage";
            smashIcons[0x2c2] = "m_fighter";
            smashIcons[0x2c3] = "m_friend";
            smashIcons[0x2c4] = "m_download";
            smashIcons[0x2c5] = "m_loading";
            smashIcons[0x2c6] = "m_nowloading";
            smashIcons[0x2c7] = "m_team";
            smashIcons[0x2c8] = "m_exitmulti";
            smashIcons[0x2ca] = "m_rulenormal";
            smashIcons[0x2cb] = "m_ruletimelimit";
            smashIcons[0x2cc] = "m_customize";
            smashIcons[0x2cd] = "m_rulehandicap";
            smashIcons[0x2ce] = "m_ruleblowratio";
            smashIcons[0x2cf] = "m_rulestaeselect";
            smashIcons[0x2d0] = "m_rulestock";
            smashIcons[0x2d1] = "m_ruleadditonal";
            smashIcons[0x2d2] = "m_ra_stocklimit";
            smashIcons[0x2d3] = "m_ra_teamattack";
            smashIcons[0x2d4] = "m_ra_pausefunction";
            smashIcons[0x2d5] = "m_ra_scoredisplay";
            smashIcons[0x2d6] = "m_ra_damagedisplay";
            smashIcons[0x2d7] = "m_ra_randomstage";
            smashIcons[0x2d8] = "m_miifighter";
            smashIcons[0x2d9] = "m_miishooter";
            smashIcons[0x2da] = "m_miisordsman";
            smashIcons[0x2db] = "m_figlist_rotate";
            smashIcons[0x2dc] = "m_figlist_move";
            smashIcons[0x2dd] = "m_tr_lock";
            smashIcons[0x2de] = "m_tr_camera";
            smashIcons[0x2df] = "m_tr_fixedcamera";
            smashIcons[0x2e0] = "m_tr_zoomin";
            smashIcons[0x2e1] = "m_time";
            smashIcons[0x2e2] = "m_ra_selfmiss";
            smashIcons[0x2e3] = "m_popup  ";
            smashIcons[0x2e4] = "m_title";
            smashIcons[0x2e5] = "m_main";
            smashIcons[0x2e6] = "m_clear";
            smashIcons[0x2e7] = "m_ar";
            smashIcons[0x2e8] = "m_info";
            smashIcons[0x2e9] = "m_wiiu";
            smashIcons[0x2ea] = "m_pass";
            smashIcons[0x2eb] = "m_arcade";
            smashIcons[0x2ec] = "m_pad1";
            smashIcons[0x2ed] = "m_tds";
            smashIcons[0x2c9] = "m_melee";
            smashIcons[0x2ee] = "m_rule";
            smashIcons[0x2ef] = "m_rule_item";
            smashIcons[0x2f0] = "m_rule_add";
            smashIcons[0x2f1] = "m_rule_stage";
            smashIcons[0x2f2] = "m_chara";
            smashIcons[0x2f3] = "m_stage";
            smashIcons[0x2f4] = "m_result";
            smashIcons[0x2f5] = "m_chara_8";
            smashIcons[0x2f6] = "m_result_8";
            smashIcons[0x2f7] = "m_melee_multi";
            smashIcons[0x2f8] = "m_melee_newg";
            smashIcons[0x2f9] = "m_wifi_friend";
            smashIcons[0x2fa] = "m_friend_list";
            smashIcons[0x2fb] = "m_wifi_other";
            smashIcons[0x2fc] = "m_wifi_enjoy";
            smashIcons[0x2fd] = "m_wifi_serious";
            smashIcons[0x2fe] = "m_wifi_record";
            smashIcons[0x2ff] = "m_wifi_watch";
            smashIcons[0x300] = "m_watch_list";
            smashIcons[0x301] = "m_wifi_share";
            smashIcons[0x302] = "m_share_replay";
            smashIcons[0x303] = "m_share_list";
            smashIcons[0x304] = "m_share_photo";
            smashIcons[0x305] = "m_share_mii";
            smashIcons[0x306] = "m_share_stage";
            smashIcons[0x307] = "m_wifi_event";
            smashIcons[0x308] = "m_event_compe";
            smashIcons[0x309] = "m_event_conq";
            smashIcons[0x30a] = "m_wifi_tour";
            smashIcons[0x30b] = "m_field";
            smashIcons[0x30c] = "m_result_field1";
            smashIcons[0x30d] = "m_other";
            smashIcons[0x30e] = "m_other_one";
            smashIcons[0x30f] = "m_diffi";
            smashIcons[0x313] = "m_order";
            smashIcons[0x313] = "m_order";
            smashIcons[0x313] = "m_order";
            smashIcons[0x313] = "m_order";
            smashIcons[0x314] = "m_order_stage_m";
            smashIcons[0x315] = "m_order_stock";
            smashIcons[0x316] = "m_order_stage_c";
            smashIcons[0x317] = "m_order_how";
            smashIcons[0x318] = "m_world";
            smashIcons[0x319] = "m_event";
            smashIcons[0x31a] = "m_other_all";
            smashIcons[0x31b] = "m_stadium";
            smashIcons[0x31c] = "m_stadium_spar";
            smashIcons[0x31d] = "m_other_chrmake";
            smashIcons[0x31e] = "m_chrmake_cs";
            smashIcons[0x31f] = "m_chrmake_mii";
            smashIcons[0x320] = "m_other_stgmake";
            smashIcons[0x321] = "m_other_col";
            smashIcons[0x322] = "m_col_fig";
            smashIcons[0x323] = "m_flg_list";
            smashIcons[0x324] = "m_fig_disp";
            smashIcons[0x325] = "m_fig_coin";
            smashIcons[0x326] = "m_fig_shop";
            smashIcons[0x327] = "m_fig_box";
            smashIcons[0x328] = "m_col_album";
            smashIcons[0x329] = "m_col_replay";
            smashIcons[0x32a] = "m_col_sound";
            smashIcons[0x32b] = "m_col_record";
            smashIcons[0x32c] = "m_col_record_vs";
            smashIcons[0x32d] = "m_record_count";
            smashIcons[0x32e] = "m_record_info";
            smashIcons[0x32f] = "m_record_bonus";
            smashIcons[0x330] = "m_other_option";
            smashIcons[0x331] = "m_option_vibrate";
            smashIcons[0x332] = "m_option_button";
            smashIcons[0x333] = "m_option_sound";
            smashIcons[0x334] = "m_option_border";
            smashIcons[0x335] = "m_option_pos";
            smashIcons[0x336] = "m_option_wifi";
            smashIcons[0x337] = "m_option_profile";
            smashIcons[0x338] = "m_option_camera";
            smashIcons[0x339] = "m_option_mymusic";
            smashIcons[0x33a] = "m_from_offical";
            smashIcons[0x33b] = "m_from_friend";
            smashIcons[0x33c] = "m_from_delivery";
            smashIcons[0x3e8] = "mariocart";

            return smashIcons;
        }
        #endregion
    }
}
