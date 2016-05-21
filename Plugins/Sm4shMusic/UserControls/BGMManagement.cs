using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sm4shMusic.Objects;
using Sm4shMusic.Globals;
using Sm4shMusic.Forms;
using Sm4shFileExplorer.DB;
using Sm4shFileExplorer;
using Sm4shFileExplorer.Objects;
using Sm4shFileExplorer.Globals;

namespace Sm4shMusic.UserControls
{
    public partial class BGMManagement : UserControl
    {
        private EnumEntity[] _EnumSoundSource;
        private EnumEntity[] _EnumSoundMixType;
        private EnumEntity[] _EnumBackgroundBehavior;
        private EnumEntity[] _EnumIcons;
        private SoundEntry _CurrentSoundEntry;
        private SoundEntry _CurrentSoundEntryOriginal;
        private bool _LoadingSound = false;

        private BGMProperties _BGMProperties;
        private SortMusic _SortMusic;
        private CharacterRotation _CharacterRotation;

        public SoundEntryCollection SoundEntryCollection { get; set; }
        public SoundEntryCollection SoundEntryCollectionBackup { get; set; }

        #region Initialization
        public BGMManagement()
        {
            InitializeComponent();
            _BGMProperties = new BGMProperties();
            _SortMusic = new SortMusic();
            _CharacterRotation = new CharacterRotation();
        }

        public void Initialize(Sm4shProject sm4shProject)
        {
            RefreshBGMTextArea();

            _EnumSoundSource = (EnumEntity[])((IEnumerable<SoundSource>)Enum.GetValues(typeof(SoundSource))).Select(c => new EnumEntity() { Value = (uint)c, Name = c.ToString() }).ToArray();
            ddlSoundSource.Items.Clear();
            ddlSoundSource.Items.AddRange(_EnumSoundSource);

            _EnumSoundMixType = (EnumEntity[])((IEnumerable<SoundMixType>)Enum.GetValues(typeof(SoundSource))).Select(c => new EnumEntity() { Value = (uint)c, Name = c.ToString() }).ToArray();
            ddlSoundMixType.Items.Clear();
            ddlSoundMixType.Items.AddRange(_EnumSoundMixType);

            _EnumBackgroundBehavior = (EnumEntity[])((IEnumerable<SoundTestBackImageBehavior>)Enum.GetValues(typeof(SoundTestBackImageBehavior))).Select(c => new EnumEntity() { Value = (uint)c, Name = c.ToString() }).ToArray();
            ddlBackRotationBehavior.Items.Clear();
            ddlBackRotationBehavior.Items.AddRange(_EnumBackgroundBehavior);

            _EnumIcons = (EnumEntity[])IconsDB.Icons.Select(c => new EnumEntity() { Value = (uint)c.Key, Name = c.Value }).ToArray();
            ddlGroupStageCreation.Items.Clear();
            ddlGroupStageCreation.Items.Add(new EnumEntity() { Name = "NULL", Value = 0xffffffff });
            ddlGroupStageCreation.Items.AddRange(_EnumIcons);
            ddlSoundIconID.Items.Clear();
            ddlSoundIconID.Items.Add(new EnumEntity() { Name = "NULL", Value = 0xffffffff });
            ddlSoundIconID.Items.AddRange(_EnumIcons);

            listEntries.DataSource = SoundEntryCollection.SoundEntries;
        }
        #endregion

        #region Methods
        private AutoCompleteStringCollection GetAllBGMFiles()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            col.AddRange(Sm4shSoundTools.SoundFiles.ToArray());
            return col;
        }

        public void RefreshBGMTextArea()
        {
            AutoCompleteStringCollection col = GetAllBGMFiles();
            txtBGM1.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBGM1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBGM1.AutoCompleteCustomSource = col;
            txtBGM2.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBGM2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBGM2.AutoCompleteCustomSource = col;
            txtBGM3.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBGM3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBGM3.AutoCompleteCustomSource = col;
            txtBGM4.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBGM4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBGM4.AutoCompleteCustomSource = col;
            txtBGM5.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBGM5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBGM5.AutoCompleteCustomSource = col;
        }

        private void PopulateInfo(SoundEntry sEntry)
        {
            _LoadingSound = true;

            VGMStreamPlayer.StopCurrentVGMStreamPlayback();
            ClearBindings(this);

            _CurrentSoundEntry = sEntry;

            if (sEntry.BGMFiles[0].BGMEntry.BGMTitle == "TEST05")
                SanitizeBGMTest05();

            if (SoundEntryCollectionBackup.SoundEntriesPerID.ContainsKey(sEntry.SoundID))
            {
                _CurrentSoundEntryOriginal = SoundEntryCollectionBackup.SoundEntriesPerID[sEntry.SoundID];
                btnRestoreDBFile.Visible = true;
                btnRestoreMSBT.Visible = true;
            }
            else
            {
                _CurrentSoundEntryOriginal = null;
                btnRestoreDBFile.Visible = false;
                btnRestoreMSBT.Visible = false;
            }

            for (int i = 1; i <= 5; i++)
            {
                TextBox txtBGM = (TextBox)(this.Controls.Find("txtBGM" + i, true)[0]);
                if (sEntry.BGMFiles.Count >= i)
                    txtBGM.Text = sEntry.BGMFiles[i - 1].BGMEntry.BGMTitle;
                else
                    txtBGM.Text = string.Empty;
            }

            ddlSoundSource.SelectedItem = _EnumSoundSource.FirstOrDefault(p => p.Value == (int)sEntry.SoundSource);
            ddlSoundMixType.SelectedItem = _EnumSoundMixType.FirstOrDefault(p => p.Value == (int)sEntry.SoundMixType);
            ddlBackRotationBehavior.SelectedItem = _EnumBackgroundBehavior.FirstOrDefault(p => p.Value == (int)sEntry.SoundTestBackImageBehavior);
            ddlGroupStageCreation.SelectedItem = _EnumIcons.FirstOrDefault(p => p.Value == (int)sEntry.StageCreationGroupID);
            if (ddlGroupStageCreation.SelectedItem == null)
                ddlGroupStageCreation.SelectedIndex = 0;
            ddlSoundIconID.SelectedItem = _EnumIcons.FirstOrDefault(p => p.Value == (int)sEntry.IconID);
            if (ddlSoundIconID.SelectedItem == null)
                ddlSoundIconID.SelectedIndex = 0;

            listCharacterBackgroundRotation.Items.Clear();
            foreach (string chara in sEntry.AssociatedFightersName)
                listCharacterBackgroundRotation.Items.Add(chara);

            txtOrderSoundTest.Text = sEntry.SoundTestOrder.ToString();
            txtOrderStageCreation.Text = sEntry.StageCreationOrder.ToString();

            //Properties supporting binding
            txtTitle.DataBindings.Add("Text", sEntry, "Title");
            txtTitleSoundTest.DataBindings.Add("Text", sEntry, "SoundTestTitle");
            txtDescription.DataBindings.Add("Text", sEntry, "Description");
            txtDescription2.DataBindings.Add("Text", sEntry, "Description2");
            txtSource.DataBindings.Add("Text", sEntry, "Source");

            txtShort17.DataBindings.Add("Text", sEntry, "Int17");

            chkInSoundTest.DataBindings.Add("Checked", sEntry, "InSoundTest");
            chkUnknownByte2.DataBindings.Add("Checked", sEntry, "Byte2");
            chkUnknownByte3.DataBindings.Add("Checked", sEntry, "Byte3");
            chkUnknownByte4.DataBindings.Add("Checked", sEntry, "Byte4");
            chkRegionJPN.DataBindings.Add("Checked", sEntry, "InRegionJPN");
            chkRegionUSEUR.DataBindings.Add("Checked", sEntry, "InRegionEUUS");

            _LoadingSound = false;
        }

        private void SanitizeBGMTest05()
        {
            if(MessageBox.Show(Strings.TEST05_SANITIZE, Strings.CAPTION_INFO, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _CurrentSoundEntry.BGMFiles = new List<SoundEntryBGM>();
                _CurrentSoundEntry.BGMFiles.Add(new SoundEntryBGM(SoundEntryCollection, _CurrentSoundEntry, SoundEntryCollection.SoundEntriesBGMs.First().BGMID));
                _CurrentSoundEntry.InSoundTest = true;
                _CurrentSoundEntry.Byte2 = true;
                _CurrentSoundEntry.Byte3 = true;
                _CurrentSoundEntry.Byte4 = false;
                _CurrentSoundEntry.InRegionEUUS = true;
                _CurrentSoundEntry.InRegionJPN = true;
                _CurrentSoundEntry.SoundSource = SoundSource.CoreGameSound;
                _CurrentSoundEntry.SoundMixType = SoundMixType.Original;
                _CurrentSoundEntry.IconID = 0x0; //smashbros
                _CurrentSoundEntry.AssociatedFightersIDs = new List<uint>();
                _CurrentSoundEntry.SoundTestBackImageBehavior = SoundTestBackImageBehavior.RosterRandom;
                _CurrentSoundEntry.StageCreationGroupID = 0x0; //smashbros
                _CurrentSoundEntry.Int17 = 0x0;
            }
        }

        private void ClearBindings(Control parentControl)
        {
            if (parentControl != null)
            {
                foreach (Control control in parentControl.Controls)
                {
                    control.DataBindings.Clear();
                    ClearBindings(control);
                }
            }
        }

        public void Clean()
        {
            playerBGM1.File = null;
            playerBGM2.File = null;
            playerBGM3.File = null;
            playerBGM4.File = null;
            playerBGM5.File = null;
        }
        #endregion

        #region Event Handlers
        private void listEntries_ItemSelected(object sender, EventHandlers.ListEntryArgs e)
        {
            SoundEntry sEntry = e.ListEntry as SoundEntry;
            if (sEntry != null)
                PopulateInfo(sEntry);
        }

        private void listEntries_ItemAdded(object sender, EventArgs e)
        {
            SoundEntryCollection.CreateSoundEntry();
            listEntries.RefreshList();
            listEntries.SelectLastItem();
        }

        private void listEntries_ItemRemoved(object sender, EventHandlers.ListEntryArgs e)
        {
            SoundEntry sEntry = e.ListEntry as SoundEntry;
            if (sEntry != null)
            {
                ClearBindings(this);
                _CurrentSoundEntry = null;
                SoundEntryCollection.RemoveSoundEntry(sEntry.SoundID);
                listEntries.RefreshList();
            }
        }

        private void txtBGM_TextChanged(object sender, EventArgs e)
        {
            TextBox textAreaBGM = (TextBox)sender;
            string nameBGM = textAreaBGM.Text;
            int index = Convert.ToInt32(textAreaBGM.Name.Replace("txtBGM", string.Empty));
            string file = "snd_bgm_" + nameBGM + ".nus3bank";
            VGMStreamPlayer vgmStreamPlayer = ((VGMStreamPlayer)(this.Controls.Find("playerBGM" + index, true)[0]));
            if (VGMStreamPlayer.CurrentVGMStreamPlayer == vgmStreamPlayer)
                VGMStreamPlayer.StopCurrentVGMStreamPlayback();
            Button buttonBGMAdvanced = (Button)(this.Controls.Find("btnBGMAdv" + index, true)[0]);
            file = SoundEntryCollection.GetBGMFullPath(file);
            if (!string.IsNullOrEmpty(file) && SoundEntryCollection.SoundEntriesBGMsPerName.ContainsKey(nameBGM))
            {
                vgmStreamPlayer.File = file;
                textAreaBGM.Tag = SoundEntryCollection.SoundEntriesBGMsPerName[nameBGM];
                if (textAreaBGM.Tag == null)
                    throw new Exception("Problem finding SoundEntryBGM");
                buttonBGMAdvanced.Enabled = true;
            }
            else
            {
                vgmStreamPlayer.File = null;
                vgmStreamPlayer.Enabled = false;
                buttonBGMAdvanced.Enabled = false;
                textAreaBGM.Tag = null;
            }

            if (_LoadingSound)
                return;

            //Update BGMFiles
            SoundEntryBGM mainBGM = _CurrentSoundEntry.BGMFiles[0];
            _CurrentSoundEntry.BGMFiles.Clear();
            for (int i = 1; i <= 5; i++)
            {
                TextBox txtBGM = (TextBox)(this.Controls.Find("txtBGM" + i, true)[0]);
                if (txtBGM.Tag != null)
                    _CurrentSoundEntry.BGMFiles.Add(new SoundEntryBGM(SoundEntryCollection, _CurrentSoundEntry, ((BGMEntry)txtBGM.Tag).BGMID));
                else if (i == 1)
                    _CurrentSoundEntry.BGMFiles.Add(mainBGM);
            }
        }

        private void btmBGMAdv_Click(object sender, EventArgs e)
        {
            Button buttonBGMAdvanced = (Button)sender;
            string index = buttonBGMAdvanced.Name.Replace("btnBGMAdv", string.Empty);
            TextBox txtBGM = (TextBox)(this.Controls.Find("txtBGM" + index, true)[0]);
            BGMEntry sEntryBGM = (BGMEntry)(txtBGM.Tag);
            _BGMProperties.CurrentSoundEntryBGM = sEntryBGM;
            if (SoundEntryCollectionBackup.SoundEntriesBGMsPerID.ContainsKey(sEntryBGM.BGMID))
                _BGMProperties.CurrentSoundEntryBGMOriginal = SoundEntryCollectionBackup.SoundEntriesBGMsPerID[sEntryBGM.BGMID];
            else
                _BGMProperties.CurrentSoundEntryBGMOriginal = null;
            _BGMProperties.FormTitle = string.Format("BGM properties for {0}", sEntryBGM.BGMTitle);
            _BGMProperties.ShowDialog(this);
        }

        private void btnRestoreMSBT_Click(object sender, EventArgs e)
        {
            if (_CurrentSoundEntryOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _CurrentSoundEntry.Title = _CurrentSoundEntryOriginal.Title;
                _CurrentSoundEntry.Description = _CurrentSoundEntryOriginal.Description;
                _CurrentSoundEntry.Description2 = _CurrentSoundEntryOriginal.Description2;
                _CurrentSoundEntry.SoundTestTitle = _CurrentSoundEntryOriginal.SoundTestTitle;
                _CurrentSoundEntry.Source = _CurrentSoundEntryOriginal.Source;
            }
        }

        private void btnRestoreDBFile_Click(object sender, EventArgs e)
        {
            if (_CurrentSoundEntryOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _CurrentSoundEntry.AssociatedFightersIDs = _CurrentSoundEntryOriginal.AssociatedFightersIDs;
                _CurrentSoundEntry.BGMFiles.Clear();
                foreach (SoundEntryBGM sEntryBGM in _CurrentSoundEntryOriginal.BGMFiles)
                    _CurrentSoundEntry.BGMFiles.Add(new SoundEntryBGM(SoundEntryCollection, _CurrentSoundEntry, sEntryBGM.BGMID));
                _CurrentSoundEntry.Byte2 = _CurrentSoundEntryOriginal.Byte2;
                _CurrentSoundEntry.Byte3 = _CurrentSoundEntryOriginal.Byte3;
                _CurrentSoundEntry.Byte4 = _CurrentSoundEntryOriginal.Byte4;
                _CurrentSoundEntry.IconID = _CurrentSoundEntryOriginal.IconID;
                _CurrentSoundEntry.InSoundTest = _CurrentSoundEntryOriginal.InSoundTest;
                _CurrentSoundEntry.InRegionEUUS = _CurrentSoundEntryOriginal.InRegionEUUS;
                _CurrentSoundEntry.InRegionJPN = _CurrentSoundEntryOriginal.InRegionJPN;
                _CurrentSoundEntry.Int17 = _CurrentSoundEntryOriginal.Int17;
                _CurrentSoundEntry.SoundMixType = _CurrentSoundEntryOriginal.SoundMixType;
                _CurrentSoundEntry.SoundSource = _CurrentSoundEntryOriginal.SoundSource;
                _CurrentSoundEntry.SoundTestBackImageBehavior = _CurrentSoundEntryOriginal.SoundTestBackImageBehavior;
                _CurrentSoundEntry.StageCreationGroupID = _CurrentSoundEntryOriginal.StageCreationGroupID;

                PopulateInfo(_CurrentSoundEntry);
            }
        }

        private void btnOrderSoundTest_Click(object sender, EventArgs e)
        {
            _SortMusic.LoadSoundTestOrder(SoundEntryCollection.SoundEntries, SoundEntryCollectionBackup.SoundEntries);
            _SortMusic.FormTitle = "Sound Test Order";
            _SortMusic.ShowDialog(this);
            txtOrderSoundTest.Text = _CurrentSoundEntry.SoundTestOrder.ToString();
        }

        private void btnOrderStageCreation_Click(object sender, EventArgs e)
        {
            _SortMusic.LoadStageCreationOrder(SoundEntryCollection.SoundEntries, SoundEntryCollectionBackup.SoundEntries);
            _SortMusic.FormTitle = "Stage Creation Order";
            _SortMusic.ShowDialog(this);
            txtOrderStageCreation.Text = _CurrentSoundEntry.StageCreationOrder.ToString();
        }

        private void btnChooseChararactersBackRotation_Click(object sender, EventArgs e)
        {
            _CharacterRotation.CurrentSoundEntry = _CurrentSoundEntry;
            _CharacterRotation.FormTitle = string.Format("BGM properties for {0}", _CurrentSoundEntry.Title);
            _CharacterRotation.ShowDialog(this);

            listCharacterBackgroundRotation.Items.Clear();
            foreach (string chara in _CurrentSoundEntry.AssociatedFightersName)
                listCharacterBackgroundRotation.Items.Add(chara);
        }

        private void ddlSoundSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumEntity entity = (EnumEntity)ddlSoundSource.SelectedItem;
            _CurrentSoundEntry.SoundSource = (SoundSource)entity.Value;
        }

        private void ddlSoundMixType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumEntity entity = (EnumEntity)ddlSoundMixType.SelectedItem;
            _CurrentSoundEntry.SoundMixType = (SoundMixType)entity.Value;
        }

        private void ddlSoundIconID_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumEntity entity = (EnumEntity)ddlSoundIconID.SelectedItem;
            if (entity != null)
                _CurrentSoundEntry.IconID = (uint)entity.Value;
            else
                _CurrentSoundEntry.IconID = 4294967295;
        }

        private void ddlBackRotationBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumEntity entity = (EnumEntity)ddlBackRotationBehavior.SelectedItem;
            _CurrentSoundEntry.SoundTestBackImageBehavior = (SoundTestBackImageBehavior)entity.Value;
        }

        private void ddlGroupStageCreation_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumEntity entity = (EnumEntity)ddlGroupStageCreation.SelectedItem;
            if (entity != null)
                _CurrentSoundEntry.StageCreationGroupID = (uint)entity.Value;
            else
                _CurrentSoundEntry.StageCreationGroupID = 4294967295;
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if(_CurrentSoundEntry != null)
                listEntries.RefreshSelectedItem();
        }
        #endregion

        #region Help
        private void help_Click(object sender, EventArgs e)
        {
            PictureBox helpBtn = sender as PictureBox;
            if (helpBtn == null)
                return;

            switch(helpBtn.Name)
            {
                case "helpTitle":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_TITLE, Strings.CAPTION_HELP);
                    break;
                case "helpTitleSoundTest":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_TITLE_SOUNDTEST, Strings.CAPTION_HELP);
                    break;
                case "helpDescription":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_DESCRIPTION, Strings.CAPTION_HELP);
                    break;
                case "helpDescription2":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_DESCRIPTION2, Strings.CAPTION_HELP);
                    break;
                case "helpSource":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_SOURCE, Strings.CAPTION_HELP);
                    break;
                case "helpBGM1":
                case "helpBGM2":
                case "helpBGM3":
                case "helpBGM4":
                case "helpBGM5":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_BGM, Strings.CAPTION_HELP);
                    break;
                case "helpSoundSource":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_SOUND_SOURCE, Strings.CAPTION_HELP);
                    break;
                case "helpSoundMixType":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_SOUND_MIXTYPE, Strings.CAPTION_HELP);
                    break;
                case "helpSoundIconID":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_SOUND_ICON, Strings.CAPTION_HELP);
                    break;
                case "helpBackRotationBehavior":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_ROTATION_BEHAVIOR, Strings.CAPTION_HELP);
                    break;
                case "helpCharacterBackgroundRotation":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_ROTATION_BACKGROUND, Strings.CAPTION_HELP);
                    break;
                case "helpOrderSoundTest":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_ORDER_SOUND_TEST, Strings.CAPTION_HELP);
                    break;
                case "helpOrderStageCreation":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_ORDER_STAGE_CREATION, Strings.CAPTION_HELP);
                    break;
                case "helpGroupStageCreation":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_GROUP_STAGE_CREATION, Strings.CAPTION_HELP);
                    break;
                case "helpInMyMusic":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_INSOUNDTEST, Strings.CAPTION_HELP);
                    break;
                case "helpShort17":
                case "helpUnknownByte2":
                case "helpUnknownByte3":
                case "helpUnknownByte4":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_UNKNOWN, Strings.CAPTION_HELP);
                    break;
                case "helpRegionJPN":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_REGIONJAP, Strings.CAPTION_HELP);
                    break;
                case "helpRegionUSEUR":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_REGIONUSAEUR, Strings.CAPTION_HELP);
                    break;
            }
            
        }
        #endregion
    }
}
