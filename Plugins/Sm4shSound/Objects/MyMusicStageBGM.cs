using Sm4shSound.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shSound.Objects
{
    [XmlType(TypeName = "mmsb")]
    public class MyMusicStageBGM : ICloneable, INotifyPropertyChanged, ISm4shListItem
    {
        #region Attributes
        private ushort _Index;
        private ushort _SubIndex;
        private uint _Rarity;
        private ushort _Unk3;
        private ushort _Unk4;
        private uint _Unk5;
        private uint _Unk6;
        private uint _Unk7;
        private uint _Unk8;
        private uint _Unk9;
        private uint _BGMID;
        #endregion

        #region Properties
        [XmlElement("bid")]
        public uint BGMID { get { return _BGMID; } set { _BGMID = value; } }
        [XmlElement("in")] 
        public ushort Index { get { return _Index; } set { _Index = value; InvokePropertyChanged(new PropertyChangedEventArgs("Index")); } }
        [XmlElement("sub")] 
        public ushort SubIndex { get { return _SubIndex; } set { _SubIndex = value; InvokePropertyChanged(new PropertyChangedEventArgs("SubIndex")); } }
        [XmlElement("ra")] 
        public uint Rarity { get { return _Rarity; } set { _Rarity = value; InvokePropertyChanged(new PropertyChangedEventArgs("Rarity")); } }
        [XmlElement("u3")] 
        public ushort Unk3 { get { return _Unk3; } set { _Unk3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk3")); } }
        [XmlElement("u4")] 
        public ushort Unk4 { get { return _Unk4; } set { _Unk4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk4")); } }
        [XmlElement("u5")] 
        public uint Unk5 { get { return _Unk5; } set { _Unk5 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk5")); } }
        [XmlElement("u6")] 
        public uint Unk6 { get { return _Unk6; } set { _Unk6 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk6")); } }
        [XmlElement("u7")] 
        public uint Unk7 { get { return _Unk7; } set { _Unk7 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk7")); } }
        [XmlElement("u8")] 
        public uint Unk8 { get { return _Unk8; } set { _Unk8 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk8")); } }
        [XmlElement("u9")] 
        public uint Unk9 { get { return _Unk9; } set { _Unk9 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Unk9")); } }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public BGMEntry BGMEntry { get { return SoundEntryCollection.SoundEntriesBGMsPerID[_BGMID]; } }
        [XmlIgnore]
        public string ListTitle { get { return BGMEntry.BGMTitle; } }
        [XmlIgnore]
        public string ListValue { get { return BGMEntry.BGMID.ToString(); } }
        #endregion

        public MyMusicStageBGM(SoundEntryCollection soundEntryCollection, uint bgmID)
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
