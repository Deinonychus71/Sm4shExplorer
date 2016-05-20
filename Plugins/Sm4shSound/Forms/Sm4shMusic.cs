using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shSound.Globals;
using Sm4shSound.Objects;
using Sm4shSound.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Sm4shSound.Forms
{
    public partial class Sm4shMusic : Form
    {
        public const string VERSION = "0.01";
        private BGMManagement _BGMManagement;
        private Sm4shProject _Sm4shProject;
        private MyMusicManagement _MyMusicManagement;
        private About _About;
        private SoundEntryCollection _SoundEntryCollection;

        public SoundEntryCollection SoundEntryCollection
        {
            get { return _SoundEntryCollection; }
            set
            {
                _SoundEntryCollection = value;
                SoundEntryCollection sEntryCollectionOriginal = (SoundEntryCollection)_SoundEntryCollection.Clone();
                _BGMManagement.SoundEntryCollection = _SoundEntryCollection;
                _BGMManagement.SoundEntryCollectionBackup = sEntryCollectionOriginal;
                _MyMusicManagement.SoundEntryCollection = _SoundEntryCollection;
                _MyMusicManagement.SoundEntryCollectionBackup = sEntryCollectionOriginal;
                _About = new About();
            }
        }

        public Sm4shMusic()
        {
            InitializeComponent();
            _BGMManagement = new BGMManagement();
            tabBGMManagement.Controls.Add(_BGMManagement);
            _BGMManagement.Dock = DockStyle.Fill;
            _MyMusicManagement = new MyMusicManagement();
            tabStage.Controls.Add(_MyMusicManagement);
            _MyMusicManagement.Dock = DockStyle.Fill;
            this.Text += " v" + VERSION;
        }

        public void Initialize(Sm4shProject sm4shProject)
        {
            _Sm4shProject = sm4shProject;

            //Init sound files
            refreshBGMFilesListToolStripMenuItem_Click(this, new EventArgs());

            _BGMManagement.Initialize(sm4shProject);
            _MyMusicManagement.Initialize(sm4shProject);
        }

        #region Buttons
        private void generateCSVForSoundDBAndMSBTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.FileName = "SoundDB.csv";
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string pathToSave = saveFileDialog.FileName;
                Debug.WriteDebugSoundMSBTCSV(_SoundEntryCollection, pathToSave);
                LogHelper.Info(string.Format(Strings.DEBUG_EXPORT, pathToSave));
            }
        }

        private void generateCSVForBGMEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.FileName = "SoundEntriesBGMs.csv";
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string pathToSave = saveFileDialog.FileName;
                Debug.WriteDebugBGMEntriesCSV(_SoundEntryCollection, pathToSave);
                LogHelper.Info(string.Format(Strings.DEBUG_EXPORT, pathToSave));
            }
        }

        private void generateCSVForMyMusicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.FileName = "MyMusic.csv";
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string pathToSave = saveFileDialog.FileName;
                Debug.WriteDebugMyMusicCSV(_SoundEntryCollection, pathToSave);
                LogHelper.Info(string.Format(Strings.DEBUG_EXPORT, pathToSave));
            }
        }

        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ValidateChildren();
            saveFileDialog.Filter = "XML files|*.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FileName = "sm4shsound_config.xml";
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string pathToSave = saveFileDialog.FileName;
                Serializer.SerializeObjectToXml(_SoundEntryCollection, pathToSave);
                LogHelper.Info(string.Format("Sm4shMusic: Configuration '{0}' saved.", pathToSave));
            }
        }

        private void loadConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "XML files|*.xml";
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FileName = "sm4shsound_config.xml";
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string pathToOpen = openFileDialog.FileName;
                SoundEntryCollection = (SoundEntryCollection)Serializer.DeserializeXmlToObject<SoundEntryCollection>(pathToOpen).Clone();

                //Hack for textarea
                foreach(SoundEntry sEntry in SoundEntryCollection.SoundEntries)
                {
                    sEntry.Title = sEntry.Title.Replace("\n", Environment.NewLine);
                    sEntry.SoundTestTitle = sEntry.SoundTestTitle.Replace("\n", Environment.NewLine);
                    sEntry.Description = sEntry.Description.Replace("\n", Environment.NewLine);
                    sEntry.Description2 = sEntry.Description2.Replace("\n", Environment.NewLine);
                    sEntry.Source = sEntry.Source.Replace("\n", Environment.NewLine);
                }

                LogHelper.Info(string.Format("Sm4shMusic: Configuration '{0}' loaded.", pathToOpen));
                Initialize(_Sm4shProject);
            }
        }

        private void listAllOrphanBGMsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Info(SoundEntryCollection.CleanBGMDatabase(false));
        }

        private void compileTheModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ValidateChildren();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void exitWillCancelAllChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refreshBGMFilesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sm4shSoundTools.RefreshSoundFiles();
            List<string> soundFiles = Sm4shSoundTools.SoundFiles;

            SoundEntryCollection.CleanBGMDatabase(true);
            foreach (string soundFile in soundFiles)
            {
                if (_SoundEntryCollection.SoundEntriesBGMsPerName.ContainsKey(soundFile))
                    continue;
                SoundEntryCollection.CreateBGMEntry(soundFile);
            }

            _BGMManagement.RefreshBGMTextArea();

            LogHelper.Info("Sm4shMusic: BGM List refreshed.");
        }

        private void thanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _About.ShowDialog(this);
        }
        #endregion 

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            VGMStreamPlayer.StopCurrentVGMStreamPlayback();
            _BGMManagement.Clean();
            _MyMusicManagement.Clean();
        }
    }
}
