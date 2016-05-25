using Sm4shMusic.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shMusic.Globals
{
    public static class Debug
    {
        public static void WriteDebugSoundMSBTCSV(SoundEntryCollection soundCollection, string pathToSave)
        {
            List<string> soundDBLines = new List<string>();
            soundDBLines.Add("ID,SoundLabel,BGM1,BGM2,BGM3,BGM4,BGM5,InSoundTest,byte2,byte3,byte4,InRegionJPN,InRegionEUUS,SoundSource,SoundMixType,IconID,SoundTestBackImageBehavior,AssociatedFighters,SoundTestOrder,StageCreationOrder,StageCreationGroup,short17,Title,SoundTestTitle,Description1,Description2,Source");
            
            foreach (SoundEntry sEntry in soundCollection.SoundEntries)
            {
                string[] files = new string[5];
                for (int i = 0; i < sEntry.BGMFiles.Count; i++)
                    files[i] = sEntry.BGMFiles[i].BGMEntry.BGMTitle;

                soundDBLines.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26}", sEntry.SoundID, sEntry.SoundLabel, files[0], files[1], files[2], files[3], files[4], sEntry.InSoundTest, sEntry.Byte2, sEntry.Byte3, sEntry.Byte4, sEntry.InRegionJPN, sEntry.InRegionEUUS, sEntry.SoundSource, sEntry.SoundMixType, sEntry.IconName, sEntry.SoundTestBackImageBehavior, string.Join(" | ", sEntry.AssociatedFightersName), sEntry.SoundTestOrder, sEntry.StageCreationOrder, sEntry.StageCreationGroupName, sEntry.Int17, StringToCSVCell(sEntry.Title), StringToCSVCell(sEntry.SoundTestTitle), StringToCSVCell(sEntry.Description), StringToCSVCell(sEntry.Description2), StringToCSVCell(sEntry.Source)));
            }
            
            File.WriteAllLines(pathToSave, soundDBLines);
        }

        public static void WriteDebugBGMEntriesCSV(SoundEntryCollection soundCollection, string pathToSave)
        {
            List<string> bgmsLines = new List<string>();
            bgmsLines.Add("BGMID,Title,unk1,unk2,unk3,unk4,MenuCheckPoint1,MenuCheckPoint2,MenuCheckPoint3,MenuCheckPoint4,LoopStartTime,LoopEndTime,LoopStartSample,LoopEndSample,StreamTotalDuration,StreamTotalSamples");
            
            foreach (BGMEntry eBGM in soundCollection.SoundEntriesBGMs)
                bgmsLines.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", eBGM.BGMID, eBGM.BGMTitle, eBGM.BGMUnk1, eBGM.BGMUnk2, eBGM.BGMUnk3, eBGM.BGMUnk4, eBGM.MenuCheckPoint1, eBGM.MenuCheckPoint2, eBGM.MenuCheckPoint3, eBGM.MenuCheckPoint4, eBGM.LoopStartTime, eBGM.LoopEndTime, eBGM.LoopStartSample, eBGM.LoopEndSample, eBGM.StreamTotalDuration, eBGM.StreamTotalSamples));

            File.WriteAllLines(pathToSave, bgmsLines);
        }

        public static void WriteDebugMyMusicCSV(SoundEntryCollection soundCollection, string pathToSave)
        {
            List<string> myMusicLines = new List<string>();
            myMusicLines.Add("StageMyMusicID,StageSoundDBID,StageName,Index,BGMID,BGMTitle,SubIndex,Rarity,unk3,unk4,PlayDelay,unk6,unk7,unk8,unk9");

            foreach (MyMusicStage myMusicStage in soundCollection.MyMusicStages)
            {
                foreach (MyMusicStageBGM musicStageBGM in myMusicStage.BGMs)
                {
                    myMusicLines.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", myMusicStage.MyMusicStageID, myMusicStage.BGMStage.BGMDBID, myMusicStage.BGMStage.Label, musicStageBGM.Index, musicStageBGM.BGMEntry.BGMID, musicStageBGM.BGMEntry.BGMTitle, musicStageBGM.SubIndex, musicStageBGM.Rarity, musicStageBGM.Unk3, musicStageBGM.Unk4, musicStageBGM.PlayDelay, musicStageBGM.Unk6, musicStageBGM.Unk7, musicStageBGM.Unk8, musicStageBGM.Unk9));
                }
            }

            File.WriteAllLines(pathToSave, myMusicLines);
        }


        private static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
