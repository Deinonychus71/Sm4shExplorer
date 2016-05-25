using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shMusic.DB;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Sm4shMusic.Objects
{
    public class PropertyFile
    {
        public SoundEntryCollection SoundEntryCollection { get; set; }
        private string _Path;
        private const int HEADER_LEN = 0x8;
        private byte[] _Header;

        public PropertyFile(Sm4shProject projectManager, SoundEntryCollection sEntryCollection, string path)
        {
            string propertyFile = projectManager.ExtractResource(path, PathHelper.FolderTemp);
            SoundEntryCollection = sEntryCollection;
            _Path = path;

            using (FileStream fileStream = File.Open(propertyFile, FileMode.Open))
            {
                using (BinaryReader b = new BinaryReader(fileStream))
                {
                    if (new string(b.ReadChars(3)) != "MPB")
                        throw new Exception(string.Format("Can't load '{0}', the file is either compressed or its not a MPB file", propertyFile));

                    //Keep header
                    b.BaseStream.Position = 0;
                    _Header = b.ReadBytes(HEADER_LEN);

                    //Nbr BGM
                    b.BaseStream.Position = HEADER_LEN;
                    uint nbrBgm = b.ReadUInt32();

                    //Songtable
                    uint songTableOffset = HEADER_LEN + 0x4 + (nbrBgm * 0x4);
                    uint[] songIds = new uint[nbrBgm];
                    b.BaseStream.Position = HEADER_LEN + 0x4;
                    for (int i = 0; i < nbrBgm; i++)
                        songIds[i] = b.ReadUInt32();

                    //Parsing
                    for (int i = 0; i < nbrBgm; i++)
                    {
                        if (songIds[i] != 0xffffffff)
                        {
                            b.BaseStream.Position = songTableOffset + songIds[i];
                            BGMEntry sEntry = ParseProperty(b, i);
                            SoundEntryCollection.SoundEntriesBGMs.Add(sEntry);
                        }
                    }
                }
            }
        }

        private BGMEntry ParseProperty(BinaryReader b, int songId)
        {
            BGMEntry bEntry = new BGMEntry(songId);

            bEntry.BGMTitle = new string(b.ReadChars(0x3c));
            if (bEntry.BGMTitle.StartsWith("\0"))
                bEntry.BGMTitle = BGMFilesDB.BGMs[songId];
            else
                bEntry.BGMTitle = bEntry.BGMTitle.TrimEnd('\0');

            bEntry.BGMUnk1 = b.ReadInt32();
            bEntry.BGMUnk2 = b.ReadInt32();
            bEntry.BGMUnk3 = b.ReadInt32();
            bEntry.BGMUnk4 = b.ReadInt32();
            bEntry.MenuCheckPoint1 = b.ReadInt32();
            bEntry.MenuCheckPoint2 = b.ReadInt32();
            bEntry.MenuCheckPoint3 = b.ReadInt32();
            bEntry.MenuCheckPoint4 = b.ReadInt32();
            bEntry.LoopStartTime = b.ReadInt32();
            bEntry.LoopEndTime = b.ReadInt32();
            bEntry.LoopStartSample = b.ReadInt32();
            bEntry.LoopEndSample = b.ReadInt32();
            bEntry.StreamTotalDuration = b.ReadInt32();
            bEntry.StreamTotalSamples = b.ReadInt32();

            return bEntry;
        }

        public void BuildFile()
        {
            string savePath = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH) + _Path.Replace('/', Path.DirectorySeparatorChar);
            int nbrBGM = SoundEntryCollection.SoundEntriesBGMs.Max(p => p.BGMID) + 1;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    w.Write(_Header);
                    w.Write(nbrBGM);
                    uint indexOffset = 0x00000000;
                    for (int i = 0; i < nbrBGM; i++)
                    {
                        if (!SoundEntryCollection.SoundEntriesBGMsPerID.ContainsKey(i))
                            w.Write(0xffffffff);
                        else
                        {
                            w.Write(indexOffset);
                            indexOffset += 0x74;
                        }
                    }

                    foreach (BGMEntry bEntry in SoundEntryCollection.SoundEntriesBGMsPerID.Values)
                    {
                        WriteBGMEntry(bEntry, w);
                    }
                }
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                File.WriteAllBytes(savePath, stream.ToArray());
            }
 
        }

        private void WriteBGMEntry(BGMEntry bEntry, BinaryWriter writerPackage)
        {
            int titleSize = bEntry.BGMTitle.Length;
            if (titleSize > 0x3c)
                throw new Exception(string.Format("Title of BGMEntry too long! : {0}", bEntry.BGMTitle));
            writerPackage.Write(Encoding.ASCII.GetBytes(bEntry.BGMTitle));
            while (titleSize < 0x3c)
            {
                writerPackage.Write((byte)0x00);
                titleSize++;
            }
            writerPackage.Write(bEntry.BGMUnk1);
            writerPackage.Write(bEntry.BGMUnk2);
            writerPackage.Write(bEntry.BGMUnk3);
            writerPackage.Write(bEntry.BGMUnk4);
            writerPackage.Write(bEntry.MenuCheckPoint1);
            writerPackage.Write(bEntry.MenuCheckPoint2);
            writerPackage.Write(bEntry.MenuCheckPoint3);
            writerPackage.Write(bEntry.MenuCheckPoint4);
            writerPackage.Write(bEntry.LoopStartTime);
            writerPackage.Write(bEntry.LoopEndTime);
            writerPackage.Write(bEntry.LoopStartSample);
            writerPackage.Write(bEntry.LoopEndSample);
            writerPackage.Write(bEntry.StreamTotalDuration);
            writerPackage.Write(bEntry.StreamTotalSamples);
        }
    }
}
