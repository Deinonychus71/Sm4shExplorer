using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "sds")]
    public class SoundDBStage
    {
        #region Properties
        public List<SoundDBStageSoundEntry> SoundEntries { get; set; }
        [XmlElement("stid")]
        public uint SoundDBStageID { get; set; }

        [XmlIgnore]
        public SoundEntryCollection SoundEntryCollection { get; set; }
        #endregion

        public SoundDBStage(SoundEntryCollection soundEntryCollection, uint soundDBStageID)
        {
            SoundEntryCollection = soundEntryCollection;
            SoundDBStageID = soundDBStageID;
            SoundEntries = new List<SoundDBStageSoundEntry>();
        }

        public SoundDBStage()
        {
        }
    }
}
