using Sm4shMusic.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "sdsse")]
    public class SoundDBStageSoundEntry : ISm4shListItem, ICloneable
    {
        #region Properties
        [XmlElement("sid")]
        public int SoundID { get; set; }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public SoundEntry SoundEntry { get { return SoundEntryCollection.SoundEntriesPerID.ContainsKey(SoundID) ? SoundEntryCollection.SoundEntriesPerID[SoundID] : null; } }
        [XmlIgnore]
        public string ListTitle { get { return SoundEntry != null ? SoundEntry.ListTitle : SoundID + " - " + Strings.DEFAULT_SENTRY_TITLE; } }
        [XmlIgnore]
        public string ListValue { get { return SoundID.ToString(); } }
        #endregion

        public SoundDBStageSoundEntry(SoundEntryCollection soundEntryCollection, int soundID)
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
