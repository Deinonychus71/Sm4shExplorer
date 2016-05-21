using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "be")]
    public class BGMEntry : ISm4shListItem, INotifyPropertyChanged, ICloneable
    {
        #region Attributes
        private uint _BGMID;
        private uint _BGMUnk1;
        private uint _BGMUnk2;
        private uint _BGMUnk3;
        private uint _BGMUnk4;
        private uint _MenuCheckPoint1;
        private uint _MenuCheckPoint2;
        private uint _MenuCheckPoint3;
        private uint _MenuCheckPoint4;
        private uint _LoopStartTime;
        private uint _LoopEndTime;
        private uint _LoopStartSample;
        private uint _LoopEndSample;
        private uint _StreamTotalDuration;
        private uint _StreamTotalSamples;
        #endregion

        #region Properties
        [XmlElement("id")] 
        public uint BGMID { get { return _BGMID; } set { _BGMID = value; } }
        [XmlElement("title")] 
        public string BGMTitle { get; set; }

        [XmlElement("u1")] 
        public uint BGMUnk1 { get { return _BGMUnk1; } set { _BGMUnk1 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk1")); } }
        [XmlElement("u2")] 
        public uint BGMUnk2 { get { return _BGMUnk2; } set { _BGMUnk2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk2")); } }
        [XmlElement("u3")] 
        public uint BGMUnk3 { get { return _BGMUnk3; } set { _BGMUnk3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk3")); } }
        [XmlElement("u4")] 
        public uint BGMUnk4 { get { return _BGMUnk4; } set { _BGMUnk4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk4")); } }
        [XmlElement("u5")] 
        public uint MenuCheckPoint1 { get { return _MenuCheckPoint1; } set { _MenuCheckPoint1 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint1")); } }
        [XmlElement("u6")] 
        public uint MenuCheckPoint2 { get { return _MenuCheckPoint2; } set { _MenuCheckPoint2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint2")); } }
        [XmlElement("u7")] 
        public uint MenuCheckPoint3 { get { return _MenuCheckPoint3; } set { _MenuCheckPoint3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint3")); } }
        [XmlElement("u8")] 
        public uint MenuCheckPoint4 { get { return _MenuCheckPoint4; } set { _MenuCheckPoint4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint4")); } }

        [XmlElement("lst")] 
        public uint LoopStartTime { get { return _LoopStartTime; } set { _LoopStartTime = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopStartTime")); } }
        [XmlElement("let")] 
        public uint LoopEndTime { get { return _LoopEndTime; } set { _LoopEndTime = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopEndTime")); } }
        [XmlElement("lss")] 
        public uint LoopStartSample { get { return _LoopStartSample; } set { _LoopStartSample = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopStartSample")); } }
        [XmlElement("les")] 
        public uint LoopEndSample { get { return _LoopEndSample; } set { _LoopEndSample = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopEndSample")); } }
        [XmlElement("std")] 
        public uint StreamTotalDuration { get { return _StreamTotalDuration; } set { _StreamTotalDuration = value; InvokePropertyChanged(new PropertyChangedEventArgs("StreamTotalDuration")); } }
        [XmlElement("sts")] 
        public uint StreamTotalSamples { get { return _StreamTotalSamples; } set { _StreamTotalSamples = value; InvokePropertyChanged(new PropertyChangedEventArgs("StreamTotalSamples")); } }

        [XmlIgnore]
        public string BGMFilename { get { return "snd_bgm_" + BGMTitle + ".nus3bank"; } }
        [XmlIgnore]
        public string ListTitle { get { return BGMTitle; } }
        [XmlIgnore]
        public string ListValue { get { return BGMID.ToString(); } }
        #endregion

        public BGMEntry(uint bgmID)
        {
            _BGMID = bgmID;
        }

        public BGMEntry()
        {
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return BGMTitle;
        }
    }
}
