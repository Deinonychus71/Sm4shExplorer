using Sm4shFileExplorer.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "seb")]
    public class SoundEntryBGM : ICloneable
    {
        #region Properties
        [XmlElement("bid")]
        public int BGMID { get; set; }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public SoundEntry SoundEntry { get; set; }
        [XmlIgnore]
        public BGMEntry BGMEntry { get { return GetBGMEntry(); } }
        #endregion

        public SoundEntryBGM(SoundEntryCollection soundEntryCollection, SoundEntry sEntry, int bgmID)
        {
            SoundEntryCollection = soundEntryCollection;
            BGMID = bgmID;
            SoundEntry = sEntry;
        }

        public SoundEntryBGM()
        {
        }

        private BGMEntry GetBGMEntry()
        {
            if (SoundEntryCollection.SoundEntriesBGMsPerID.ContainsKey(BGMID))
                return SoundEntryCollection.SoundEntriesBGMsPerID[BGMID];
            BGMEntry newEntry = SoundEntryCollection.SoundEntriesBGMs.First();
            LogHelper.Error(string.Format("BGM ID {0} was not found for Sound '{1}', reverting to '{2}'. Please check this Sound ID and fix it if needed.", BGMID, SoundEntry.ListTitle, newEntry.BGMTitle));
            BGMID = newEntry.BGMID;
            return GetBGMEntry();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
