using Sm4shFileExplorer.Globals;
using Sm4shMusic.DB;
using Sm4shMusic.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
{
    [XmlType(TypeName = "sec")]
    public class SoundEntryCollection : ICloneable
    {
        private List<SoundEntry> _SoundEntries;
        private List<BGMEntry> _SoundEntriesBGMs;
        private List<MyMusicStage> _MyMusicStages;
        private List<SoundDBStage> _SoundDBStages;

        private Dictionary<int, SoundEntry> _SoundEntriesPerID;
        private SortedDictionary<int, BGMEntry> _SoundEntriesBGMsPerID;
        private SortedDictionary<string, BGMEntry> _SoundEntriesBGMsPerName;
        private Dictionary<int, MyMusicStage> _MyMusicStagesPerID;
        private Dictionary<int, SoundDBStage> _SoundDBStagesPerID;

        public List<SoundEntry> SoundEntries { get { return _SoundEntries; } set { _SoundEntries = value; } }
        public List<BGMEntry> SoundEntriesBGMs { get { return _SoundEntriesBGMs; } set { _SoundEntriesBGMs = value; } }
        public List<MyMusicStage> MyMusicStages { get { return _MyMusicStages; } set { _MyMusicStages = value; } }
        public List<SoundDBStage> SoundDBStages { get { return _SoundDBStages; } set { _SoundDBStages = value; } }
        
        [XmlIgnore]
        public Dictionary<int, SoundEntry> SoundEntriesPerID { get { return _SoundEntriesPerID; } }
        [XmlIgnore]
        public SortedDictionary<int, BGMEntry> SoundEntriesBGMsPerID { get { return _SoundEntriesBGMsPerID; } }
        [XmlIgnore]
        public SortedDictionary<string, BGMEntry> SoundEntriesBGMsPerName { get { return _SoundEntriesBGMsPerName; } }
        
        [XmlIgnore]
        public Dictionary<int, MyMusicStage> MyMusicStagesPerID { get { return _MyMusicStagesPerID; } }
        [XmlIgnore]
        public Dictionary<int, SoundDBStage> SoundDBStagesPerID { get { return _SoundDBStagesPerID; } }

        public SoundEntryCollection()
        {
            _SoundEntries = new List<SoundEntry>();
            _SoundEntriesBGMs = new List<BGMEntry>();
            _MyMusicStages = new List<MyMusicStage>();
            _SoundDBStages = new List<SoundDBStage>();
        }

        public void GenerateStagesDictionaries()
        {
            _MyMusicStagesPerID = new Dictionary<int, MyMusicStage>();
            _SoundDBStagesPerID = new Dictionary<int, SoundDBStage>();

            foreach (MyMusicStage myMusicStage in _MyMusicStages)
                _MyMusicStagesPerID.Add(myMusicStage.MyMusicStageID, myMusicStage);

            foreach (SoundDBStage soundDBStage in _SoundDBStages)
                _SoundDBStagesPerID.Add(soundDBStage.SoundDBStageID, soundDBStage);
        }

        public void GenerateSoundEntriesDictionary()
        {
            _SoundEntriesPerID = new Dictionary<int, SoundEntry>();
            foreach (SoundEntry sEntry in _SoundEntries)
                _SoundEntriesPerID.Add(sEntry.SoundID, sEntry);
        }

        public void GenerateSoundEntriesBGMDictionary()
        {
            _SoundEntriesBGMsPerID = new SortedDictionary<int, BGMEntry>();
            _SoundEntriesBGMsPerName = new SortedDictionary<string, BGMEntry>();
            foreach (BGMEntry sEntryBGM in _SoundEntriesBGMs)
            {
                _SoundEntriesBGMsPerID.Add(sEntryBGM.BGMID, sEntryBGM);
                _SoundEntriesBGMsPerName.Add(sEntryBGM.BGMTitle, sEntryBGM);
            }
        }

        #region Create
        public BGMEntry CreateBGMEntry(string bgmName)
        {
            string file = GetBGMFullPath("snd_bgm_" + bgmName + ".nus3bank");
            VGMStreamReader reader = new VGMStreamReader(file);

            int lastId = GetNewBGMEntryID();
            BGMEntry sEntryBGM = new BGMEntry(lastId);
            sEntryBGM.BGMTitle = bgmName;
            sEntryBGM.BGMUnk1 = 1; // Most common value is 1
            sEntryBGM.BGMUnk2 = -1;
            sEntryBGM.BGMUnk3 = 0x190; //Most common value
            sEntryBGM.BGMUnk4 = -1; //Most common value is null
            sEntryBGM.MenuCheckPoint1 = -1;
            sEntryBGM.MenuCheckPoint2 = -1;
            sEntryBGM.MenuCheckPoint3 = -1;
            sEntryBGM.MenuCheckPoint4 = -1;
            if (reader.FileLoaded)
            {
                sEntryBGM.LoopStartTime = reader.LoopStartMilliseconds;
                sEntryBGM.LoopEndTime = reader.LoopEndMilliseconds;
                sEntryBGM.LoopStartSample = reader.LoopStartSample;
                sEntryBGM.LoopEndSample = reader.LoopEndSample;
                sEntryBGM.StreamTotalDuration = reader.TotalMilliseconds;
                sEntryBGM.StreamTotalSamples = reader.TotalSamples;
                reader.Dispose();
            }
            else
            {
                LogHelper.Warning(string.Format(Strings.WARNING_VGMSTREAM_CREATE_BGMENTRY, file));
            }

            _SoundEntriesBGMs.Add(sEntryBGM);
            _SoundEntriesBGMsPerID.Add(lastId, sEntryBGM);
            _SoundEntriesBGMsPerName.Add(bgmName, sEntryBGM);

            return sEntryBGM;
        }

        public SoundEntry CreateSoundEntry()
        {
            SoundEntry sEntry = new SoundEntry(this);

            sEntry.SoundID = GetNewSoundID();
            sEntry.BGMFiles.Add(new SoundEntryBGM(this, sEntry, 0x450));
            sEntry.InSoundTest = true;
            sEntry.Byte2 = true;
            sEntry.Byte3 = true;
            sEntry.Byte4 = false;
            sEntry.InRegionEUUS = true;
            sEntry.InRegionJPN = true;
            sEntry.SoundSource = SoundSource.CoreGameSound;
            sEntry.SoundMixType = SoundMixType.Original;
            sEntry.IconID = 0x0; //smashbros
            sEntry.SoundTestBackImageBehavior = SoundTestBackImageBehavior.RosterRandom;
            sEntry.SoundTestOrder = 999;
            sEntry.StageCreationOrder = 999;
            sEntry.StageCreationGroupID = 0x0; //smashbros
            sEntry.Int17 = 0x0;

            sEntry.Title = Strings.DEFAULT_SENTRY_TITLE;
            sEntry.SoundTestTitle = Strings.DEFAULT_SENTRY_TITLE2;
            sEntry.Description = string.Empty;
            sEntry.Description2 = string.Empty;
            sEntry.Source = string.Empty;

            _SoundEntries.Add(sEntry);
            _SoundEntries.Sort((x, y) => x.SoundID.CompareTo(y.SoundID));
            _SoundEntriesPerID.Add(sEntry.SoundID, sEntry);

            return sEntry;
        }

        public MyMusicStageBGM CreateMyMusicStageBGM(int sEntryBGMID, ushort index)
        {
            MyMusicStageBGM myMusicStageBGM = new MyMusicStageBGM(this, sEntryBGMID);
            myMusicStageBGM.Index = (short)(index + 1);
            myMusicStageBGM.Rarity = 100;
            myMusicStageBGM.SubIndex = -1;
            myMusicStageBGM.Unk3 = -1;

            return myMusicStageBGM;
        }
        #endregion

        #region Delete
        public void RemoveSoundEntry(int soundID)
        {
            if (_SoundEntriesPerID.ContainsKey(soundID))
            {
                SoundEntry sEntry = _SoundEntriesPerID[soundID];

                //Delete SoundEntry in stages
                foreach (SoundDBStage soundDBStage in _SoundDBStages)
                {
                    SoundDBStageSoundEntry sStageEntry = soundDBStage.SoundEntries.Find(p => p.SoundEntry.SoundID == sEntry.SoundID);
                    if (sStageEntry != null)
                        soundDBStage.SoundEntries.Remove(sStageEntry);
                }
                   
                //Delete in SoundDB
                _SoundEntriesPerID.Remove(soundID);
                _SoundEntries.Remove(sEntry);
            }
        }

        public void RemoveBGMEntry(int bgmID)
        {
            BGMEntry bEntry = _SoundEntriesBGMsPerID[bgmID];
            _SoundEntriesBGMsPerID.Remove(bEntry.BGMID);
            _SoundEntriesBGMsPerName.Remove(bEntry.BGMTitle);
            _SoundEntriesBGMs.Remove(bEntry);
        }
        #endregion

        #region Tools
        public string GetBGMFullPath(string file)
        {
            if (File.Exists(PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_SOUND_BGM) + file))
                return PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_SOUND_BGM) + file;
            if (File.Exists(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM) + file))
                return PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM) + file;
            return null;
        }

        /// <summary>
        /// Will remove from the base any BGM that isnt used by SOUND or MyMusic
        /// </summary>
        public string CleanBGMDatabase(bool remove)
        {
            //List of orphan BGMs
            List<BGMEntry> orphanBGMs = new List<BGMEntry>();

            //Reference all BGMs from SoundDB
            List<int> soundBGMEntriesIDs = new List<int>();
            foreach (SoundEntry sEntry in _SoundEntries)
            {
                foreach (SoundEntryBGM sBGMEntry in sEntry.BGMFiles)
                    soundBGMEntriesIDs.Add(sBGMEntry.BGMID);
            }

            //Reference all BGMS from MyMusic
            List<int> myMusicBGMEntriesIDs = new List<int>();
            foreach (MyMusicStage myMusicStage in _MyMusicStages)
            {
                foreach (MyMusicStageBGM myMusicStageBGM in myMusicStage.BGMs)
                    myMusicBGMEntriesIDs.Add(myMusicStageBGM.BGMID);
            }

            foreach (BGMEntry bEntry in _SoundEntriesBGMs)
            {
                //Test SOUNDDB
                if(soundBGMEntriesIDs.Contains(bEntry.BGMID))
                    continue;

                //Test MyMusic
                if (myMusicBGMEntriesIDs.Contains(bEntry.BGMID))
                    continue;

                //Exclude character sound (TODO, do it dynamically)
                if ((bEntry.BGMTitle.StartsWith("Z") && (bEntry.BGMTitle.Contains("_F_") || bEntry.BGMTitle.Contains("_J_"))) ||
                    bEntry.BGMTitle.StartsWith("TEST") || (bEntry.BGMTitle.StartsWith("SEQ_item")))
                    continue;

                orphanBGMs.Add(bEntry);
            }

            string listOrphan = string.Empty;
            foreach (BGMEntry bEntry in orphanBGMs)
                listOrphan += string.Format("\r\nID : {0}, Name : {1}", bEntry.BGMID, bEntry.BGMTitle);

            if (remove)
                foreach (BGMEntry bEntry in orphanBGMs)
                    RemoveBGMEntry(bEntry.BGMID);

            return string.Format(Strings.DEBUG_LIST_ORPHAN_BGMS, orphanBGMs.Count, listOrphan);
        }

        public int GetNewBGMEntryID()
        {
            for (int i = 1; i > 0; i++)
            {
                if (_SoundEntriesBGMsPerID.ContainsKey(i))
                    continue;
                return i;
            }
            return 0;
        }

        public int GetNewSoundID()
        {
            for (int i = 1; i > 0; i++)
            {
                if (_SoundEntriesPerID.ContainsKey(i))
                    continue;
                return i;
            }
            return 0;
        }

        public object Clone()
        {
            SoundEntryCollection sCollection = new SoundEntryCollection();

            foreach (BGMEntry sEntryBGM in _SoundEntriesBGMs)
            {
                BGMEntry newSEntryBGM = (BGMEntry)sEntryBGM.Clone();
                sCollection._SoundEntriesBGMs.Add(newSEntryBGM);
            }
            sCollection.GenerateSoundEntriesBGMDictionary();

            foreach (SoundEntry sEntry in _SoundEntries)
            {
                SoundEntry newSEntry = (SoundEntry)sEntry.Clone();
                newSEntry.BGMFiles = new List<SoundEntryBGM>();
                foreach (SoundEntryBGM sEntryBGM in sEntry.BGMFiles)
                    newSEntry.BGMFiles.Add(new SoundEntryBGM(sCollection, newSEntry, sEntryBGM.BGMID));
                newSEntry.SoundEntryCollection = sCollection;
                sCollection._SoundEntries.Add(newSEntry);
            }
            sCollection.GenerateSoundEntriesDictionary();

            foreach (MyMusicStage myMusicStage in _MyMusicStages)
            {
                MyMusicStage nMyMusicStage = new MyMusicStage(myMusicStage.MyMusicStageID);
                foreach (MyMusicStageBGM sMusicStageBGM in myMusicStage.BGMs)
                {
                    MyMusicStageBGM nMyMusicStageBGM = (MyMusicStageBGM)sMusicStageBGM.Clone();
                    nMyMusicStageBGM.SoundEntryCollection = sCollection;
                    nMyMusicStage.BGMs.Add(nMyMusicStageBGM);
                }
                sCollection._MyMusicStages.Add(nMyMusicStage);
            }

            foreach (SoundDBStage soundDBStage in _SoundDBStages)
            {
                List<SoundDBStageSoundEntry> sEntries = new List<SoundDBStageSoundEntry>();
                foreach (SoundDBStageSoundEntry sSoundDBStageSoundEntry in soundDBStage.SoundEntries)
                    sEntries.Add(new SoundDBStageSoundEntry(sCollection, sSoundDBStageSoundEntry.SoundID));
                SoundDBStage nSoundDBStage = new SoundDBStage(sCollection, soundDBStage.SoundDBStageID);
                nSoundDBStage.SoundEntries = sEntries;
                sCollection._SoundDBStages.Add(nSoundDBStage);
            }

            sCollection.GenerateStagesDictionaries();

            return sCollection;
        }
        #endregion
    }
}
