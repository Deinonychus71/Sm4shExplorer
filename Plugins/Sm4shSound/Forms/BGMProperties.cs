using Sm4shSound.Globals;
using Sm4shSound.Objects;
using System;
using System.Windows.Forms;

namespace Sm4shSound.Forms
{
    public partial class BGMProperties : Form
    {
        public BGMEntry CurrentSoundEntryBGM { get; set; }
        public BGMEntry CurrentSoundEntryBGMOriginal { get; set; }
        public string FormTitle { get { return this.Text; } set { this.Text = value; } }

        public BGMProperties()
        {
            InitializeComponent();
        }

        private void PopulateData(BGMEntry sEntryBGM)
        {
            lblBGMIDValue.Text = sEntryBGM.BGMID.ToString();
            lblBGMNameValue.Text = sEntryBGM.BGMTitle;
            lblBGMFilenameValue.Text = sEntryBGM.BGMFilename;
           
            ClearBindings(this);

            txtUnk1.DataBindings.Add("Text", sEntryBGM, "BGMUnk1");
            txtUnk2.DataBindings.Add("Text", sEntryBGM, "BGMUnk2");
            txtUnk3.DataBindings.Add("Text", sEntryBGM, "BGMUnk3");
            txtUnk4.DataBindings.Add("Text", sEntryBGM, "BGMUnk4");
            txtMenuCheckpoint1.DataBindings.Add("Text", sEntryBGM, "MenuCheckPoint1");
            txtMenuCheckpoint2.DataBindings.Add("Text", sEntryBGM, "MenuCheckPoint2");
            txtMenuCheckpoint3.DataBindings.Add("Text", sEntryBGM, "MenuCheckPoint3");
            txtMenuCheckpoint4.DataBindings.Add("Text", sEntryBGM, "MenuCheckPoint4");

            txtLoopStartSample.DataBindings.Add("Text", sEntryBGM, "LoopStartSample");
            txtLoopStartTime.DataBindings.Add("Text", sEntryBGM, "LoopStartTime");
            txtLoopEndSample.DataBindings.Add("Text", sEntryBGM, "LoopEndSample");
            txtLoopEndTime.DataBindings.Add("Text", sEntryBGM, "LoopEndTime");
            txtStreamTotalSamples.DataBindings.Add("Text", sEntryBGM, "StreamTotalSamples");
            txtStreamTotalTime.DataBindings.Add("Text", sEntryBGM, "StreamTotalDuration");

            btnRestoreProperties.Visible = CurrentSoundEntryBGMOriginal != null;
            btnRestoreSampleInfo.Visible = CurrentSoundEntryBGMOriginal != null;
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

        private void BGMProperties_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                PopulateData(CurrentSoundEntryBGM);
            }
        }

        private void BGMProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Validate();
            this.ValidateChildren();
        }

        private void btnRestoreProperties_Click(object sender, EventArgs e)
        {
            if (CurrentSoundEntryBGMOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                CurrentSoundEntryBGM.BGMUnk1 = CurrentSoundEntryBGMOriginal.BGMUnk1;
                CurrentSoundEntryBGM.BGMUnk2 = CurrentSoundEntryBGMOriginal.BGMUnk2;
                CurrentSoundEntryBGM.BGMUnk3 = CurrentSoundEntryBGMOriginal.BGMUnk3;
                CurrentSoundEntryBGM.BGMUnk4 = CurrentSoundEntryBGMOriginal.BGMUnk4;

                CurrentSoundEntryBGM.MenuCheckPoint1 = CurrentSoundEntryBGMOriginal.MenuCheckPoint1;
                CurrentSoundEntryBGM.MenuCheckPoint2 = CurrentSoundEntryBGMOriginal.MenuCheckPoint2;
                CurrentSoundEntryBGM.MenuCheckPoint3 = CurrentSoundEntryBGMOriginal.MenuCheckPoint3;
                CurrentSoundEntryBGM.MenuCheckPoint4 = CurrentSoundEntryBGMOriginal.MenuCheckPoint4;
            }
        }

        private void btnRestoreSampleInfo_Click(object sender, EventArgs e)
        {
            if (CurrentSoundEntryBGMOriginal != null && MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                CurrentSoundEntryBGM.LoopStartSample = CurrentSoundEntryBGMOriginal.LoopStartSample;
                CurrentSoundEntryBGM.LoopStartTime = CurrentSoundEntryBGMOriginal.LoopStartTime;
                CurrentSoundEntryBGM.LoopEndSample = CurrentSoundEntryBGMOriginal.LoopEndSample;
                CurrentSoundEntryBGM.LoopEndTime = CurrentSoundEntryBGMOriginal.LoopEndTime;
                CurrentSoundEntryBGM.StreamTotalDuration = CurrentSoundEntryBGMOriginal.StreamTotalDuration;
                CurrentSoundEntryBGM.StreamTotalSamples = CurrentSoundEntryBGMOriginal.StreamTotalSamples;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            PictureBox helpBtn = sender as PictureBox;
            if (helpBtn == null)
                return;

            switch (helpBtn.Name)
            {
                case "helpBGMIDValue":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_BGMID, Strings.CAPTION_HELP);
                    break;
                case "helpBGMNameValue":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_BGMNAME, Strings.CAPTION_HELP);
                    break;
                case "helpBGMFilenameValue":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_BGMFILENAME, Strings.CAPTION_HELP);
                    break;
                case "helpUnk1":
                case "helpUnk2":
                case "helpUnk3":
                case "helpUnk4":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_UNKNOWN, Strings.CAPTION_HELP);
                    break;
                case "helpMenuCheckpoint1":
                case "helpMenuCheckpoint2":
                case "helpMenuCheckpoint3":
                case "helpMenuCheckpoint4":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_MENUCHECKPOINT, Strings.CAPTION_HELP);
                    break;
                case "helpLoopStartSample":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_LOOPSTARTSAMPLE, Strings.CAPTION_HELP);
                    break;
                case "helpLoopEndSample":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_LOOPENDSAMPLE, Strings.CAPTION_HELP);
                    break;
                case "helpStreamTotalSamples":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_TOTALSAMPLES, Strings.CAPTION_HELP);
                    break;
                case "helpLoopStartTime":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_LOOPSTARTTIME, Strings.CAPTION_HELP);
                    break;
                case "helpLoopEndTime":
                    MessageBox.Show(Strings.HELP_PROPERTIESBGM_TOTALTIME, Strings.CAPTION_HELP);
                    break;
                case "helpStreamTotalTime":
                    MessageBox.Show(Strings.HELP_BGM_MANAG_TITLE, Strings.CAPTION_HELP);
                    break;
            }
        }
    }
}
