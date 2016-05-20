using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shSound.Objects
{
    [XmlType(TypeName = "seb")]
    public class SoundEntryBGM : ICloneable
    {
        #region Properties
        [XmlElement("bid")]
        public uint BGMID { get; set; }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        [XmlIgnore]
        public BGMEntry BGMEntry { get { return SoundEntryCollection.SoundEntriesBGMsPerID[BGMID]; } }
        #endregion

        public SoundEntryBGM(SoundEntryCollection soundEntryCollection, uint bgmID)
        {
            SoundEntryCollection = soundEntryCollection;
            BGMID = bgmID;
        }

        public SoundEntryBGM()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
