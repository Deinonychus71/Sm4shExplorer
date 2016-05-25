﻿using Sm4shFileExplorer;
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
        private const bool INCLUDE_EMPTY_SOUND = false;
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
                    int nbrStages = ReadNbrEntries(b);

                    //Offset to second table
                    int offsetUnknownSecondTable = HEADER_LEN + 0x5 + (nbrStages * 0xd2);
                    b.BaseStream.Position = offsetUnknownSecondTable;
                    int nbrUnknownSecondTableBlocs = ReadNbrEntries(b);
                    b.BaseStream.Position = offsetUnknownSecondTable;
                    _SecondBloc = b.ReadBytes((int)(0x5 + (nbrUnknownSecondTableBlocs * 0xa)));

                    //Offset to variable/songid table
                    int offsetSongTable = offsetUnknownSecondTable + 0x5 + (nbrUnknownSecondTableBlocs * 0xa);
                    b.BaseStream.Position = offsetSongTable;
                    int nbrSoundEntries = ReadNbrEntries(b);

                    //Retrieve all sounds
                    for (int i = 0; i < nbrSoundEntries; i++)
                    {
                        int index = ReadInt32(b);
                        SoundEntry sEntry = ParseSoundEntry(b, index);
                        if (sEntry != null && (INCLUDE_EMPTY_SOUND || sEntry.BGMFiles.Count == 0 || sEntry.BGMFiles[0].BGMID != 0x450))
                            SoundEntryCollection.SoundEntries.Add(sEntry);
                    }

                    //Retrieve info per stage
                    for (int i = 0; i < nbrStages; i++)
                    {
                        b.BaseStream.Position = HEADER_LEN + 0x5 + (i * 0xd2);
                        int stageId = ReadInt32(b);
                        int nbrSounds = ReadInt32(b);

                        List<SoundDBStageSoundEntry> stageSoundEntries = new List<SoundDBStageSoundEntry>();
                        for (int j = 0; j < nbrSounds; j++)
                        {
                            int sEntryIndex = ReadInt32(b);
                            stageSoundEntries.Add(new SoundDBStageSoundEntry(SoundEntryCollection, sEntryIndex));
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
                    WriteNbrEntries(w, SoundEntryCollection.SoundDBStages.Count);

                    foreach (SoundDBStage sDBStage in SoundEntryCollection.SoundDBStages)
                    {
                        WriteInt32BigEndian(w, sDBStage.SoundDBStageID);
                        WriteInt32BigEndian(w, sDBStage.SoundEntries.Count);

                        for (int i = 0; i < 40; i++)
                        {
                            if (sDBStage.SoundEntries.Count > i)
                                WriteInt32BigEndian(w, sDBStage.SoundEntries[i].SoundEntry.SoundID);
                            else
                                WriteInt32BigEndian(w, -1);
                        }
                    }

                    //Second Block
                    w.Write(_SecondBloc);

                    //Third bloc
                    int lastId = SoundEntryCollection.SoundEntries[SoundEntryCollection.SoundEntries.Count - 1].SoundID;
                    WriteNbrEntries(w, lastId + 1);
                    for(int i = 0; i <= lastId; i++)
                    {
                        if (SoundEntryCollection.SoundEntriesPerID.ContainsKey(i))
                            WriteSoundEntry(w, SoundEntryCollection.SoundEntriesPerID[i]);
                        else
                            WriteDummySoundEntry(w, i);
                    }
                    //foreach (SoundEntry sEntry in SoundEntryCollection.SoundEntries)
                    //    WriteSoundEntry(w, sEntry);
                }

                foreach (string savePath in savePaths)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, stream.ToArray());
                }
            }
        }

        #region Specific Data
        private SoundEntry ParseSoundEntry(BinaryReader b, int id)
        {
            SoundEntry sEntry = new SoundEntry(SoundEntryCollection);

            //Sound Index
            sEntry.SoundID = id; //Important, has apparently something to do with unlock conditions.

            //Sound BGM (up to 5)
            for (int i = 0; i < 5; i++)
            {
                int bgmID = ReadInt32(b);
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
            sEntry.IconID = ReadInt32(b);
            sEntry.SoundTestBackImageBehavior = ReadSoundTestBackImageBehavior(b);

            int nbrAssociatedCharacters = ReadInt32(b);
            for (int i = 0; i < 8; i++)
            {
                int fighterID = ReadInt32(b);
                if (fighterID != -1)
                    sEntry.AssociatedFightersIDs.Add(fighterID);
            }

            sEntry.OriginalSoundLabel = ReadString(b);

            sEntry.SoundTestOrder = ReadOrder(b);
            sEntry.StageCreationOrder = ReadOrder(b);
            sEntry.StageCreationGroupID = ReadInt32(b);

            sEntry.Int17 = ReadInt16(b);

            return sEntry;
        }

        private void WriteDummySoundEntry(BinaryWriter w, int id)
        {
            SoundEntry sEntry = new SoundEntry(SoundEntryCollection);
            sEntry.SoundID = id;
            sEntry.BGMFiles.Add(new SoundEntryBGM(SoundEntryCollection, sEntry, 0x450));
            sEntry.SoundSource = SoundSource.CoreGameSound;
            sEntry.SoundMixType = SoundMixType.Original;
            sEntry.IconID = -1;
            sEntry.StageCreationGroupID = -1;
            sEntry.SoundTestOrder = 999;
            sEntry.StageCreationOrder = 999;
            sEntry.SoundTestBackImageBehavior = SoundTestBackImageBehavior.NULL;

            WriteSoundEntry(w, sEntry);
        }

        private void WriteSoundEntry(BinaryWriter w, SoundEntry sEntry)
        {
            WriteInt32BigEndian(w, sEntry.SoundID);

            for (int i = 0; i < 5; i++)
            {
                if (sEntry.BGMFiles.Count > i)
                    WriteInt32BigEndian(w, sEntry.BGMFiles[i].BGMID);
                else
                    WriteInt32BigEndian(w, 0);
            }

            WriteBool(w, sEntry.InSoundTest);
            WriteBool(w, sEntry.Byte2);
            WriteBool(w, sEntry.Byte3);
            WriteBool(w, sEntry.Byte4);
            WriteBool(w, sEntry.InRegionJPN);
            WriteBool(w, sEntry.InRegionEUUS);

            WriteInt32BigEndian(w, (int)sEntry.SoundSource);
            WriteInt32BigEndian(w, (int)sEntry.SoundMixType);
            WriteInt32BigEndian(w, sEntry.IconID);
            WriteInt32BigEndian(w, (int)sEntry.SoundTestBackImageBehavior);

            WriteInt32BigEndian(w, sEntry.AssociatedFightersIDs.Count);
            for (int i = 0; i < 8; i++)
            {
                if (sEntry.AssociatedFightersIDs.Count > i)
                    WriteInt32BigEndian(w, sEntry.AssociatedFightersIDs[i]);
                else
                    WriteInt32BigEndian(w, -1);
            }

            WriteString(w, sEntry.SoundLabel);

            WriteInt32BigEndian(w, sEntry.SoundTestOrder);
            WriteInt32BigEndian(w, sEntry.StageCreationOrder);
            WriteInt32BigEndian(w, sEntry.StageCreationGroupID);

            WriteInt16BigEndian(w, sEntry.Int17);
        }

        private SoundMixType ReadMixType(BinaryReader b)
        {
            int value = ReadInt32(b);
            if (value > 3)
                throw new Exception("Error, this value is not SoundMixType: " + value);
            return (SoundMixType)value;
        }

        private SoundSource ReadSourceSound(BinaryReader b)
        {
            int value = ReadInt32(b);
            if (value > 3)
                throw new Exception("Error, this value is not SoundSource: " + value);
            return (SoundSource)value;
        }

        private SoundTestBackImageBehavior ReadSoundTestBackImageBehavior(BinaryReader b)
        {
            int value = ReadInt32(b);
            if (value > 4)
                throw new Exception("Error, this value is not SoundTestBackImageBehavior: " + value);
            return (SoundTestBackImageBehavior)value;
        }
        #endregion

        #region Generic Data
        private int ReadNbrEntries(BinaryReader b)
        {
            if (b.ReadByte() != 0x20)
                throw new Exception("Error while reading nbr entries in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }

        private void WriteNbrEntries(BinaryWriter w, int input)
        {
            w.Write((byte)0x20);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }

        /*private uint ReadUInt32(BinaryReader b)
        {
            int controlByte = b.ReadByte();
            if (controlByte != 0x5 && controlByte != 0x6)
                throw new Exception("Error while reading 0x5 UInt32 in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }*/

        /*private void WriteUInt32BigEndian(BinaryWriter w, uint input)
        {
            w.Write((byte)0x5);
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            w.Write(value);
        }*/

        private int ReadInt32(BinaryReader b)
        {
            int controlByte = b.ReadByte();
            if (controlByte != 0x5 && controlByte != 0x6)
                throw new Exception("Error while reading 0x5/0x6 Int32 in 'ui_sound_db.bin'");
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }

        private int ReadOrder(BinaryReader b)
        {
            if (b.ReadByte() != 0x6) //Order?
                throw new Exception("Error while reading 0x6 Order in 'ui_sound_db.bin'");
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
