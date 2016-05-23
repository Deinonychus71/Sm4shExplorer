using Sm4shFileExplorer.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    public enum SoundMixType
    {
        Original = 0,
        Remix = 1,
        SmashOriginal = 2,
        RemixRed = 3
    }

    public enum SoundSource
    {
        CoreGameSound = 0,
        DLCSound = 1,
        UpdateSound = 2, //Miiverse
        TournamentSound = 3
    }

    public enum SoundTestBackImageBehavior
    {
        NULL = 0,
        CharacterRandom = 1,
        RosterRandom = 2,
        CharacterFirst = 3, //TOTEST
        UnlockableCharacterFirst = 4 //TOTEST
    }

    [XmlType(TypeName = "se")]
    public class SoundEntry : ISm4shListItem, INotifyPropertyChanged, ICloneable
    {
        #region Attributes
        private bool _InSoundTest;
        private bool _Byte2;
        private bool _Byte3;
        private bool _Byte4;
        private bool _InRegionJPN;
        private bool _InRegionEUUS;
        private short _Int17;
        private string _Title;
        private string _Title2;
        private string _Description;
        private string _Description2;
        private string _Source;
        #endregion

        #region Properties
        [XmlElement("sid")]
        public string SoundID { get; set; }
        [XmlElement("in")]
        public uint Index { get; set; }
        public List<SoundEntryBGM> BGMFiles { get; set; }

        [XmlElement("b1")]
        public bool InSoundTest { get { return _InSoundTest; } set { _InSoundTest = value; InvokePropertyChanged(new PropertyChangedEventArgs("InSoundTest")); } }
        [XmlElement("b2")]
        public bool Byte2 { get { return _Byte2; } set { _Byte2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Byte2")); } }
        [XmlElement("b3")]
        public bool Byte3 { get { return _Byte3; } set { _Byte3 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Byte3")); } }
        [XmlElement("b4")]
        public bool Byte4 { get { return _Byte4; } set { _Byte4 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Byte4")); } }
        [XmlElement("b5")]
        public bool InRegionJPN { get { return _InRegionJPN; } set { _InRegionJPN = value; InvokePropertyChanged(new PropertyChangedEventArgs("InRegionJPN")); } }
        [XmlElement("b6")]
        public bool InRegionEUUS { get { return _InRegionEUUS; } set { _InRegionEUUS = value; InvokePropertyChanged(new PropertyChangedEventArgs("InRegionEUUS")); } }

        [XmlElement("source")]
        public SoundSource SoundSource { get; set; }
        [XmlElement("mix")]
        public SoundMixType SoundMixType { get; set; }
        [XmlElement("icon")]
        public uint IconID { get; set; }
        [XmlElement("back")]
        public SoundTestBackImageBehavior SoundTestBackImageBehavior { get; set; }
        public List<uint> AssociatedFightersIDs { get; set; }

        [XmlElement("storder")]
        public int SoundTestOrder { get; set; }
        [XmlElement("scorder")]
        public int StageCreationOrder { get; set; }
        [XmlElement("scgid")]
        public uint StageCreationGroupID { get; set; }
        [XmlElement("u17")]
        public short Int17 { get { return _Int17; } set { _Int17 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Int17")); } }

        [XmlElement("t1")]
        public string Title { get { return _Title; } set { _Title = value; InvokePropertyChanged(new PropertyChangedEventArgs("Title")); } }
        [XmlElement("t2")]
        public string SoundTestTitle { get { return _Title2; } set { _Title2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("SoundTestTitle")); } }
        [XmlElement("d1")]
        public string Description { get { return _Description; } set { _Description = value; InvokePropertyChanged(new PropertyChangedEventArgs("Description")); } }
        [XmlElement("d2")]
        public string Description2 { get { return _Description2; } set { _Description2 = value; InvokePropertyChanged(new PropertyChangedEventArgs("Description2")); } }
        [XmlElement("so")]
        public string Source { get { return _Source; } set { _Source = value; InvokePropertyChanged(new PropertyChangedEventArgs("Source")); } }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public string FullSoundID { get { return "SOUND" + SoundID; } }
        [XmlIgnore]
        public string IconName { get { if (IconID == 0xffffffff) return string.Empty; return IconsDB.Icons[IconID]; } }
        [XmlIgnore]
        public string StageCreationGroupName { get { if (StageCreationGroupID == 0xffffffff) return string.Empty; return IconsDB.Icons[StageCreationGroupID]; } }
        [XmlIgnore]
        public List<string> AssociatedFightersName { get { List<string> output = new List<string>(); foreach (uint fighterId in AssociatedFightersIDs) output.Add(CharsDB.Chars[fighterId]); return output; } }
        [XmlIgnore]
        public string ListTitle { get { return SoundID + " - " + Title; } }
        [XmlIgnore]
        public string ListValue { get { return SoundID; } }
        #endregion

        public SoundEntry(SoundEntryCollection soundEntryCollection)
        {
            SoundEntryCollection = soundEntryCollection;
            BGMFiles = new List<SoundEntryBGM>();
            AssociatedFightersIDs = new List<uint>();
        }

        public SoundEntry()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return ListTitle;
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
