using Sm4shMusic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.DB
{
    public class BGMStage
    {
        private uint _BGMMyMusicID;
        private uint? _BGMDBID;
        private string _Label;

        public uint BGMMyMusicID { get { return _BGMMyMusicID; } set { _BGMMyMusicID = value; } }
        public uint? BGMDBID { get { return _BGMDBID; } set { _BGMDBID = value; } }
        public string Label { get { return _Label; } set { _Label = value; } }

        public BGMStage(string label, uint myMusicID, uint? dbID)
        {
            _Label = label;
            _BGMMyMusicID = myMusicID;
            _BGMDBID = dbID;
        }

        public BGMStage()
        {
        }
    }
}
