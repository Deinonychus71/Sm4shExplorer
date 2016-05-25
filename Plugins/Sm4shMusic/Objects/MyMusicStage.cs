using Sm4shMusic.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "mms")]
    public class MyMusicStage : ISm4shListItem
    {
        #region Attributes
        private int _MyMusicStageID;
        private BGMStage _BGMStage;
        #endregion

        #region Properties
        [XmlElement("mmsid")] 
        public int MyMusicStageID { get { return _MyMusicStageID; } set { _MyMusicStageID = value; _BGMStage = BGMStageDB.BGMMyMusics[MyMusicStageID]; } }
        public List<MyMusicStageBGM> BGMs { get; set; }

        [XmlIgnore]
        public string ListTitle { get { return _BGMStage.BGMMyMusicID + " - " + _BGMStage.Label; } }
        [XmlIgnore]
        public string ListValue { get { return _BGMStage.BGMMyMusicID.ToString(); } }
        [XmlIgnore]
        public BGMStage BGMStage { get { return _BGMStage; } }
        #endregion

        public MyMusicStage(int myMusicStageID)
        {
            MyMusicStageID = myMusicStageID;
            BGMs = new List<MyMusicStageBGM>();
        }

        public MyMusicStage()
        {
        }

        
    }
}
