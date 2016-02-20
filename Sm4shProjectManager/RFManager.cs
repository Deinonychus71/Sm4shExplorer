using DTLS;
using Sm4shProjectManager.Globals;
using Sm4shProjectManager.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Sm4shProjectManager
{
    public class RFManager
    {
        #region Members
        private string _lsFilePath = string.Empty;
        private string _TempFolder = string.Empty;
        private string[] _DtPaths = null;
        private PatchListFile _PatchFileList = null;
        public bool Debug { get; set; }
        public bool SkipTrashEntries { get; set; }
        public bool ForceOriginalFlags { get; set; }
        private Dictionary<string, DataSource> _CachedDataSources = new Dictionary<string, DataSource>();
        #endregion

        #region Constructors
        public RFManager(string lsFile, string[] dtPaths, string tempFolder)
        {
            _lsFilePath = lsFile;
            _TempFolder = tempFolder;
            _DtPaths = dtPaths;
        }
        #endregion

        #region Loading RFFiles
        public PatchFileItem[] LoadPatchFile(string patchFile)
        {
            Console.WriteLine(string.Format("Loading patchfile File '{0}'", patchFile));
            _PatchFileList = new PatchListFile(patchFile);
            return _PatchFileList.Files;
        }

        public ResourceCollection[] LoadRFFiles(string[] rfFiles)
        {
            //Loading LS File
            Console.WriteLine(string.Format("Loading LS File '{0}'", _lsFilePath));
            LSFile lsFile = new LSFile(_lsFilePath);

            //Sorting Resource files
            Array.Sort(rfFiles, new CustomStringComparer());

            //Loading RF Files
            ResourceCollection[] resCols = new ResourceCollection[rfFiles.Length];
            for (int i = 0; i < rfFiles.Length; i++)
            {
                Console.WriteLine(string.Format("Loading RF File '{0}' ({1}/{2})", rfFiles[i], i + 1, rfFiles.Length));
                resCols[i] = LoadRFFile(rfFiles[i], lsFile);
            }
            return resCols;
        }

        public ResourceCollection LoadRFFile(string rfFilePath, LSFile lsFile)
        {
            string region = Utils.GetRegionFromFilename(rfFilePath);
            PatchFileItem currentPackedPatchFile = null;

            //Create new ResourceCollection
            ResourceCollection resCol = new ResourceCollection(region);

            //Use temp folder instead of patch folder
            string tempRF = _TempFolder + Path.GetFileName(rfFilePath);
            File.Copy(rfFilePath, tempRF, true);

            //Load RF file
            RFFile rfFile = new RFFile(tempRF);

            string[] pathParts = new string[20];
            LSEntryObject[] offsetParts = new LSEntryObject[20];
            foreach (ResourceEntryObject rEntry in rfFile.ResourceEntries)
            {
                if (rEntry == null || string.IsNullOrEmpty(rEntry.EntryString))
                    continue;

                if (IsJunkEntry(rEntry))
                    continue;

                //Figuring out the path of the entry
                pathParts[rEntry.FolderDepth - 1] = rEntry.EntryString;
                Array.Clear(pathParts, rEntry.FolderDepth, pathParts.Length - (rEntry.FolderDepth));

                //New ResourceItem object
                ResourceItem rItem = new ResourceItem(rEntry.EntryString, rEntry.OffInPack, rEntry.CmpSize, rEntry.DecSize, rEntry.HasPack);
                rItem.Path = string.Join(string.Empty, pathParts);

                LSEntryObject fileEntry = null;
                PatchFileItem patchItem = null;
                if (rEntry.HasPack)
                {
                    //Check if part of the patch/mod
                    patchItem = _PatchFileList.GetRegionPath(rItem.Path, region);
                    if (patchItem != null)
                    {
                        if(patchItem.Packed)
                            currentPackedPatchFile = patchItem;
                    }
                    //Part of LS
                    else
                    {
                        currentPackedPatchFile = null;
                        string crcPath = "data" + region + "/" + rItem.Path.TrimEnd('/') + (rEntry.Compressed ? "/packed" : "");
                        uint crc = DTLS.IO.Util.calc_crc(crcPath);
                        lsFile.Entries.TryGetValue(crc, out fileEntry);
                        if (fileEntry == null)
                            rItem.Source = FileSource.NotFound;
                        else
                        {
                            lsFile.Entries.Remove(crc);
                            rItem.Source = FileSource.LS;
                        }
                    }
                }

                //Check if part of the patch/mod
                if (currentPackedPatchFile != null && rItem.Path.StartsWith(currentPackedPatchFile.Path))
                    rItem.Source = FileSource.Patch;
                else if (patchItem != null)
                    rItem.Source = FileSource.Patch;
                //Part of LS
                else
                {
                    currentPackedPatchFile = null;
                    offsetParts[rEntry.FolderDepth - 1] = fileEntry;
                    Array.Clear(offsetParts, rEntry.FolderDepth, offsetParts.Length - (rEntry.FolderDepth));

                    if (!rItem.Path.EndsWith("/"))
                    {
                        rItem.LSEntryInfo = offsetParts.LastOrDefault(x => x != null);
                        if (rItem.LSEntryInfo == null)
                            rItem.Source = FileSource.NotFound;
                        else
                            rItem.Source = FileSource.LS;
                    }
                }

                rItem.PatchItem = currentPackedPatchFile;
                rItem.ResourceCollection = resCol;

                if (rItem.Source != FileSource.NotFound)
                {
                    for (int i = 0; i < pathParts.Length; i++)
                    {
                        if (pathParts[i] != null)
                        {
                            string folder = string.Join(string.Empty, pathParts, 0, i);
                            if (!resCol.Resources.ContainsKey(folder))
                                continue;
                            if (rItem.Source == FileSource.LS && resCol.Resources[folder].Source == FileSource.Patch)
                                resCol.Resources[folder].Source = rItem.Source;
                            else if (resCol.Resources[folder].Source == FileSource.NotFound)
                                resCol.Resources[folder].Source = rItem.Source;
                        }
                    }
                }

                rItem.OriginalFlags = rEntry.Flags;

                resCol.Resources.Add(rItem.Path, rItem);
            }

            rfFile._workingSource.Close();
            if(File.Exists(tempRF))
                File.Delete(tempRF);
            if (File.Exists(tempRF + ".dec"))
            File.Delete(tempRF + ".dec");

            Console.WriteLine(string.Format("Resource entries: {0}", resCol.Resources.Count));
            return resCol;
        }
        #endregion

        public bool IsJunkEntry(ResourceEntryObject rEntry)
        {
            if (!SkipTrashEntries)
                return false;

            if ((rEntry.Flags & 0x2000) == 0x2000) //Empty folders, probably used for debug
                return true;
            if ((rEntry.Flags & 0x200) == 0 && !rEntry.EntryString.Contains(".")) //Trash files
                return true;
            return false;
        }

        #region Extract Files
        public bool ExtractFileFromLS(ResourceItem rItem, string outputPath)
        {
            try
            {
                if (rItem.LSEntryInfo == null)
                {
                    Console.WriteLine(string.Format("The LS information for {0} could not be found, can't extract the file.", rItem.Filename));
                    return false;
                }
                byte[] fileSize = GetFileDataDecompressed(rItem.LSEntryInfo.DTOffset + rItem.OffInPack, rItem.CmpSize, rItem.LSEntryInfo.DTIndex);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllBytes(outputPath, fileSize);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error extracting {0}, error: {1}", rItem.Filename, e.Message));
                return false;
            }
        }

        public bool ExtractFileFromPatch(ResourceItem rItem, string patchFolder, string outputPath)
        {
            try
            {
                if (File.Exists(patchFolder))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    File.Copy(patchFolder, outputPath, true);
                    return true;
                }

                string mainfolder = "";
                if (!rItem.Path.EndsWith("/"))
                {
                    DataSource packedSource;

                    //If packed file not datasourced yet
                    string packed = "data" + rItem.ResourceCollection.Region + "/" + rItem.PatchItem.Path;
                    if (!_CachedDataSources.TryGetValue(packed, out packedSource))
                    {
                        string path = patchFolder.Substring(0, patchFolder.LastIndexOf("\\data")) + GlobalConstants.FOLDER_SEPARATOR + packed.Replace("/", "\\") + "packed";
                        if (File.Exists(path))
                        {
                            packedSource = new DataSource(FileMap.FromFile(path));
                            _CachedDataSources.Add(packed, packedSource);
                            mainfolder = path.Remove(path.Length - 6);
                        }
                    }

                    var fileData = new byte[0];

                    byte[] checkCmp = packedSource.Slice((int)rItem.OffInPack, 4);
                    if (checkCmp[0] == 0x78 && checkCmp[1] == 0x9c)
                        fileData = Utils.DeCompress(packedSource.Slice((int)rItem.OffInPack, (int)rItem.CmpSize));
                    else
                        fileData = packedSource.Slice((int)rItem.OffInPack, (int)rItem.DecSize);
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    File.WriteAllBytes(outputPath, fileData);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error extracting {0}, error: {1}", rItem.Filename, e.Message));
                return false;
            }
        }

        public void ClearCachedDataSources()
        {
            foreach (DataSource ds in _CachedDataSources.Values)
                ds.Close();
            _CachedDataSources = new Dictionary<string, DataSource>();
        }

        /// <summary>
        /// Returns file data from the dt file with an index of "dtIndex"
        /// </summary>
        /// <param name="start">Start offset of file data</param>
        /// <param name="size">Size of the data in bytes</param>
        /// <param name="dtIndex">Index of the dt file to access</param>
        /// <returns></returns>
        private byte[] GetFileDataDecompressed(uint start, uint size, int dtIndex)
        {
            uint diff = 0;
            DataSource src = GetFileChunk(start, size, dtIndex, out diff);

            byte[] b = src.Slice((int)diff, (int)size);

            if (b.Length >= 4)
            {
                var z = 0;
                if (BitConverter.ToUInt32(b, 0) == 0xCCCCCCCC)
                {
                    while (b[z] != 0x78)
                        z += 2;
                }
                if (b[0] == 0x78 && b[1] == 0x9c)
                    b = Utils.DeCompress(b.Skip(z).ToArray());
            }
            src.Close();

            return b;
        }

        private DataSource GetFileChunk(uint start, uint size, int dtIndex, out uint difference)
        {
            SYSTEM_INFO _info = new SYSTEM_INFO();
            GetSystemInfo(ref _info);

            uint chunk_start = start;
            uint chunk_len = size;
            difference = 0;
            if (start % _info.allocationGranularity != 0)
            {
                chunk_start = start.RoundDown((int)_info.allocationGranularity);
                difference = start - chunk_start;
                chunk_len = difference + size;
            }
            return new DataSource(FileMap.FromFile(_DtPaths[dtIndex], FileMapProtect.ReadWrite, chunk_start, (int)chunk_len));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        #endregion

        #region Rebuilding
        private const int POSITION_NAME_START = 0x11;
        private const int POSITION_RESS_START = 0x18;
        public void RebuildResourceFile(ResourceCollection collection, string exportFolder)
        {
            int resourcesSize;
            Dictionary<string, int> resourcesOffsets;

            collection.SortResourcesDictionary();

            byte[] names = CreateNamesTable(collection.Resources, out resourcesOffsets);
            byte[] res = CreateResourcesTable(collection.Resources, resourcesOffsets, out resourcesSize);

            int realFinalSize = res.Length + names.Length;
            int finalPadding = realFinalSize;
            while (finalPadding % 0x10 != 0 || finalPadding < realFinalSize + 0x60)
                finalPadding++;

            byte[] final = new byte[finalPadding];
            Buffer.BlockCopy(res, 0, final, 0, res.Length);
            Buffer.BlockCopy(names, 0, final, res.Length, names.Length);
            for (int i = realFinalSize; i < finalPadding; i++)
                final[i] = 0xBB;

            byte[] finalCompressed = Utils.Compress(final);
            byte[] header = CreateNewHeader(resourcesSize, names.Length, collection.Resources.Count, finalCompressed.Length, final.Length, res.Length + 0x80);

            byte[] export = new byte[header.Length + finalCompressed.Length];
            Buffer.BlockCopy(header, 0, export, 0, header.Length);
            Buffer.BlockCopy(finalCompressed, 0, export, header.Length, finalCompressed.Length);

            Directory.CreateDirectory(exportFolder);
            File.WriteAllBytes(exportFolder + "resource" + collection.Region, export);
            if (Debug)
            {
                byte[] unCompressed = new byte[header.Length + final.Length];
                Buffer.BlockCopy(header, 0, unCompressed, 0, header.Length);
                Buffer.BlockCopy(final, 0, unCompressed, header.Length, final.Length);
                File.WriteAllBytes(exportFolder + "resource" + collection.Region + ".dec", unCompressed);
            }
        }

        public void RebuildPatchListFile(string[] filesToAdd, string[] filesToRemove, string exportFolder)
        {
            List<string> files = new List<string>();
            foreach (PatchFileItem pItem in _PatchFileList.Files)
            {
                string formatedPath = pItem.RegionPath + (pItem.Packed ? "packed" : string.Empty);
                if (Array.Exists(filesToRemove, p => p == formatedPath))
                    continue;
                files.Add(formatedPath);
            }
            foreach (string fileToAdd in filesToAdd)
                if (!files.Contains(fileToAdd))
                    files.Add(fileToAdd);

            string[] strFilesFinal = files.ToArray();
            Array.Sort(strFilesFinal, new CustomStringComparer());

            int headerSize = _PatchFileList.Header.Length;
            byte[] fileFinal = new byte[headerSize + (strFilesFinal.Length * 0x80)];
            byte[] newValueLength = BitConverter.GetBytes(strFilesFinal.Length);
            Buffer.BlockCopy(_PatchFileList.Header, 0, fileFinal, 0, headerSize);
            Buffer.BlockCopy(newValueLength, 0, fileFinal, 0x04, newValueLength.Length);
            for(int i = 0; i < strFilesFinal.Length; i++)
            {
                byte[] strByte = Encoding.ASCII.GetBytes(strFilesFinal[i]);
                Buffer.BlockCopy(strByte, 0, fileFinal, headerSize + (i * 0x80), strByte.Length);
            }

            Directory.CreateDirectory(exportFolder);
            File.WriteAllBytes(exportFolder + "patchlist", fileFinal);
        }

        public ushort GetUInt16FlagForCompressedNames(int length, int offset)
        {
            return (ushort)(((length - 4) & 0x1f) | (offset & 0x300) >> 2 | (offset & 0xFF) << 8);
        }

        public byte[] CreateNewHeader(int resourcesSize, int namesSize, int nbrEntries, int compressedSize, int uncompressedSize, int offSetNames)
        {
            byte[] output = null;
            using (MemoryStream streamHeader = new MemoryStream())
            {
                using (BinaryWriter writerHeader = new BinaryWriter(streamHeader))
                {
                    byte[] tag = new byte[4] { 0x52, 0x46, 0x06, 0x00 };
                    writerHeader.Write(tag); // RF
                    writerHeader.Write(0x80); //Header length
                    writerHeader.Write(0x00); //Padding
                    writerHeader.Write(0x80); //Resource entry

                    writerHeader.Write(resourcesSize); //Resource count
                    writerHeader.Write(0x00); //Timestamp
                    writerHeader.Write(compressedSize); //Compressed size
                    writerHeader.Write(uncompressedSize); //Decompressed size

                    writerHeader.Write(offSetNames); //Offset names
                    writerHeader.Write(namesSize); //Name size
                    writerHeader.Write(nbrEntries + 1); //Nbr entries

                    for (int i = 0x00; i < 0x54; i += 0x01)
                    {
                        writerHeader.Write((byte)0xaa);
                    }
                    output = streamHeader.ToArray();
                }
            }
            return output;
        }

        public byte[] CreateNamesTable(Dictionary<string, ResourceItem> resourceFlatView, out Dictionary<string, int> resourcesOffsets)
        {
            resourcesOffsets = new Dictionary<string, int>();
            List<ResourceNameItem> resourceNames = new List<ResourceNameItem>();
            ResourceExtension[] extensionsName = Utils.GetExtensionTable();
            int currentPosition = POSITION_NAME_START;
            int namesPadding = 0;
            byte[] names = null;

            using (MemoryStream streamNames = new MemoryStream())
            {
                using (BinaryWriter writerNames = new BinaryWriter(streamNames))
                {
                    //Initial bytes
                    writerNames.Seek(4 + POSITION_NAME_START, SeekOrigin.Begin);

                    foreach (ResourceItem rItem in resourceFlatView.Values)
                    {
                        string name = rItem.Filename;

                        //If extension, remove the extension, and add it if new extension
                        int extensionFlag = 0;
                        if (!rItem.IsFolder)
                        {
                            string extension = name.Substring(name.LastIndexOf("."));
                            name = name.Substring(0, name.LastIndexOf("."));
                            ResourceExtension extensionItem = extensionsName.LastOrDefault(p => p.Name == extension);
                            if (extensionItem == null)
                                throw new Exception("Extension not found!");

                            if (extensionItem.Offset == 0)
                            {
                                writerNames.Write(Encoding.ASCII.GetBytes(extension));
                                writerNames.Write((byte)0);
                                extensionItem.Offset = currentPosition;
                                currentPosition += (extension.Length + 1);
                            }
                            extensionFlag = extensionItem.Flag;
                        }

                        //If name already exists, return its offset
                        ResourceNameItem nameItem = resourceNames.Find(p => p.Name == name);
                        if (nameItem != null)
                        {
                            resourcesOffsets.Add(rItem.Path, nameItem.Offset | extensionFlag);
                            continue;
                        }

                        //Code to prevent reading a name between 2 blocks of 0x2000
                        for (int i = 0; i < name.Length + 4; i++)
                        {
                            if ((currentPosition + i) % 0x2000 == 0)
                            {
                                for (int j = 0; j < i; j++)
                                    writerNames.Write((byte)0);
                                currentPosition += i;
                            }
                        }

                        //The name doesnt exist at all, creating it
                        writerNames.Write(Encoding.ASCII.GetBytes(name));
                        writerNames.Write((byte)0);
                        resourceNames.Add(new ResourceNameItem() { Name = name, Offset = currentPosition });
                        resourcesOffsets.Add(rItem.Path, currentPosition | extensionFlag);
                        currentPosition += name.Length + 1;
                    }

                    //Name padding
                    namesPadding = currentPosition; //the first 4 bits are not part of the string table
                    while (namesPadding % 0x2000 != 0)
                        namesPadding++;
                    writerNames.Seek(namesPadding - currentPosition, SeekOrigin.End);

                    //Writing extensions
                    writerNames.Write(extensionsName.Length);
                    foreach (ResourceExtension nameItem in extensionsName)
                        writerNames.Write(nameItem.Offset);
                }
                names = streamNames.ToArray();
            }
            names[0] = (byte)(namesPadding / 0x2000);
            return names;
        }

        public byte[] CreateResourcesTable(Dictionary<string, ResourceItem> resourceFlatView, Dictionary<string, int> resourcesOffsets, out int resourceSize)
        {
            byte[] res = null;

            using (MemoryStream streamResources = new MemoryStream())
            {
                using (BinaryWriter writerResources = new BinaryWriter(streamResources))
                {
                    //Initial bytes
                    writerResources.Seek(POSITION_RESS_START, SeekOrigin.Begin);

                    foreach (ResourceItem rItem in resourceFlatView.Values)
                    {
                        uint flags = ForceOriginalFlags ? (rItem.OriginalFlags != 0 ? rItem.OriginalFlags : rItem.Flags) : rItem.Flags;
                        writerResources.Write(rItem.OffInPack);
                        writerResources.Write(resourcesOffsets[rItem.Path]);
                        writerResources.Write(rItem.CmpSize);
                        writerResources.Write(rItem.DecSize);
                        writerResources.Write(0); //Timestamp
                        writerResources.Write(flags);
                    }

                    int resourcePadding = streamResources.ToArray().Length;
                    resourceSize = resourcePadding;
                    while (resourcePadding % 0x10 != 0 || resourcePadding < resourceSize + 0x20)
                    {
                        writerResources.Write((byte)0xBB);
                        resourcePadding++;
                    }
                }
                res = streamResources.ToArray();
            }

            res[0x04] = (byte)0x10;
            res[0x15] = (byte)0x02;

            return res;
        }
        #endregion
    }
}
