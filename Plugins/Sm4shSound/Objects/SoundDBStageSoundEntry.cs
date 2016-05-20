using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shSound.Objects
{
    [XmlType(TypeName = "sdsse")]
    public class SoundDBStageSoundEntry : ISm4shListItem, ICloneable
    {
        #region Properties
        [XmlElement("sid")]
        public string SoundID { get; set; }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public SoundEntry SoundEntry { get { return SoundEntryCollection.SoundEntriesPerID[SoundID]; } }
        [XmlIgnore]
        public string ListTitle { get { return SoundEntry.ListTitle; } }
        [XmlIgnore]
        public string ListValue { get { return SoundEntry.ListValue; } }
        #endregion

        public SoundDBStageSoundEntry(SoundEntryCollection soundEntryCollection, string soundID)
        {
            SoundEntryCollection = soundEntryCollection;
            SoundID = soundID;
        }

        public SoundDBStageSoundEntry()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
