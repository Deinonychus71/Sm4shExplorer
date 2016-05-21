using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sm4shMusic.Objects;
using Sm4shMusic.Globals;
using Sm4shFileExplorer;

namespace Sm4shMusic.UserControls
{
    public partial class MyMusicManagement : UserControl
    {
        private MusicStageList _ListSoundDB;
        private MusicStageList _ListMyMusic;
        private MyMusicStage _CurrentMyMusicStage;
        private MyMusicStage _CurrentMyMusicStageOriginal;
        private SoundDBStage _CurrentSoundDBStage;
        private SoundDBStage _CurrentSoundDBStageOriginal;
        private MyMusicStageBGM _CurrentMyMusicStageBGM;
        private MyMusicStageBGM _CurrentMyMusicStageBGMOriginal;

        public SoundEntryCollection SoundEntryCollection { get; set; }
        public SoundEntryCollection SoundEntryCollectionBackup { get; set; }

        public MyMusicManagement()
        {
            InitializeComponent();
            _ListSoundDB = new MusicStageList();
            _ListSoundDB.Name = "SoundDB";
            _ListSoundDB.Dock = DockStyle.Fill;
            _ListSoundDB.ItemMoving += _ListSoundDB_ItemMoving;
            _ListSoundDB.ItemRemoved += _ListSoundDB_ItemRemoved;
            _ListSoundDB.ItemAdded += _ListSoundDB_ItemAdded;
            _ListSoundDB.ItemSelected += _ListSoundDB_ItemSelected;
            _ListMyMusic = new MusicStageList();
            _ListMyMusic.Name = "MyMusic";
            _ListMyMusic.Dock = DockStyle.Fill;
            _ListMyMusic.ItemMoving += _ListMyMusic_ItemMoving;
            _ListMyMusic.ItemRemoved += _ListMyMusic_ItemRemoved;
            _ListMyMusic.ItemAdded += _ListMyMusic_ItemAdded;
            _ListMyMusic.ItemSelected += _ListMyMusic_ItemSelected;
            groupListSounds.Controls.Add(_ListSoundDB);
            groupListMyMusic.Controls.Add(_ListMyMusic);
        }

        public void Initialize(Sm4shProject sm4shProject)
        {
            //Init List
            _ListMyMusic.MaxItems = 200;
            _ListSoundDB.MaxItems = 40;

            listEntries.DataSource = SoundEntryCollection.MyMusicStages;
            _ListSoundDB.DataSource = SoundEntryCollection.SoundEntries;
        }

        #region Methods
        private void PopulateInfo(MyMusicStage myMusicStage)
        {
            VGMStreamPlayer.StopCurrentVGMStreamPlayback();
            _CurrentMyMusicStage = myMusicStage;
            _CurrentMyMusicStageOriginal = SoundEntryCollectionBackup.MyMusicStagesPerID[myMusicStage.MyMusicStageID];
            if(myMusicStage.BGMStage.BGMDBID != null)
            {
                _CurrentSoundDBStage = SoundEntryCollection.SoundDBStagesPerID[(uint)(myMusicStage.BGMStage.BGMDBID)];
                _CurrentSoundDBStageOriginal = SoundEntryCollectionBackup.SoundDBStagesPerID[(uint)(myMusicStage.BGMStage.BGMDBID)];
                _ListSoundDB.Enabled = true;
                btnRestoreSoundDBStageList.Visible = true;
            }
            else
            {
                _CurrentSoundDBStage = null;
                _CurrentSoundDBStageOriginal = null;
                _ListSoundDB.Enabled = false;
                btnRestoreSoundDBStageList.Visible = false;
            }

            lblMyMusicIDValue.Text = myMusicStage.MyMusicStageID.ToString();
            lblSoundDBIDValue.Text = myMusicStage.BGMStage.BGMDBID.ToString();

            _ListMyMusic.Items = myMusicStage.BGMs;
            if (_CurrentSoundDBStage != null)
                _ListSoundDB.Items = _CurrentSoundDBStage.SoundEntries;
            else
                _ListSoundDB.Items = null;

        }

        private void PopulateInfoStageBGM(MyMusicStageBGM myMusicStageBGM)
        {
            this.groupMusicStageBGM.Visible = myMusicStageBGM != null;
            if (myMusicStageBGM == null)
                return;

            _CurrentMyMusicStageBGM = myMusicStageBGM;
            _CurrentMyMusicStageBGMOriginal = _CurrentMyMusicStageOriginal.BGMs.Find(p => p.BGMID == myMusicStageBGM.BGMID);
            this.btnRestoreMyMusicProperties.Visible = _CurrentMyMusicStageBGMOriginal != null;

            BGMEntry sEntryBGM = SoundEntryCollection.SoundEntriesBGMsPerID[myMusicStageBGM.BGMID];

            lblBGMIDValue.Text = myMusicStageBGM.BGMID.ToString();
            lblBGMNameValue.Text = sEntryBGM.BGMTitle;

            _ListMyMusic.VGMStreamFile = SoundEntryCollection.GetBGMFullPath(sEntryBGM.BGMFilename);

            //Properties supporting binding
            ClearBindings(this);

            txtBGMIndex.DataBindings.Add("Text", myMusicStageBGM, "Index");
            txtBGMSubIndex.DataBindings.Add("Text", myMusicStageBGM, "SubIndex");
            txtRarity.DataBindings.Add("Text", myMusicStageBGM, "Rarity");
            txtUnk3.DataBindings.Add("Text", myMusicStageBGM, "Unk3");
            txtUnk4.DataBindings.Add("Text", myMusicStageBGM, "Unk4");
            txtUnk5.DataBindings.Add("Text", myMusicStageBGM, "Unk5");
            txtUnk6.DataBindings.Add("Text", myMusicStageBGM, "Unk6");
            txtUnk7.DataBindings.Add("Text", myMusicStageBGM, "Unk7");
            txtUnk8.DataBindings.Add("Text", myMusicStageBGM, "Unk8");
            txtUnk9.DataBindings.Add("Text", myMusicStageBGM, "Unk9");
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
            _ListSoundDB.VGMStreamFile = null;
            _ListMyMusic.VGMStreamFile = null;
        }
        #endregion

        #region Event Handlers
        private void listEntries_ItemSelected(object sender, EventHandlers.ListEntryArgs e)
        {
            MyMusicStage myMusicStage = e.ListEntry as MyMusicStage;
            if (myMusicStage != null)
                PopulateInfo(myMusicStage);
        }

        private void MyMusicManagement_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                _ListSoundDB.RefreshData();
                _ListMyMusic.DataSource = SoundEntryCollection.SoundEntriesBGMs.OrderBy(p => p.BGMTitle).ToList();
            }
        }

        private void btnRestoreSoundDBStageList_Click(object sender, EventArgs e)
        {
            if (_CurrentSoundDBStageOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _CurrentSoundDBStage.SoundEntries.Clear();
                foreach (SoundDBStageSoundEntry sSoundDBStageSoundEntry in _CurrentSoundDBStageOriginal.SoundEntries)
                {
                    SoundDBStageSoundEntry nSoundDBStageSoundEntry = (SoundDBStageSoundEntry)sSoundDBStageSoundEntry.Clone();
                    nSoundDBStageSoundEntry.SoundEntryCollection = SoundEntryCollection;
                    _CurrentSoundDBStage.SoundEntries.Add(nSoundDBStageSoundEntry);
                }
                _ListSoundDB.RefreshItems();
            }
        }

        private void btnRestoreMyMusicStageList_Click(object sender, EventArgs e)
        {
            if (_CurrentMyMusicStageOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _CurrentMyMusicStage.BGMs.Clear();
                foreach (MyMusicStageBGM myMusicStageBGM in _CurrentMyMusicStageOriginal.BGMs)
                {
                    MyMusicStageBGM nMyMusicStageBGM = (MyMusicStageBGM)myMusicStageBGM.Clone();
                    nMyMusicStageBGM.SoundEntryCollection = SoundEntryCollection;
                    _CurrentMyMusicStage.BGMs.Add(nMyMusicStageBGM);
                }
                _ListMyMusic.RefreshItems();
            }
        }

        private void btnRestoreMyMusicProperties_Click(object sender, EventArgs e)
        {
            if (_CurrentMyMusicStageBGMOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _CurrentMyMusicStageBGM.Index = _CurrentMyMusicStageBGMOriginal.Index;
                _CurrentMyMusicStageBGM.SubIndex = _CurrentMyMusicStageBGMOriginal.SubIndex;
                _CurrentMyMusicStageBGM.Rarity = _CurrentMyMusicStageBGMOriginal.Rarity;
                _CurrentMyMusicStageBGM.Unk3 = _CurrentMyMusicStageBGMOriginal.Unk3;
                _CurrentMyMusicStageBGM.Unk4 = _CurrentMyMusicStageBGMOriginal.Unk4;
                _CurrentMyMusicStageBGM.Unk5 = _CurrentMyMusicStageBGMOriginal.Unk5;
                _CurrentMyMusicStageBGM.Unk6 = _CurrentMyMusicStageBGMOriginal.Unk6;
                _CurrentMyMusicStageBGM.Unk7 = _CurrentMyMusicStageBGMOriginal.Unk7;
                _CurrentMyMusicStageBGM.Unk8 = _CurrentMyMusicStageBGMOriginal.Unk8;
                _CurrentMyMusicStageBGM.Unk9 = _CurrentMyMusicStageBGMOriginal.Unk9;
            }
        }

        #region SoundDB/MyMusic Lists
        private void _ListSoundDB_ItemMoving(object sender, EventHandlers.MoveItemArgs e)
        {
            if (_CurrentSoundDBStage == null)
                return;
            int newIndex = e.Index + e.Direction;
            _CurrentSoundDBStage.SoundEntries.RemoveAt(e.Index);
            if (e.Direction == -1 && newIndex > e.Index) newIndex--;
            _CurrentSoundDBStage.SoundEntries.Insert(newIndex, (SoundDBStageSoundEntry)e.ListEntry);
        }
        private void _ListMyMusic_ItemMoving(object sender, EventHandlers.MoveItemArgs e)
        {
            int newIndex = e.Index + e.Direction;
            _CurrentMyMusicStage.BGMs.RemoveAt(e.Index);
            if (e.Direction == -1 && newIndex > e.Index) newIndex--;
            _CurrentMyMusicStage.BGMs.Insert(newIndex, (MyMusicStageBGM)e.ListEntry);
        }
        private void _ListSoundDB_ItemRemoved(object sender, EventHandlers.ListEntryArgs e)
        {
            SoundDBStageSoundEntry sSoundDBStageSoundEntry = e.ListEntry as SoundDBStageSoundEntry;
            if (sSoundDBStageSoundEntry != null)
            {
                _CurrentSoundDBStage.SoundEntries.Remove(sSoundDBStageSoundEntry);
                bool needAdd = false;
                List<BGMEntry> myMusicEntryBGMs = new List<BGMEntry>();
                foreach (MyMusicStageBGM myMusicStageBGM in _CurrentMyMusicStage.BGMs)
                    myMusicEntryBGMs.Add(myMusicStageBGM.BGMEntry);
                foreach (SoundEntryBGM sEntryBGM in sSoundDBStageSoundEntry.SoundEntry.BGMFiles)
                {
                    if (myMusicEntryBGMs.Find(p => p.BGMID == sEntryBGM.BGMID) != null)
                    {
                        needAdd = true;
                        break;
                    }
                }
                if (needAdd && MessageBox.Show(Strings.WARNING_COPY_SOUND_REMOVE, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (SoundEntryBGM sEntryBGM in sSoundDBStageSoundEntry.SoundEntry.BGMFiles)
                    {
                        if (myMusicEntryBGMs.Find(p => p.BGMID == sEntryBGM.BGMID) != null)
                            _CurrentMyMusicStage.BGMs.Remove(_CurrentMyMusicStage.BGMs.Find(p => p.BGMEntry.BGMID == sEntryBGM.BGMID));
                    }
                    _ListMyMusic.RefreshItems();
                }
            }
        }
        private void _ListMyMusic_ItemRemoved(object sender, EventHandlers.ListEntryArgs e)
        {
            _CurrentMyMusicStage.BGMs.Remove((MyMusicStageBGM)e.ListEntry);
        }
        private void _ListSoundDB_ItemAdded(object sender, EventHandlers.ListEntryArgs e)
        {
            SoundEntry sEntry = e.ListEntry as SoundEntry;
            if (sEntry != null)
            {
                foreach (SoundDBStageSoundEntry sSoundDBStageSoundEntry in _CurrentSoundDBStage.SoundEntries)
                {
                    if (sEntry.SoundID == sSoundDBStageSoundEntry.SoundEntry.SoundID)
                    {
                        MessageBox.Show(Strings.ERROR_SOUND_ADD, Strings.CAPTION_ERROR);
                        return;
                    }
                }
                _CurrentSoundDBStage.SoundEntries.Add(new SoundDBStageSoundEntry(SoundEntryCollection,sEntry.SoundID));
                //Check if bgms need to be added
                bool needAdd = false;
                List<BGMEntry> myMusicEntryBGMs = new List<BGMEntry>();
                foreach (MyMusicStageBGM myMusicStageBGM in _CurrentMyMusicStage.BGMs)
                    myMusicEntryBGMs.Add(myMusicStageBGM.BGMEntry);
                foreach (SoundEntryBGM sEntryBGM in sEntry.BGMFiles)
                {
                    if (myMusicEntryBGMs.Find(p => p.BGMID == sEntryBGM.BGMID) == null)
                    {
                        needAdd = true;
                        break;
                    }
                }
                if (needAdd && MessageBox.Show(Strings.WARNING_COPY_SOUND, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (SoundEntryBGM sEntryBGM in sEntry.BGMFiles)
                    {
                        if (myMusicEntryBGMs.Find(p => p.BGMID == sEntryBGM.BGMID) == null)
                        {
                            MyMusicStageBGM myMusicStageBGM = SoundEntryCollection.CreateMyMusicStageBGM(sEntryBGM.BGMID, (ushort)_CurrentMyMusicStage.BGMs.Count);
                            _CurrentMyMusicStage.BGMs.Add(myMusicStageBGM);
                        }
                    }
                    _ListMyMusic.RefreshItems();
                }
            }

        }
        private void _ListMyMusic_ItemAdded(object sender, EventHandlers.ListEntryArgs e)
        {
            BGMEntry sEntryBGM = e.ListEntry as BGMEntry;
            if (sEntryBGM != null)
            {
                MyMusicStageBGM myMusicStageBGM = SoundEntryCollection.CreateMyMusicStageBGM(sEntryBGM.BGMID, (ushort)_CurrentMyMusicStage.BGMs.Count);
                _CurrentMyMusicStage.BGMs.Add(myMusicStageBGM);
                foreach (SoundDBStageSoundEntry sDBStageSoundEntry in _CurrentSoundDBStage.SoundEntries)
                {
                    if (sDBStageSoundEntry.SoundEntry.BGMFiles.Find(p => p.BGMEntry == sEntryBGM) != null)
                        return;
                }
                if (MessageBox.Show(Strings.WARNING_COPY_MYMUSIC, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (SoundEntry sEntry in SoundEntryCollection.SoundEntries)
                    {
                        if (sEntry.BGMFiles.Find(p => p.BGMID == sEntryBGM.BGMID) != null)
                        {
                            _CurrentSoundDBStage.SoundEntries.Add(new SoundDBStageSoundEntry(SoundEntryCollection,sEntry.SoundID));
                            break;
                        }
                    }
                    _ListSoundDB.RefreshItems();
                }
            }
        }
        private void _ListSoundDB_ItemSelected(object sender, EventHandlers.ListEntryArgs e)
        {
            SoundDBStageSoundEntry sSoundDBStageSoundEntry = e.ListEntry as SoundDBStageSoundEntry;
            if (sSoundDBStageSoundEntry != null)
                _ListSoundDB.VGMStreamFile = SoundEntryCollection.GetBGMFullPath(sSoundDBStageSoundEntry.SoundEntry.BGMFiles[0].BGMEntry.BGMFilename);
            else
                _ListSoundDB.VGMStreamFile = null;
        }
        private void _ListMyMusic_ItemSelected(object sender, EventHandlers.ListEntryArgs e)
        {
            PopulateInfoStageBGM((MyMusicStageBGM)e.ListEntry);
        }
        #endregion

        #endregion

        private void help_Click(object sender, EventArgs e)
        {
            PictureBox helpBtn = sender as PictureBox;
            if (helpBtn == null)
                return;

            switch (helpBtn.Name)
            {
                case "helpMyMusicID":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_MYMUSIC_ID, Strings.CAPTION_HELP);
                    break;
                case "helpSoundDBID":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_SOUNDDB_ID, Strings.CAPTION_HELP);
                    break;
                case "helpBGMIndex":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_INDEX, Strings.CAPTION_HELP);
                    break;
                case "helpBGMSubIndex":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_SUBINDEX, Strings.CAPTION_HELP);
                    break;
                case "helpRarity":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_RARITY, Strings.CAPTION_HELP);
                    break;
                case "helpUnk3":
                case "helpUnk4":
                case "helpUnk5":
                case "helpUnk6":
                case "helpUnk7":
                case "helpUnk8":
                case "helpUnk9":
                    MessageBox.Show(Strings.HELP_MUSIC_STAGE_UNKNOWN, Strings.CAPTION_HELP);
                    break;
            }
        }
    }
}
