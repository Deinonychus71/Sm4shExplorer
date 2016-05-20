//From lioncash
//https://github.com/lioncash/vgmstreamSharp
using System;
using NAudio.Wave;
using System.IO;

namespace Sm4shSound.Globals
{
    /// <summary>
    /// Class for VGMStream playback.
    /// </summary>
    public sealed class VGMStreamReader : WaveStream, IDisposable
    {
        private readonly WaveFormat _WaveFormat;
        private readonly IntPtr _Vgmstream;

        private int _TotalSamplesToPlay;           // Total samples to play
        private readonly int _Channels;      // Number of channels this VGMSTREAM uses.
        private readonly int _SampleRate;    // Sample rate of this VGMSTREAM.
        private readonly int loopCount = 1; // Number of times to loop. // TODO: Make configurable.
        private int _LoopStartSample;
        private int _LoopEndSample;
        private int _TotalSamples;
        private bool _FileLoaded = false;
        private int _TotalPlayed = 0;

        public int TotalPlayed {  get { return _TotalPlayed / _SampleRate; } }

        public int TotalSamplesToPlay { get { return _TotalSamplesToPlay; } }
        public int TotalSecondsToPlay { get { return _TotalSamplesToPlay / _SampleRate; } }
        public int LoopStartSample { get { return _LoopStartSample; } }
        public int LoopEndSample { get { return _LoopEndSample; } }
        public int TotalSamples { get { return _TotalSamples; } }
        public int LoopStartMilliseconds { get { return (int)(_LoopStartSample / (_SampleRate / 1000.00)); } }
        public int LoopEndMilliseconds { get { return (int)(_LoopEndSample / (_SampleRate / 1000.00)); } }
        public int TotalMilliseconds { get { return (int)(_TotalSamples / (_SampleRate / 1000.00)); } }
        public bool FileLoaded { get { return _FileLoaded; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filename">File to open in VGMStream.</param>
        public VGMStreamReader(string filename)
        {
            _Vgmstream = VGMStreamNative.InitVGMStream(filename);
            if (_Vgmstream == IntPtr.Zero)
            {
                _FileLoaded = false;
                return;
            }

            _FileLoaded = true;
            _SampleRate = VGMStreamNative.GetVGMStreamSampleRate(_Vgmstream);
            _Channels = VGMStreamNative.GetVGMStreamChannelCount(_Vgmstream);
            _TotalSamplesToPlay = VGMStreamNative.GetVGMStreamPlaySamples(loopCount, 0, 0, _Vgmstream);

            _LoopStartSample = VGMStreamNative.GetVGMStreamLoopStartSample(_Vgmstream);
            _LoopEndSample = VGMStreamNative.GetVGMStreamLoopEndSample(_Vgmstream) - 1; //Smash values always seem to be 1 less.
            _TotalSamples = VGMStreamNative.GetVGMStreamTotalSamples(_Vgmstream);

            _WaveFormat = new WaveFormat(_SampleRate, 16, _Channels);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            short[] sBuffer = new short[count / 2];

            // Generate samples.
            VGMStreamNative.RenderVGMStream(sBuffer, sBuffer.Length / _Channels, _Vgmstream);

            // Convert the short samples to byte samples and place them
            // in the NAudio byte sample buffer.
            Buffer.BlockCopy(sBuffer, 0, buffer, 0, buffer.Length);

            _TotalPlayed += sBuffer.Length / _Channels;

            return buffer.Length;
        }

        public override WaveFormat WaveFormat
        {
            get { return _WaveFormat; }
        }

        public override long Length
        {
            get
            {
                int lengthInMs = VGMStreamNative.GetVGMStreamPlaySamples(0, 0, 0, _Vgmstream) * 1000 / _SampleRate;
                return lengthInMs; // TODO: This should actually be in samples or bytes.
            }
        }

        // TODO: Add seeking support.
        public override long Position
        {
            get;
            set;
        }

        public void ResetVGM()
        {
            _TotalPlayed = 0;
            VGMStreamNative.ResetVGMStream(_Vgmstream);
        }

        #region IDisposable Methods

        public new void Dispose()
        {
            base.Dispose();

            try
            {
                VGMStreamNative.CloseVGMStream(_Vgmstream);
            }
            catch
            {
            }
        }

        #endregion
    }
}
