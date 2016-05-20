using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sm4shSound.Globals;
using NAudio.Wave;
using System.IO;
using Sm4shFileExplorer.Globals;

namespace Sm4shSound.UserControls
{
    public partial class VGMStreamPlayer : UserControl
    {
        private const string EMPTY_SOUND = "00:00 / 00:00";

        private string _File;
        private VGMStreamReader _Reader;
        private string _FormatTotalTime;
        private Timer _Timer;
        public bool Valid { get; set; }
        private static IWavePlayer _Player;
        private static VGMStreamPlayer _CurrentVGMStreamPlayer;

        public string File
        {
            get
            {
                return _File;
            }
            set
            {
                if (_File != value)
                {
                    _File = value;
                    LoadFile();
                }
            }
        }
        public static VGMStreamPlayer CurrentVGMStreamPlayer { get { return _CurrentVGMStreamPlayer; } }

        public VGMStreamPlayer()
        {
            InitializeComponent();
            _Player = new WaveOut();
            _Timer = new Timer();
            _Timer.Interval = 1000;
            _Timer.Tick += _Timer_Tick;
        }

        private void _Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = new TimeSpan(0, 0, _Reader.TotalPlayed).ToString(@"mm\:ss") + _FormatTotalTime;
        }

        private void LoadFile()
        {
            this.Enabled = false;
            Valid = false;
            if (_Reader != null)
            {
                _Reader.Dispose();
                _Reader = null;
            }
            if (_File != null)
            {
                if (System.IO.File.Exists(_File) && !Path.GetFileName(_File).StartsWith("snd_bgm_TEST"))
                    Valid = true;
                else
                {
                    lblTime.Text = EMPTY_SOUND;
                    return;
                }

                _Reader = new VGMStreamReader(_File);
                if (!_Reader.FileLoaded)
                {
                    LogHelper.Warning(string.Format(Strings.WARNING_VGMSTREAM_LOAD_BGM, _File));
                    lblTime.Text = EMPTY_SOUND;
                    return;
                }
                _FormatTotalTime = " / " + new TimeSpan(0, 0, _Reader.TotalSecondsToPlay).ToString(@"mm\:ss");
                lblTime.Text = "00:00" + _FormatTotalTime;
                this.Enabled = true;

            }
            else
                lblTime.Text = EMPTY_SOUND;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_Player != null && _Reader != null && _Reader.FileLoaded)
            {
                //In case launching a new player, reset the old one
                if (_CurrentVGMStreamPlayer != null && _CurrentVGMStreamPlayer != this)
                    StopCurrentVGMStreamPlayback();

                if (_Player.PlaybackState == PlaybackState.Playing)
                {
                    _Player.Stop();
                    btnPlay.Image = Properties.Resources.play;
                }
                else
                {
                    if (_CurrentVGMStreamPlayer == null)
                        _Player.Init(_Reader);
                    _Player.Play();
                    _Timer.Start();
                    _CurrentVGMStreamPlayer = this;
                    btnPlay.Image = Properties.Resources.stop;
                }
            }
        }

        public static void StopCurrentVGMStreamPlayback()
        {
            if (_CurrentVGMStreamPlayer == null)
                return;

            _CurrentVGMStreamPlayer._Reader.ResetVGM();
            _CurrentVGMStreamPlayer._Timer.Stop();
            _CurrentVGMStreamPlayer.lblTime.Text = "00:00" + _CurrentVGMStreamPlayer._FormatTotalTime;
            _CurrentVGMStreamPlayer.btnPlay.Image = Properties.Resources.play;
            if (_Player.PlaybackState == PlaybackState.Playing)
            {
                _Player.Stop();
                _Player.Dispose();
                _Player = new WaveOut();
            }
            _CurrentVGMStreamPlayer = null;
        }
    }
}
