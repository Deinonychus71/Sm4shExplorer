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
        private int _BGMID;
        private int _BGMUnk1;
        private int _BGMUnk2;
        private int _BGMUnk3;
        private int _BGMUnk4;
        private int _MenuCheckPoint1;
        private int _MenuCheckPoint2;
        private int _MenuCheckPoint3;
        private int _MenuCheckPoint4;
        private int _LoopStartTime;
        private int _LoopEndTime;
        private int _LoopStartSample;
        private int _LoopEndSample;
        private int _StreamTotalDuration;
        private int _StreamTotalSamples;
        #endregion

        #region Properties
        [XmlElement("id")] 
        public int BGMID { get { return _BGMID; } set { _BGMID = value; } }
        [XmlElement("title")] 
        public string BGMTitle { get; set; }

        [XmlElement("i1")] 
        public int BGMUnk1 { get { return _BGMUnk1; } set { _BGMUnk1 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk1")); } }
        [XmlElement("i2")] 
        public int BGMUnk2 { get { return _BGMUnk2; } set { _BGMUnk2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk2")); } }
        [XmlElement("i3")] 
        public int BGMUnk3 { get { return _BGMUnk3; } set { _BGMUnk3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk3")); } }
        [XmlElement("i4")] 
        public int BGMUnk4 { get { return _BGMUnk4; } set { _BGMUnk4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("BGMUnk4")); } }
        [XmlElement("i5")] 
        public int MenuCheckPoint1 { get { return _MenuCheckPoint1; } set { _MenuCheckPoint1 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint1")); } }
        [XmlElement("i6")] 
        public int MenuCheckPoint2 { get { return _MenuCheckPoint2; } set { _MenuCheckPoint2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint2")); } }
        [XmlElement("i7")] 
        public int MenuCheckPoint3 { get { return _MenuCheckPoint3; } set { _MenuCheckPoint3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint3")); } }
        [XmlElement("i8")] 
        public int MenuCheckPoint4 { get { return _MenuCheckPoint4; } set { _MenuCheckPoint4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("MenuCheckPoint4")); } }

        [XmlElement("lst")] 
        public int LoopStartTime { get { return _LoopStartTime; } set { _LoopStartTime = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopStartTime")); } }
        [XmlElement("let")] 
        public int LoopEndTime { get { return _LoopEndTime; } set { _LoopEndTime = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopEndTime")); } }
        [XmlElement("lss")] 
        public int LoopStartSample { get { return _LoopStartSample; } set { _LoopStartSample = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopStartSample")); } }
        [XmlElement("les")] 
        public int LoopEndSample { get { return _LoopEndSample; } set { _LoopEndSample = value; InvokePropertyChanged(new PropertyChangedEventArgs("LoopEndSample")); } }
        [XmlElement("std")] 
        public int StreamTotalDuration { get { return _StreamTotalDuration; } set { _StreamTotalDuration = value; InvokePropertyChanged(new PropertyChangedEventArgs("StreamTotalDuration")); } }
        [XmlElement("sts")] 
        public int StreamTotalSamples { get { return _StreamTotalSamples; } set { _StreamTotalSamples = value; InvokePropertyChanged(new PropertyChangedEventArgs("StreamTotalSamples")); } }

        [XmlIgnore]
        public string BGMFilename { get { return "snd_bgm_" + BGMTitle + ".nus3bank"; } }
        [XmlIgnore]
        public string ListTitle { get { return BGMTitle; } }
        [XmlIgnore]
        public string ListValue { get { return BGMID.ToString(); } }
        #endregion

        public BGMEntry(int bgmID)
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
