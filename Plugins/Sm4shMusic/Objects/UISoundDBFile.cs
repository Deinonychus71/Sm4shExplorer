using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sm4shMusic.Objects
{
    public class UISoundDBFile
    {
        private const int HEADER_LEN = 0x8;
        private const bool INCLUDE_EMPTY_SOUND = true;
        private byte[] _Header;
        private byte[] _SecondBloc;
        public SoundEntryCollection SoundEntryCollection { get; set; }
        private string _Path;
        private Sm4shProject _Project;

        public UISoundDBFile(Sm4shProject projectManager, SoundEntryCollection sEntryCollection, string path)
        {
            string uiSoundDBFile = projectManager.ExtractResource(path, PathHelper.FolderTemp);
            SoundEntryCollection = sEntryCollection;
            _Path = path;
            _Project = projectManager;

            using (FileStream fileStream = File.Open(uiSoundDBFile, FileMode.Open))
            {
                using (BinaryReader b = new BinaryReader(fileStream))
                {
                    byte[] header = b.ReadBytes(2);
                    if (header[0] != 0xff || header[1] != 0xff)
                        throw new Exception(string.Format("Can't load '{0}', the file doesn't appear to be 'ui_sound_db.bin'.", uiSoundDBFile));

                    //Keep header
                    b.BaseStream.Position = 0;
                    _Header = b.ReadBytes(HEADER_LEN);

                    //Number of elements
                    b.BaseStream.Position = HEADER_LEN;
                    uint nbrStages = ReadNbrEntries(b);

                    //Offset to second table
                    uint offsetUnknownSecondTable = HEADER_LEN + 0x5 + (nbrStages * 0xd2);
                    b.BaseStream.Position = offsetUnknownSecondTable;
                    uint nbrUnknownSecondTableBlocs = ReadNbrEntries(b);
                    b.BaseStream.Position = offsetUnknownSecondTable;
                    _SecondBloc = b.ReadBytes((int)(0x5 + (nbrUnknownSecondTableBlocs * 0xa)));

                    //Offset to variable/songid table
                    uint offsetSongTable = offsetUnknownSecondTable + 0x5 + (nbrUnknownSecondTableBlocs * 0xa);
                    b.BaseStream.Position = offsetSongTable;
                    uint nbrSoundEntries = ReadNbrEntries(b);

                    //Retrieve all sounds
                    Dictionary<uint, SoundEntry> soundEntriesIndex = new Dictionary<uint, SoundEntry>();
                    for (int i = 0; i < nbrSoundEntries; i++)
                    {
                        uint index = ReadUInt32(b);
                        SoundEntry sEntry = ParseSoundEntry(b, index);
                        if (sEntry != null && (INCLUDE_EMPTY_SOUND || sEntry.BGMFiles.Count == 0 || sEntry.BGMFiles[0].BGMID != 0x450))
                        {
                            SoundEntryCollection.SoundEntries.Add(sEntry);
                            soundEntriesIndex.Add(index, sEntry);
                        }
                    }

                    //Retrieve info per stage
                    for (int i = 0; i < nbrStages; i++)
                    {
                        b.BaseStream.Position = HEADER_LEN + 0x5 + (i * 0xd2);
                        uint stageId = ReadUInt32(b);
                        uint nbrSounds = ReadUInt32(b);

                        List<SoundDBStageSoundEntry> stageSoundEntries = new List<SoundDBStageSoundEntry>();
                        for (int j = 0; j < nbrSounds; j++)
                        {
                            uint sEntryIndex = ReadUInt32(b);
                            stageSoundEntries.Add(new SoundDBStageSoundEntry(SoundEntryCollection, soundEntriesIndex[sEntryIndex].SoundID));
                        }
                        SoundDBStage soundDBStage = new SoundDBStage(SoundEntryCollection, stageId);
                        soundDBStage.SoundEntries = stageSoundEntries;
                        SoundEntryCollection.SoundDBStages.Add(soundDBStage);
                    }
                }
            }
        }

        public void BuildFile()
        {
            List<string> savePaths = new List<string>();
            foreach (ResourceCollection resCol in _Project.ResourceDataCollection)
                savePaths.Add(PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH) + _Path.Replace('/', Path.DirectorySeparatorChar).Replace("data", resCol.PartitionName));

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    w.Write(_Header);

                    //Stages
                    //Nbr Stages
                    WriteNbrEntries(w, (uint)SoundEntryCollection.SoundDBStages.Count);

                    foreach (SoundDBStage sDBStage in SoundEntryCollection.SoundDBStages)
                    {
                        WriteUInt32BigEndian(w, sDBStage.SoundDBStageID);
                        WriteUInt32BigEndian(w, (uint)sDBStage.SoundEntries.Count);

                        for (int i = 0; i < 40; i++)
                        {
                            if (sDBStage.SoundEntries.Count > i)
                                WriteUInt32BigEndian(w, sDBStage.SoundEntries[i].SoundEntry.Index);
                            else
                                WriteUInt32BigEndian(w, (uint)0xffffffff);
                        }
                    }

                    //Second Block
                    w.Write(_SecondBloc);

                    //Third bloc
                    WriteNbrEntries(w, (uint)SoundEntryCollection.SoundEntries.Count);
                    foreach (SoundEntry sEntry in SoundEntryCollection.SoundEntries)
                        WriteSoundEntry(w, sEntry);
                }

                foreach (string savePath in savePaths)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, stream.ToArray());
                }
            }
        }

        #region Specific Data
        private SoundEntry ParseSoundEntry(BinaryReader b, uint index)
        {
            SoundEntry sEntry = new SoundEntry(SoundEntryCollection);

            //Sound Index
            sEntry.Index = index; //Important, has apparently something to do with unlock conditions.

            //Sound BGM (up to 5)
            for (int i = 0; i < 5; i++)
            {
                uint bgmID = ReadUInt32(b);
                if (bgmID != 0x0)
                    sEntry.BGMFiles.Add(new SoundEntryBGM(SoundEntryCollection, sEntry, bgmID));
            }

            //Flags?
            sEntry.InSoundTest = ReadBool(b);
            sEntry.Byte2 = ReadBool(b);
            sEntry.Byte3 = ReadBool(b);
            sEntry.Byte4 = ReadBool(b);
            sEntry.InRegionJPN = ReadBool(b);
            sEntry.InRegionEUUS = ReadBool(b);

            //Properties
            sEntry.SoundSource = ReadSourceSound(b);
            sEntry.SoundMixType = ReadMixType(b);
            sEntry.IconID = ReadUInt32(b);
            sEntry.SoundTestBackImageBehavior = ReadSoundTestBackImageBehavior(b);

            int nbrAssociatedCharacters = ReadInt32(b);
            for (int i = 0; i < 8; i++)
            {
                uint fighterID = ReadUInt32(b);
                if (fighterID != 0xffffffff)
                    sEntry.AssociatedFightersIDs.Add(fighterID);
            }

            sEntry.SoundID = ReadString(b).Replace("SOUND", string.Empty);

            sEntry.SoundTestOrder = ReadInt32(b);
            sEntry.StageCreationOrder = ReadInt32(b);
            sEntry.StageCreationGroupID = ReadUInt32(b);

            sEntry.Int17 = ReadInt16(b);

            return sEntry;
        }

        private void WriteSoundEntry(BinaryWriter w, SoundEntry sEntry)
        {
            WriteUInt32BigEndian(w, sEntry.Index);

            for (int i = 0; i < 5; i++)
            {
                if (sEntry.BGMFiles.Count > i)
                    WriteUInt32BigEndian(w, sEntry.BGMFiles[i].BGMID);
                else
                    WriteUInt32BigEndian(w, 0);
            }

            WriteBool(w, sEntry.InSoundTest);
            WriteBool(w, sEntry.Byte2);
            WriteBool(w, sEntry.Byte3);
            WriteBool(w, sEntry.Byte4);
            WriteBool(w, sEntry.InRegionJPN);
            WriteBool(w, sEntry.InRegionEUUS);

            WriteUInt32BigEndian(w, (uint)sEntry.SoundSource);
            WriteUInt32BigEndian(w, (uint)sEntry.SoundMixType);
            WriteUInt32BigEndian(w, sEntry.IconID);
            WriteUInt32BigEndian(w, (uint)sEntry.SoundTestBackImageBehavior);

            WriteInt32BigEndian(w, sEntry.AssociatedFightersIDs.Count);
            for (int i = 0; i < 8; i++)
            {
                if (sEntry.AssociatedFightersIDs.Count > i)
                    WriteUInt32BigEndian(w, sEntry.AssociatedFightersIDs[i]);
                else
                    WriteUInt32BigEndian(w, 0xffffffff);
            }

            WriteString(w, sEntry.FullSoundID);

            WriteInt32BigEndian(w, sEntry.SoundTestOrder);
            WriteInt32BigEndian(w, sEntry.StageCreationOrder);
            WriteUInt32BigEndian(w, sEntry.StageCreationGroupID);

            WriteInt16BigEndian(w, sEntry.Int17);
        }

        private SoundMixType ReadMixType(BinaryReader b)
        {
            uint value = ReadUInt32(b);
            if (value > 3)
                throw new Exception("Error, this value is not SoundMixType: " + value);
            return (SoundMixType)value;
        }

        private SoundSource ReadSourceSound(BinaryReader b)
        {
            uint value = ReadUInt32(b);
            if (value > 3)
                throw new Exception("Error, this value is not SoundSource: " + value);
            return (SoundSource)value;
        }

        private SoundTestBackImageBehavior ReadSoundTestBackImageBehavior(BinaryReader b)
        {
            uint value = ReadUInt32(b);
            if (value > 4)
                throw new Exception("Error, this value is not SoundTestBackImageBehavior: " + value);
            return (SoundTestBackImageBehavior)value;
        }
        #endregion

        #region Generic Data
        private uint ReadNbrEntries(BinaryReader b)
        {
            if (b.ReadByte() != 0x20)
                throw new Exception("Error while reading nbr entries in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }

        private void WriteNbrEntries(BinaryWriter w, uint input)
        {
            w.Write((byte)0x20);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }

        private uint ReadUInt32(BinaryReader b)
        {
            if (b.ReadByte() != 0x5)
                throw new Exception("Error while reading 0x5 UInt32 in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }

        private void WriteUInt32BigEndian(BinaryWriter w, uint input)
        {
            w.Write((byte)0x5);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }

        private int ReadInt32(BinaryReader b)
        {
            if (b.ReadByte() != 0x6)
                throw new Exception("Error while reading 0x6 Int32 in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }

        private void WriteInt32BigEndian(BinaryWriter w, int input)
        {
            w.Write((byte)0x6);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }

        private string ReadString(BinaryReader b)
        {
            if (b.ReadByte() != 0x8)
                throw new Exception("Error while reading 0x8 String in 'ui_sound_db.bin'");
            byte[] valueNbr = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(valueNbr);
            int strLength = BitConverter.ToInt32(valueNbr, 0);
            return new string(b.ReadChars(strLength));
        }

        private void WriteString(BinaryWriter w, string input)
        {
            w.Write((byte)0x8);
            byte[] value = BitConverter.GetBytes(input.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
            w.Write(Encoding.ASCII.GetBytes(input));
        }

        private short ReadInt16(BinaryReader b)
        {
            if (b.ReadByte() != 0x4)
                throw new Exception("Error while reading 0x4 Int16 in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToInt16(value, 0);
        }

        private void WriteInt16BigEndian(BinaryWriter w, short input)
        {
            w.Write((byte)0x4);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }

        private bool ReadBool(BinaryReader b)
        {
            if (b.ReadByte() != 0x2)
                throw new Exception("Error while reading 0x2 Byte in 'ui_sound_db.bin'");
            byte val = b.ReadByte();
            if(val == 0x0)
                return false;
            else if(val == 0x1)
                return true;
            throw new Exception("Error while reading 0x2 Byte in 'ui_sound_db.bin', value: " + val);
        }

        private void WriteBool(BinaryWriter w, bool input)
        {
            w.Write((byte)0x2);
            if (input)
                w.Write((byte)0x1);
            else
                w.Write((byte)0x0);
        }
        #endregion
    }
}
