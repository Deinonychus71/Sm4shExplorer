using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using Sm4shMusic.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sm4shMusic.Objects
{
    public class SoundMSBTFile
    {
        private const uint HEADER_LEN = 0x20;
        private const uint HEADER_LBL1 = 0x10;
        private const uint HEADER_ATR1 = 0x10;
        private const uint HEADER_TXT2 = 0x10;

        public const string VAR_TITLE = "MsndN_";
        public const string VAR_TITLE2 = "MsndN2_";
        public const string VAR_DESCRIPTION = "MsndW_";
        public const string VAR_DESCRIPTION2 = "MsndO_";
        public const string VAR_SOURCE = "MsndS_";

        public SoundEntryCollection SoundEntryCollection { get; set; }
        private ResourceCollection _ResCol;
        private SortedDictionary<string,MSBTVariable> _VarsMSBT;
        private string _Path;
        private MSBTHeader _Header;
        private MSBTSectionHeader _SectionLBL1;
        private uint _SizeHashTable;
        private MSBTSectionHeader _SectionATR1;
        private MSBTSectionHeader _SectionTXT2;
        private byte[] _ATR1Bloc;

        public SoundMSBTFile(Sm4shProject projectManager, SoundEntryCollection sEntryCollection, string path)
        {
            _ResCol = projectManager.GetResourceCollection(path);
            string soundMSBTFile = projectManager.ExtractResource(path, PathHelper.FolderTemp);
            SoundEntryCollection = sEntryCollection;
            _VarsMSBT = new SortedDictionary<string, MSBTVariable>();
            _Path = path;

            using (FileStream fileStream = File.Open(soundMSBTFile, FileMode.Open))
            {
                using (BinaryReader b = new BinaryReader(fileStream))
                {
                    ParseFileHeader(b);
                    if (_Header.Label != "MsgStdBn")
                        throw new Exception(string.Format("Can't load '{0}', the file doesn't appear to be a MSBT file.", soundMSBTFile));

                    //Keep header
                    b.BaseStream.Position = 0;
                    uint offSetLBL1 = HEADER_LEN;

                    //Getting offset to ATR1
                    b.BaseStream.Position = offSetLBL1;
                    _SectionLBL1 = ParseSectionHeader(b);
                    if (_SectionLBL1.Label != "LBL1")
                        throw new Exception(string.Format("Error while reading '{0}', can't find LBL1 section.", soundMSBTFile));
                    uint offSetATR1 = offSetLBL1 + HEADER_LBL1 + _SectionLBL1.SectionSize;
                    while (offSetATR1 % 0x10 != 0)// || offSetATR1 == offSetLBL1 + HEADER_LBL1 + _SectionLBL1.SectionSize)
                        offSetATR1++;
                    

                    //Getting offset to TXT2
                    b.BaseStream.Position = offSetATR1;
                    _SectionATR1 = ParseSectionHeader(b);
                    if (_SectionATR1.Label != "ATR1")
                        throw new Exception(string.Format("Error while reading '{0}', can't find ATR1 section.", soundMSBTFile));
                    b.BaseStream.Position = offSetATR1  + HEADER_ATR1;
                    _ATR1Bloc = b.ReadBytes((int)_SectionATR1.SectionSize);
                    uint offSetTXT2 = offSetATR1 + HEADER_ATR1 + _SectionATR1.SectionSize;
                    while (offSetTXT2 % 0x10 != 0)// || offSetTXT2 == offSetATR1 + HEADER_ATR1 + _SectionATR1.SectionSize)
                        offSetTXT2++;

                    //Parsing TXT2
                    b.BaseStream.Position = offSetTXT2;
                    _SectionTXT2 = ParseSectionHeader(b);
                    if (_SectionTXT2.Label != "TXT2")
                        throw new Exception(string.Format("Error while reading '{0}', can't find TXT2 section.", soundMSBTFile));
                    b.BaseStream.Position += 8;
                    uint txt2NbrEntries = ReadUInt32BigEndian(b);
                    uint[] offSetIntList = new uint[txt2NbrEntries];
                    for (int i = 0; i < txt2NbrEntries; i++)
                        offSetIntList[i] = ReadUInt32BigEndian(b);

                    b.BaseStream.Position = offSetTXT2 + HEADER_TXT2;
                    byte[] txt2Bloc = b.ReadBytes((int)_SectionTXT2.SectionSize);
                    string[] msbtvariables = GetMSBTStringVariables(txt2Bloc, txt2NbrEntries, offSetIntList);
                    
                    //Associate strings to variable names
                    b.BaseStream.Position = offSetLBL1 + HEADER_LBL1;
                    _SizeHashTable = ReadUInt32BigEndian(b);
                    uint lbl1ChecksumSize = _SizeHashTable * 0x8;
                    uint nbrEntries = 0;
                    for (int i = 0; i < _SizeHashTable; i++)
                    {
                        nbrEntries += ReadUInt32BigEndian(b);
                        b.BaseStream.Position += 4;
                    }
                    if (msbtvariables.Length != nbrEntries)
                        throw new Exception(string.Format("Error while reading '{0}', the number of LBL1 entries doesn't match the number of TXT2 entries.", soundMSBTFile));

                    b.BaseStream.Position = offSetLBL1 + HEADER_LBL1 + 0x4 + lbl1ChecksumSize;
                    for (int i = 0; i < nbrEntries; i++)
                    {
                        string variableName = b.ReadString();
                        uint variableIndex = ReadUInt32BigEndian(b);
                        MSBTVariable newVariable = new MSBTVariable(variableName, msbtvariables[variableIndex]);
                        _VarsMSBT.Add(variableName, newVariable);
                    }
                }  
            }

            //Assign SoundEntries variables
            if (!_ResCol.IsRegion)
            {
                foreach (SoundEntry sEntry in sEntryCollection.SoundEntries)
                {
                    sEntry.Title = GetVariableValue(VAR_TITLE + sEntry.OriginalSoundLabel, Strings.DEFAULT_SENTRY_TITLE);
                    sEntry.SoundTestTitle = GetVariableValue(VAR_TITLE2 + sEntry.OriginalSoundLabel, Strings.DEFAULT_SENTRY_TITLE2);
                    sEntry.Description = GetVariableValue(VAR_DESCRIPTION + sEntry.OriginalSoundLabel, string.Empty);
                    sEntry.Description2 = GetVariableValue(VAR_DESCRIPTION2 + sEntry.OriginalSoundLabel, string.Empty);
                    sEntry.Source = GetVariableValue(VAR_SOURCE + sEntry.OriginalSoundLabel, string.Empty);
                }
            }
        }

        private string[] GetMSBTStringVariables(byte[] txt2Bloc, uint txt2NbrEntries, uint[] offSetIntList)
        {
            string[] msbtvariables = null;
            using (MemoryStream mStream = new MemoryStream(txt2Bloc))
            {
                using (BinaryReader b = new BinaryReader(mStream, Encoding.BigEndianUnicode))
                {
                    msbtvariables = new string[txt2NbrEntries];
                    for (int i = 0; i < txt2NbrEntries; i++)
                    {
                        b.BaseStream.Position = offSetIntList[i];
                        //char newChar = char.MinValue;
                        string label = string.Empty;
                        uint nextOffSet = ((i + 1 < offSetIntList.Length) ? offSetIntList[i + 1] : (uint)txt2Bloc.Length) - 2;
                        //while ((newChar = b.ReadChar()) != '\0')
                        while (b.BaseStream.Position != nextOffSet)
                        {
                            label += b.ReadChar();
                        }
                        msbtvariables[i] = label;
                    }
                }
            }
            return msbtvariables;
        }

        private void ParseFileHeader(BinaryReader b)
        {
            _Header = new MSBTHeader(new string(b.ReadChars(8)));
            _Header.EndianValue = ReadUInt16BigEndian(b);
            _Header.Unk1 = b.ReadBytes(2);
            _Header.CharacterSize = b.ReadByte();
            _Header.Unk2 = b.ReadByte();
            _Header.NbrSections = ReadUInt16BigEndian(b);
            _Header.Unk3 = b.ReadBytes(2);
            _Header.FileSize = ReadUInt32BigEndian(b);
        }

        private MSBTSectionHeader ParseSectionHeader(BinaryReader b)
        {
            string label = new string(b.ReadChars(4));
            uint size = ReadUInt32BigEndian(b);
            return new MSBTSectionHeader(label, size);
        }

        private string GetVariableValue(string name, string defaultName)
        {
            if (_VarsMSBT.ContainsKey(name))
                return _VarsMSBT[name].Value.Replace("\n", Environment.NewLine);
            return defaultName;
        }

        public void BuildFile()
        {
            string savePath = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH) + _Path.Replace('/', Path.DirectorySeparatorChar);

            //GetNewDictionary
            SortedDictionary<string, MSBTVariable> newMSBT = BuildNewMSBTDatabase();

            //LBL1
            byte[] lbl1Block = BuildLBL1(newMSBT);

            //ATR1
            byte[] atr1Block = BuildATR1();

            //TXT2
            byte[] txt2Block = BuildTXT2(newMSBT);

            //Header
            _Header.FileSize = (uint)lbl1Block.Length + (uint)atr1Block.Length + (uint)txt2Block.Length + HEADER_LEN;
            byte[] header = BuildHeader();

            byte[] finalFile = new byte[_Header.FileSize];
            Buffer.BlockCopy(header, 0, finalFile, 0, header.Length);
            Buffer.BlockCopy(lbl1Block, 0, finalFile, header.Length, lbl1Block.Length);
            Buffer.BlockCopy(atr1Block, 0, finalFile, header.Length + lbl1Block.Length, atr1Block.Length);
            Buffer.BlockCopy(txt2Block, 0, finalFile, header.Length + lbl1Block.Length + atr1Block.Length, txt2Block.Length);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.WriteAllBytes(savePath, finalFile);
        }

        private SortedDictionary<string, MSBTVariable> BuildNewMSBTDatabase()
        {
            SortedDictionary<string, MSBTVariable> newMSBTDB = new SortedDictionary<string, MSBTVariable>();

            //First, we add the non sound entry variables
            foreach (MSBTVariable variable in _VarsMSBT.Values)
                if (!variable.IsSoundEntryVariable())
                    newMSBTDB.Add(variable.Name, variable);

            //Then, we manually generate all the "new variables"
            foreach (SoundEntry sEntry in SoundEntryCollection.SoundEntries)
                SetNewMSBTPerSoundEntry(newMSBTDB, sEntry);

            return newMSBTDB;
        }

        private byte[] BuildLBL1(SortedDictionary<string, MSBTVariable> msbtDictionary)
        {
            List<MSBTVariable>[] hashTable = new List<MSBTVariable>[_SizeHashTable];
            uint[] hashTableBlocSize = new uint[_SizeHashTable];
            List<uint>[] hashTableIndex = new List<uint>[_SizeHashTable];
            for (int i = 0; i < hashTable.Length; i++)
            {
                hashTable[i] = new List<MSBTVariable>();
                hashTableIndex[i] = new List<uint>();
            }

            uint index = 0;
            foreach (MSBTVariable msbtVariable in msbtDictionary.Values)
            {
                int group = GetLBL1HashValue(msbtVariable.Name);
                hashTable[group].Add(msbtVariable);
                hashTableIndex[group].Add(index);
                hashTableBlocSize[group] += (uint)(msbtVariable.Name.Length + 5);
                index++;
            }

            byte[] lbl1BlocWithoutHeader = null;
            uint sizeLbl1Bloc = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    w.Write(ConvertUInt32BigEndian(_SizeHashTable));

                    uint offSetLabels = 0x4 + _SizeHashTable * 0x8;
                    for (int i = 0; i < hashTable.Length; i++)
                    {
                        w.Write(ConvertUInt32BigEndian((uint)hashTable[i].Count));
                        w.Write(ConvertUInt32BigEndian(offSetLabels));
                        offSetLabels += hashTableBlocSize[i];
                    }

                    for (int i = 0; i < hashTable.Length; i++)
                    {
                        for(int j = 0; j < hashTable[i].Count; j++)
                        {
                            w.Write(hashTable[i][j].Name);
                            w.Write(ConvertUInt32BigEndian(hashTableIndex[i][j]));
                        }
                    }

                    sizeLbl1Bloc = (uint)w.BaseStream.Length;
                    uint sizePadding = sizeLbl1Bloc;
                    while (sizePadding % 0x10 != 0)
                    {
                        w.Write((byte)0xab);
                        sizePadding++;
                    }
                }

                lbl1BlocWithoutHeader = stream.ToArray();
            }

            //Adding header
            _SectionLBL1.SectionSize = sizeLbl1Bloc;
            byte[] header = BuildSectionHeader(_SectionLBL1);

            byte[] lbl1Bloc = new byte[lbl1BlocWithoutHeader.Length + header.Length];
            Buffer.BlockCopy(header, 0, lbl1Bloc, 0, header.Length);
            Buffer.BlockCopy(lbl1BlocWithoutHeader, 0, lbl1Bloc, header.Length, lbl1BlocWithoutHeader.Length);

            return lbl1Bloc;
        }

        private byte[] BuildATR1()
        {
            byte[] atr1BlocWithoutHeader = null;
            uint sizeAtr1Bloc = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    w.Write(_ATR1Bloc);

                    sizeAtr1Bloc = (uint)w.BaseStream.Length;
                    uint sizePadding = sizeAtr1Bloc;
                    while (sizePadding % 0x10 != 0)
                    {
                        w.Write((byte)0xab);
                        sizePadding++;
                    }
                }
                atr1BlocWithoutHeader = stream.ToArray();
            }


            byte[] header = BuildSectionHeader(_SectionATR1);

            byte[] atr1Bloc = new byte[atr1BlocWithoutHeader.Length + header.Length];
            Buffer.BlockCopy(header, 0, atr1Bloc, 0, header.Length);
            Buffer.BlockCopy(atr1BlocWithoutHeader, 0, atr1Bloc, header.Length, atr1BlocWithoutHeader.Length);
            return atr1Bloc;
        }

        private byte[] BuildTXT2(SortedDictionary<string, MSBTVariable> msbtDictionary)
        {
            byte[] txt2BlocWithoutHeader = null;
            uint sizeTxt2Bloc = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    uint nbrEntries = (uint)msbtDictionary.Count;
                    w.Write(ConvertUInt32BigEndian(nbrEntries));

                    uint offsetText = 0x4 + (nbrEntries * 0x4);

                    foreach (MSBTVariable msbtVariable in msbtDictionary.Values)
                    {
                        w.Write(ConvertUInt32BigEndian(offsetText));
                        offsetText += (uint)((msbtVariable.Value.Length * 2) + 2);
                    }

                    foreach (MSBTVariable msbtVariable in msbtDictionary.Values)
                    {
                        w.Write(Encoding.BigEndianUnicode.GetBytes(msbtVariable.Value));
                        w.Write((byte)0x0);
                        w.Write((byte)0x0);
                    }

                    sizeTxt2Bloc = (uint)w.BaseStream.Length;
                    uint sizePadding = sizeTxt2Bloc;
                    while (sizePadding % 0x10 != 0)
                    {
                        w.Write((byte)0xab);
                        sizePadding++;
                    }
                }
                txt2BlocWithoutHeader = stream.ToArray();
            }

            //Adding header
            _SectionTXT2.SectionSize = sizeTxt2Bloc;
            byte[] header = BuildSectionHeader(_SectionTXT2);

            byte[] txt2Bloc = new byte[txt2BlocWithoutHeader.Length + header.Length];
            Buffer.BlockCopy(header, 0, txt2Bloc, 0, header.Length);
            Buffer.BlockCopy(txt2BlocWithoutHeader, 0, txt2Bloc, header.Length, txt2BlocWithoutHeader.Length);

            return txt2Bloc;
        }

        private int GetLBL1HashValue(string input)
        {
            int group = 0;
            for(int i = 0; i < input.Length; i++)
            {
                group *= 0x492;
                group += (int)(input[i]);
            }
            return (int)((group & 0xffffffff) % (int)_SizeHashTable);
        }

        private byte[] BuildHeader()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    w.Write(Encoding.ASCII.GetBytes(_Header.Label));
                    w.Write(ConvertUInt16BigEndian(_Header.EndianValue));
                    w.Write(_Header.Unk1);
                    w.Write(_Header.CharacterSize);
                    w.Write(_Header.Unk2);
                    w.Write(ConvertUInt16BigEndian(_Header.NbrSections));
                    w.Write(_Header.Unk3);
                    w.Write(ConvertUInt32BigEndian(_Header.FileSize));
                    while (w.BaseStream.Length < 0x20)
                        w.Write((byte)0x0);
                }
                return stream.ToArray();
            }
        }

        private byte[] BuildSectionHeader(MSBTSectionHeader sectionHeader)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    //Label
                    w.Write(Encoding.ASCII.GetBytes(sectionHeader.Label));
                    while (w.BaseStream.Length < 4)
                        w.Write((byte)0x0);
                    //SectionSize
                    w.Write(ConvertUInt32BigEndian(sectionHeader.SectionSize));
                    while (w.BaseStream.Length < 0x10)
                        w.Write((byte)0x0);
                }
                return stream.ToArray();
            }
        }

        private void SetNewMSBTPerSoundEntry(SortedDictionary<string, MSBTVariable> newMSBTDB, SoundEntry sEntry)
        {
            MSBTVariable newTitle = new MSBTVariable(VAR_TITLE + sEntry.SoundLabel, sEntry.Title.Replace(Environment.NewLine, "\n"));
            MSBTVariable newTitle2 = new MSBTVariable(VAR_TITLE2 + sEntry.SoundLabel, sEntry.SoundTestTitle.Replace(Environment.NewLine, "\n"));
            MSBTVariable newDescription = new MSBTVariable(VAR_DESCRIPTION + sEntry.SoundLabel, sEntry.Description.Replace(Environment.NewLine, "\n"));
            MSBTVariable newDescription2 = new MSBTVariable(VAR_DESCRIPTION2 + sEntry.SoundLabel, sEntry.Description2.Replace(Environment.NewLine, "\n"));
            MSBTVariable newSource = new MSBTVariable(VAR_SOURCE + sEntry.SoundLabel, sEntry.Source.Replace(Environment.NewLine, "\n"));

            newMSBTDB.Add(newTitle.Name, newTitle);
            newMSBTDB.Add(newTitle2.Name, newTitle2);
            newMSBTDB.Add(newDescription.Name, newDescription);
            newMSBTDB.Add(newDescription2.Name, newDescription2);
            newMSBTDB.Add(newSource.Name, newSource);
        }

        #region Generic Data
        private uint ReadUInt32BigEndian(BinaryReader b)
        {
            byte[] value = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }

        private ushort ReadUInt16BigEndian(BinaryReader b)
        {
            byte[] value = b.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return BitConverter.ToUInt16(value, 0);
        }

        private byte[] ConvertUInt32BigEndian(uint input)
        {
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return value;
        }

        private byte[] ConvertUInt16BigEndian(ushort input)
        {
            byte[] value = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            return value;
        }
        #endregion
    }

    public class MSBTHeader
    {
        public string Label { get; set; }
        public ushort EndianValue { get; set; }
        public byte[] Unk1 { get; set; }
        public byte CharacterSize { get; set; }
        public byte Unk2 { get; set; }
        public ushort NbrSections { get; set; }
        public byte[] Unk3 { get; set; }
        public uint FileSize { get; set; }

        public MSBTHeader(string label)
        {
            Label = label;
        }
    }

    public class MSBTSectionHeader
    {
        public string Label { get; set; }
        public uint SectionSize { get; set; }

        public MSBTSectionHeader(string label, uint sectionSize)
        {
            Label = label;
            SectionSize = sectionSize;
        }
    }
}
