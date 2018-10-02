using Sm4shMusic.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "mmsb")]
    public class MyMusicStageBGM : ICloneable, INotifyPropertyChanged, ISm4shListItem
    {
        #region Attributes
        private short _Index;
        private short _SubIndex;
        private int _Rarity;
        private short _SaveMyMusic;
        private short _Unk4;
        private int _PlayDelay;
        private int _Unk6;
        private int _Unk7;
        private int _Unk8;
        private int _Unk9;
        private int _BGMID;
        #endregion

        #region Properties
        [XmlElement("bid")]
        public int BGMID { get { return _BGMID; } set { _BGMID = value; } }
        [XmlElement("in")] 
        public short Index { get { return _Index; } set { _Index = value; InvokePropertyChanged(new PropertyChangedEventArgs("Index")); } }
        [XmlElement("sub")] 
        public short SubIndex { get { return _SubIndex; } set { _SubIndex = value; InvokePropertyChanged(new PropertyChangedEventArgs("SubIndex")); } }
        [XmlElement("ra")] 
        public int Rarity { get { return _Rarity; } set { _Rarity = value; InvokePropertyChanged(new PropertyChangedEventArgs("Rarity")); } }
        [XmlElement("sh1")] 
        public short SaveMyMusic { get { return _SaveMyMusic; } set { _SaveMyMusic = value; InvokePropertyChanged(new PropertyChangedEventArgs("SaveMyMusic")); } }
        [XmlElement("sh2")] 
        public short Unk4 { get { return _Unk4; } set { _Unk4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk4")); } }
        [XmlElement("i1")] 
        public int PlayDelay { get { return _PlayDelay; } set { _PlayDelay = value; InvokePropertyChanged(new PropertyChangedEventArgs("PlayDelay")); } }
        [XmlElement("i2")] 
        public int Unk6 { get { return _Unk6; } set { _Unk6 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk6")); } }
        [XmlElement("i3")] 
        public int Unk7 { get { return _Unk7; } set { _Unk7 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk7")); } }
        [XmlElement("i4")] 
        public int Unk8 { get { return _Unk8; } set { _Unk8 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk8")); } }
        [XmlElement("i5")] 
        public int Unk9 { get { return _Unk9; } set { _Unk9 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk9")); } }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public BGMEntry BGMEntry { get { return SoundEntryCollection.SoundEntriesBGMsPerID[_BGMID]; } }
        [XmlIgnore]
        public string ListTitle { get { return BGMEntry.BGMTitle; } }
        [XmlIgnore]
        public string ListValue { get { return BGMEntry.BGMID.ToString(); } }
        #endregion

        public MyMusicStageBGM(SoundEntryCollection soundEntryCollection, int bgmID)
        {
            SoundEntryCollection = soundEntryCollection;
            _BGMID = bgmID;
        }

        public MyMusicStageBGM()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
        #endregion
    }
}
