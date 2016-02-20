using Sm4shProjectManager.Globals;
using Sm4shProjectManager.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sm4shProjectManager
{
    public class Sm4shProject
    {
        #region Members
        private static Sm4shMod _CurrentProject;
        private ResourceCollection[] _resCols;
        private string _ProjectFilePath = string.Empty;
        private RFManager _RfManager;
        #endregion

        #region Properties
        public ResourceCollection[] ResourceData { get { return _resCols; } }

        public static Sm4shMod CurrentProject { get { return _CurrentProject; } }
        #endregion

        #region Project Management
        public Sm4shMod CreateNewProject(string projectFilePath, string gamePath)
        {
            Sm4shMod newProject = new Sm4shMod();
            newProject.GamePath = gamePath;
            newProject.SkipJunkEntries = true;
            newProject.ExportCSVList = true;
            newProject.ExportCSVIgnoreCompSize = true;
            newProject.ExportCSVIgnoreFlags = true;
            newProject.ExportCSVIgnorePackOffsets = true;
            _CurrentProject = newProject;
            _ProjectFilePath = projectFilePath;

            SaveProject();

            return newProject;
        }

        public void SaveProject()
        {
            if (_RfManager != null)
            {
                _RfManager.Debug = _CurrentProject.Debug;
                _RfManager.SkipTrashEntries = _CurrentProject.SkipJunkEntries;
                _RfManager.ForceOriginalFlags = _CurrentProject.KeepOriginalFlags;
            }
            XmlSerializer ser = new XmlSerializer(typeof(Sm4shMod));
            using (TextWriter writer = new StreamWriter(_ProjectFilePath, false))
                ser.Serialize(writer, _CurrentProject);
        }

        public Sm4shMod LoadProject(string projectPathFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Sm4shMod));
            Sm4shMod loadedProject = null;
            using (StreamReader reader = new StreamReader(projectPathFile))
                loadedProject = (Sm4shMod)(ser.Deserialize(reader));
            _CurrentProject = loadedProject;
            _ProjectFilePath = projectPathFile;

            if (string.IsNullOrEmpty(_CurrentProject.ProjectExportPath) || !Directory.Exists(_CurrentProject.ProjectExportPath))
                _CurrentProject.ProjectExportPath = PathHelper.GetProjectExportFolder();
            if(string.IsNullOrEmpty(_CurrentProject.ProjectExportPath) || !Directory.Exists(_CurrentProject.ProjectExportPath))
                _CurrentProject.ProjectExtractPath = PathHelper.GetProjectExtractFolder();
            if (string.IsNullOrEmpty(_CurrentProject.ProjectTempPath) || !Directory.Exists(_CurrentProject.ProjectTempPath))
                _CurrentProject.ProjectTempPath = PathHelper.GetProjectTempFolder();
            if (string.IsNullOrEmpty(_CurrentProject.ProjectWorkplacePath) || !Directory.Exists(_CurrentProject.ProjectWorkplacePath))
                _CurrentProject.ProjectWorkplacePath = PathHelper.GetProjectWorkplaceFolder();

            if (!PathHelper.IsItSmashFolder(_CurrentProject.GamePath) || !PathHelper.DoesItHavePatchFolder(_CurrentProject.GamePath))
                return null;
            LoadProjectData();

            return loadedProject;
        }
        #endregion

        #region Loading Data
        public void LoadProjectData()
        {
            //Create temp folder
            Directory.CreateDirectory(PathHelper.GetProjectTempFolder());

            _RfManager = new RFManager(PathHelper.GetGamePath(PathHelperEnum.FILE_LS), PathHelper.GetDTFiles(), PathHelper.GetProjectTempFolder());
            _RfManager.Debug = _CurrentProject.Debug;
            _RfManager.SkipTrashEntries = _CurrentProject.SkipJunkEntries;
            _RfManager.ForceOriginalFlags = _CurrentProject.KeepOriginalFlags;

            string[] rfFiles = null;
            PatchFileItem[] patchFiles = null;
            if (File.Exists(PathHelper.GetGamePath(PathHelperEnum.FILE_PATCH_RESOURCE)))
            {
                rfFiles = PathHelper.GetResourceFiles(PathHelper.GetGamePath(PathHelperEnum.FOLDER_PATCH));
                patchFiles = _RfManager.LoadPatchFile(PathHelper.GetGamePath(PathHelperEnum.FILE_PATCH_PATCHLIST));
            }
            else
            {
                //LS, todo
                return;
            }

            //Read meta.xml
            try
            {
                string metaFilePath = PathHelper.GetGamePath(PathHelperEnum.FILE_META);
                if (File.Exists(metaFilePath))
                {
                    XmlDocument fileMeta = new XmlDocument();
                    fileMeta.Load(metaFilePath);
                    XmlNodeList nodeGameID = fileMeta.DocumentElement.SelectNodes("/menu/product_code");
                    XmlNodeList nodeGameVersion = fileMeta.DocumentElement.SelectNodes("/menu/title_version");
                    XmlNodeList nodeGameRegion = fileMeta.DocumentElement.SelectNodes("/menu/region");
                    _CurrentProject.GameID = nodeGameID[0].InnerText;
                    _CurrentProject.GameVersion = nodeGameVersion[0].InnerText;
                    _CurrentProject.GameID = _CurrentProject.GameID.Substring(_CurrentProject.GameID.LastIndexOf("-") + 1) + "01";
                    int regionCode = Convert.ToInt32(nodeGameRegion[0].InnerText);
                    switch (regionCode)
                    {
                        case 1:
                            _CurrentProject.GameRegion = "JAP";
                            break;
                        case 2:
                            _CurrentProject.GameRegion = "USA";
                            break;
                        case 3:
                            _CurrentProject.GameRegion = "EUR";
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing meta.xml: " + e.Message);
            }
            
            //Load RF Files
            _resCols = _RfManager.LoadRFFiles(rfFiles);
        }
        #endregion

        #region Extract Data
        private string ExtractFile(ResourceCollection resCol, string fileToExtractPath, string outputFolder, bool isFromFolder)
        {
            //Extract from workspace
            string filePath = PathHelper.GetProjectWorkplaceFolder() + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + fileToExtractPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
            string extractPath = outputFolder + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + fileToExtractPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR);

            try
            {
                if (fileToExtractPath.EndsWith("/"))
                {
                    Directory.CreateDirectory(extractPath);
                    return fileToExtractPath;
                }

                if (File.Exists(filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                    File.Copy(filePath, extractPath, true);
                    return extractPath;
                }

                ResourceItem rItem = null;
                if (!resCol.Resources.ContainsKey(fileToExtractPath))
                {
                    Console.WriteLine(string.Format("Can't find file {0} in resource data{1}", fileToExtractPath, resCol.Region));
                    return string.Empty;
                }
                rItem = resCol.Resources[fileToExtractPath];

                if (!isFromFolder)
                    Console.WriteLine(string.Format("Extracting file {0} from {1}...", rItem.Path, rItem.Source.ToString()));


                switch (rItem.Source)
                {
                    case FileSource.LS:
                        _RfManager.ExtractFileFromLS(rItem, extractPath);
                        break;
                    case FileSource.Patch:
                        string folderPatch = PathHelper.GetGamePath(PathHelperEnum.FOLDER_PATCH) + "data" + rItem.ResourceCollection.Region + GlobalConstants.FOLDER_SEPARATOR + rItem.Path.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
                        _RfManager.ExtractFileFromPatch(rItem, folderPatch, extractPath);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error extracting file '{0}': {1}", filePath, e.Message));
            }

            if (!isFromFolder)
            {
                _RfManager.ClearCachedDataSources();
                Console.WriteLine("Done!");
            }

            return extractPath;
        }

        public string ExtractFile(ResourceCollection resCol, string fileToExtractPath, string outputFolder)
        {
            return ExtractFile(resCol, fileToExtractPath, outputFolder, false);
        }

        public void ExtractFolder(ResourceCollection resCol, string pathToExtract, string outputFolder)
        {
            List<string> rResources = FilterPath(resCol.Resources.Keys.ToArray(), pathToExtract);
            rResources.AddRange(FilterPath(GetAllModPaths(resCol.Region), pathToExtract));
            rResources = rResources.Distinct().ToList();

            Console.WriteLine(string.Format("Extracting {0} files from {1}...", rResources.Count, pathToExtract));

            foreach (string resource in rResources)
            {
                ExtractFile(resCol, resource, outputFolder, true);
            }
            _RfManager.ClearCachedDataSources();

            Console.WriteLine("Done!");
        }
        #endregion

        #region Add/Remove Files to Workspace
        public void AddFilesToWorkspace(ResourceCollection resCol, string[] files, string destinationPath)
        {
            foreach (string file in files)
            {
                List<string> filesList = new List<string>();
                filesList.Add(file);

                if (!File.Exists(file))
                {
                    string[] subFiles = Directory.GetFiles(file, "*", SearchOption.AllDirectories);
                    filesList.AddRange(subFiles);
                }

                //Copy
                string baseToExclude = filesList[0].Substring(0, filesList[0].LastIndexOf(GlobalConstants.FOLDER_SEPARATOR));
                foreach (string fileToProcess in filesList)
                {
                    if (!Utils.IsAnAcceptedExtension(fileToProcess) || fileToProcess.EndsWith("packed"))
                        Console.WriteLine(string.Format("The file '{0}' has a forbidden extension, skipping...", fileToProcess));
                    else
                    {
                        string newPath = PathHelper.GetProjectWorkplaceFolder() + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + destinationPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR) + fileToProcess.Replace(baseToExclude, string.Empty);
                        if (Path.GetFileName(newPath).Contains("."))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                            File.Copy(fileToProcess, newPath, true);
                        }
                    }
                }
            }
        }

        public bool RemoveFilesFromWorkspace(ResourceCollection resCol, string pathToRemove)
        {
            Console.WriteLine(string.Format("Removing mod files from '{0}'...", pathToRemove));

            string pathToDelete = GetWorkspaceFileFromPath(resCol, pathToRemove);

            if (File.Exists(pathToDelete))
                File.Delete(pathToDelete);
            else if (Directory.Exists(pathToDelete))
                Directory.Delete(pathToDelete, true);
            else
                return false;

            Console.WriteLine("Done!");

            return true;
        }
        #endregion

        #region Modifying Files
        public bool BuildNewResources(ResourceCollection resCol, string[] filesTest, string exportFolder, bool packing)
        {
            List<string> listFiles = new List<string>();
            foreach (string file in filesTest)
            {
                if (!Utils.IsAnAcceptedExtension(file))
                    Console.WriteLine(string.Format("The file '{0}' has a forbidden extension, skipping...", file));
                else
                    listFiles.Add(file);
            }
            string[] files = listFiles.ToArray();

            string baseToExclude = files[0].Substring(0, files[0].LastIndexOf(GlobalConstants.FOLDER_SEPARATOR) + 1);
            List<PackageProcessor> packNeedsRepacking = new List<PackageProcessor>();
            foreach (string file in files)
            {
                string newPath = file.Replace(baseToExclude, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                string fileName = Path.GetFileName(file);
                bool isFolder = false;
                if (!fileName.Contains("."))
                {
                    newPath += "/";
                    fileName += "/";
                    isFolder = true;
                }

                //Checking if part of a pack to repack
                ResourceItem packedPath = GetPackedPath(resCol, newPath);
                if (packedPath != null && packNeedsRepacking.Find(p => p.PackedPath == packedPath.Path) == null)
                    packNeedsRepacking.Add(new PackageProcessor() { TopResource = packedPath, PackedPath = packedPath.Path, BaseToExclude = baseToExclude, ExportFolder = exportFolder });
                if (packedPath != null)
                {
                    packNeedsRepacking.Find(p => p.PackedPath == packedPath.Path).FilesToAdd.Add(file);
                    continue;
                }

                ResourceItem nItem = GetEditedResource(resCol, newPath, newPath, fileName);

                if (!isFolder)
                {
                    uint cmpSize;
                    uint decSize;
                    byte[] fileToWrite;

                    GetFileSizes(nItem, file, out cmpSize, out decSize, out fileToWrite);

                    nItem.CmpSize = cmpSize;
                    nItem.DecSize = decSize;

                    //Copy to export folder
                    string savePath = exportFolder + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + newPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, fileToWrite);
                }
            }

            //packed management
            foreach (PackageProcessor package in packNeedsRepacking)
                ProcessPackage(package, packing);

            return true;
        }

        private void ProcessPackageUnpacked(ResourceItem rItemTopResource, string[] files, string exportFolder)
        {
            ResourceCollection resCol = rItemTopResource.ResourceCollection;
            string baseTempFolder = PathHelper.GetProjectTempFolder() + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR;

            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];

                string resPath = file.Replace(baseTempFolder, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                string fileName = Path.GetFileName(file);
                bool isFolder = false;
                if (!fileName.Contains("."))
                {
                    resPath += "/";
                    fileName += "/";
                    isFolder = true;
                }

                ResourceItem nItem = GetEditedResource(resCol, resPath, rItemTopResource.Path, fileName);
                nItem.OffInPack = 0;
                uint cmpSize = 0;
                uint decSize = 0;

                string unpackedPath = exportFolder + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + file.Replace(baseTempFolder, string.Empty);
                if (!isFolder)
                {
                    byte[] fileToWrite;

                    GetFileSizes(nItem, file, out cmpSize, out decSize, out fileToWrite);

                    nItem.CmpSize = cmpSize;
                    nItem.DecSize = decSize;
                    nItem.IsAPackage = true;

                    //Saving UnPackedFiles
                    Directory.CreateDirectory(Path.GetDirectoryName(unpackedPath));
                    File.WriteAllBytes(unpackedPath, fileToWrite);
                }
                else
                {
                    nItem.CmpSize = 0;
                    nItem.DecSize = 0;
                    nItem.IsAPackage = false;

                    Directory.CreateDirectory(unpackedPath);
                }
            }
        }

        private void ProcessPackage(PackageProcessor package, bool packing)
        {
            ResourceCollection resCol = package.TopResource.ResourceCollection;
            string baseTempFolder = PathHelper.GetProjectTempFolder() + "data" + package.TopResource.ResourceCollection.Region + GlobalConstants.FOLDER_SEPARATOR;
            if(Directory.Exists(baseTempFolder))
                Directory.Delete(baseTempFolder, true);

            //Extract all files
            ExtractFolder(package.TopResource.ResourceCollection, package.PackedPath, PathHelper.GetProjectTempFolder());

            //Override with new files
            if (package.FilesToAdd != null)
            {
                foreach (string file in package.FilesToAdd)
                {
                    //string newPath = package.ExportFolder + file.Replace(package.BaseToExclude, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                    string savePath = file.Replace(package.BaseToExclude, baseTempFolder);//baseTempFolder + newPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
                    if (!Path.GetFileName(savePath).Contains("."))
                        Directory.CreateDirectory(savePath);
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        File.Copy(file, savePath, true);
                    }
                }
            }

            //Sort files with ordinal comparison
            string baseFolder = baseTempFolder + package.PackedPath.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
            string[] files = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);
            string[] folders = Directory.GetDirectories(baseFolder, "*", SearchOption.AllDirectories);
            int filesLength = files.Length;
            Array.Resize<string>(ref files, filesLength + folders.Length + 1);
            Array.Copy(folders, 0, files, filesLength, folders.Length);
            files[files.Length - 1] = baseFolder.Substring(0, baseFolder.Length - 1);
            Array.Sort(files, new CustomStringComparer());

            if (!packing)
            {
                ProcessPackageUnpacked(package.TopResource, files, package.ExportFolder);
            }
            else
            {
                Console.WriteLine(string.Format("Packing '{0}' with {1} new files...", package.PackedPath, package.FilesToAdd.Count));
                using (MemoryStream streamPackage = new MemoryStream())
                {
                    using (BinaryWriter writerPackage = new BinaryWriter(streamPackage))
                    {
                        ResourceItem[] currentFolder = new ResourceItem[20];
                        string[] currentFolderFile = new string[20];

                        for (int f = 0; f < files.Length; f++)
                        {
                            uint currentPosition = (uint)streamPackage.Position;
                            string file = files[f];
                            uint paddingNeeded = 0;

                            string resPath = file.Replace(baseTempFolder, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                            string fileName = Path.GetFileName(file);
                            bool isFolder = false;
                            if (!fileName.Contains("."))
                            {
                                resPath += "/";
                                fileName += "/";
                                isFolder = true;
                            }

                            ResourceItem nItem = GetEditedResource(resCol, resPath, package.TopResource.Path, fileName);
                            int folderDepth = (int)nItem.FolderDepth;
                            uint cmpSize = 0;
                            uint decSize = 0;
                            nItem.IsAPackage = f == 0;

                            if (!isFolder)
                            {
                                byte[] fileToWrite;

                                GetFileSizes(nItem, file, out cmpSize, out decSize, out fileToWrite);

                                nItem.CmpSize = cmpSize;
                                nItem.DecSize = decSize;
                                nItem.OffInPack = (uint)streamPackage.Position;

                                //Write the file
                                if (fileToWrite.Length > 0)
                                    writerPackage.Write(fileToWrite);
                                else
                                {//If empty, must had 0x80 padding
                                    for (int i = 0x00; i < 0x80; i++)
                                        writerPackage.Write((byte)0xCC);
                                    paddingNeeded = 0x80;
                                }

                                if (f < files.Length - 1) //No padding for the last file
                                {
                                    uint fileAndPadding = cmpSize;
                                    while (fileAndPadding % 0x80 != 0 || fileAndPadding - cmpSize < 0x40)
                                    {
                                        writerPackage.Write((byte)0xCC);
                                        fileAndPadding++;
                                        paddingNeeded++;
                                    }
                                }

                                folderDepth--; //Back to folder level
                            }
                            else
                            {
                                nItem.OffInPack = (uint)streamPackage.Position;
                                nItem.CmpSize = 0;
                                nItem.DecSize = 0;
                                cmpSize = decSize = 0x80;

                                //New folder, add proper padding and yada yada
                                for (int i = 0x00; i < 0x80; i++)
                                    writerPackage.Write((byte)0xCC);

                                currentFolder[folderDepth] = nItem;
                                currentFolderFile[folderDepth] = file + GlobalConstants.FOLDER_SEPARATOR;
                            }

                            //Update folders size
                            for (int i = folderDepth; i >= 0; i--)
                            {
                                if (currentFolder[i] == null)
                                    break;
                                currentFolder[i].CmpSize += cmpSize;
                                currentFolder[i].DecSize += decSize;
                                if (CheckIfLastFileInFolder(files, currentFolderFile[i], f))
                                {
                                    currentFolder[i].CmpSize += paddingNeeded;
                                    currentFolder[i].DecSize += paddingNeeded;
                                }
                            }
                        }
                    }

                    //Saving PackedFile
                    string packedPath = package.ExportFolder + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + files[0].Replace(baseTempFolder, string.Empty) + GlobalConstants.FOLDER_SEPARATOR + "packed";
                    Directory.CreateDirectory(Path.GetDirectoryName(packedPath));
                    File.WriteAllBytes(packedPath, streamPackage.ToArray());
                }
            }

            if (Directory.Exists(baseTempFolder))
                Directory.Delete(baseTempFolder, true);
        }

        public string[] GetAllModPaths(string regionPath)
        {
            if (!regionPath.Contains("data"))
                regionPath = "data" + regionPath;
            string baseToRemove = PathHelper.GetProjectWorkplaceFolder() + regionPath + GlobalConstants.FOLDER_SEPARATOR;
            if (Directory.Exists(baseToRemove))
            {
                string[] files = Directory.GetFiles(baseToRemove, "*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                    files[i] = files[i].Replace(baseToRemove, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                return files;
            }
            return new string[0];
        }

        private bool CheckIfLastFileInFolder(string[] files, string path, int position)
        {
            for (int i = position + 1; i < files.Length; i++)
            {
                if(files[i].StartsWith(path))
                    return true;
            }
            return false;
        }

        private void GetFileSizes(ResourceItem rItem, string file, out uint cmpSize, out uint decSize, out byte[] fileToWrite)
        {
            byte[] fileBinary = File.ReadAllBytes(file);
            cmpSize = (uint)fileBinary.Length;
            decSize = (uint)fileBinary.Length;
            //If file already uncompressed, check its dec size
            if (Utils.IsCompressed(fileBinary))
            {
                fileBinary = Utils.DeCompress(fileBinary);
                decSize = (uint)fileBinary.Length;
                cmpSize = (uint)fileBinary.Length;
            }
            //Check if it should be compressed, and return its CmpSize
            if (ShouldBeCompressed(rItem, fileBinary))
            {
                fileToWrite = Utils.Compress(fileBinary);
                cmpSize = (uint)fileToWrite.Length;
            }
            else
                fileToWrite = fileBinary;
        }

        private bool ShouldBeCompressed(ResourceItem rItem, byte[] file)
        {
            if (GlobalConstants.FORCE_ORIGINAL_COMPRESSION && rItem.OriginalResourceItem != null && rItem.OriginalResourceItem.CmpSize == rItem.OriginalResourceItem.DecSize)
                return false;

            //More rules to add
            if (rItem.Filename.EndsWith(".nus3bank"))
            {
                if ((rItem.Filename.StartsWith("snd_vc_") || rItem.Filename.StartsWith("snd_se_")) && !rItem.Filename.Contains("_ouen_"))
                    return false;
            }

            if (file.Length >= GlobalConstants.SIZE_FILE_COMPRESSION_MIN)
                return true;

            return false;
        }

        private ResourceItem GetEditedResource(ResourceCollection resCol, string resPath, string patchResPath, string fileName)
        {
            ResourceItem nItem = null;
            if (resCol.Resources.ContainsKey(resPath))
            {
                nItem = resCol.Resources[resPath];
                nItem.OriginalResourceItem = (ResourceItem)nItem.Clone();
            }
            else
            {
                nItem = new ResourceItem(fileName, 0, 0, 0, false);
                resCol.Resources.Add(resPath, nItem);
                nItem.Path = resPath;
                nItem.ResourceCollection = resCol;
            }

            //Common info
            if (nItem.PatchItem == null)
                nItem.PatchItem = new PatchFileItem();
            nItem.PatchItem.Path = patchResPath;
            nItem.Source = FileSource.Mod;
            nItem.PatchItem.RegionPath = "data" + resCol.Region + "/" + patchResPath;

            return nItem;
        }

        public ResourceItem GetPackedPath(ResourceCollection resCol, string path)
        {
            if (resCol.Resources.ContainsKey(path) && resCol.Resources[path].IsAPackage && !resCol.Resources[path].IsFolder)
                return null;

            while (!resCol.Resources.ContainsKey(path) || !resCol.Resources[path].IsAPackage)
            {
                int nextPathCalc = path.LastIndexOf("/", path.Length - 2);
                if (nextPathCalc == -1)
                    return null;
                path = path.Substring(0, nextPathCalc) + "/";
            }
            return resCol.Resources[path];
        }
        #endregion

        #region Unlocalizing Files
        public void UnlocalizeResources(ResourceCollection resCol, string pathToUnlocalize)
        {
            //Get list of files to unlocalize
            List<ResourceItem> uItems = GetAllResourceFromPath(resCol, pathToUnlocalize);
            ResourceCollection dataCol = _resCols.LastOrDefault(p => string.IsNullOrEmpty(p.Region));

            Console.WriteLine(string.Format("Unlocalizing {0} files from {1}...", uItems.Count, pathToUnlocalize));

            foreach (ResourceItem uItem in uItems)
            {
                if (!dataCol.Resources.ContainsKey(uItem.Path))
                {
                    Console.WriteLine(string.Format("Warning! The resource '{0}' does not exist in the data folder! Skipping it but it might cause an issue.", uItem.Path));
                    continue;
                }

                ResourceItem packedItem = GetPackedPath(uItem.ResourceCollection, uItem.Path);
                if(packedItem != null)
                    _CurrentProject.AddUnlocalized(resCol.Region, packedItem.Path + "packed");
                else
                    _CurrentProject.AddUnlocalized(resCol.Region, uItem.Path);
            }

            SaveProject();

            Console.WriteLine("Done!");
        }

        public void RemoveUnlocalized(ResourceCollection resCol, string pathToRemove)
        {
            Console.WriteLine(string.Format("Removing unlocalization for {0}...", pathToRemove));

            string[] paths = resCol.Resources.Keys.ToArray();
            foreach (string path in paths)
            {
                if (path.StartsWith(pathToRemove))
                {
                    ResourceItem dItem = resCol.Resources[path];
                    if (dItem.IsAPackage && dItem.IsFolder)
                        CurrentProject.RemoveUnlocalized(resCol.Region, path + "packed");
                    else
                        CurrentProject.RemoveUnlocalized(resCol.Region, path);
                }
            }

            SaveProject();

            Console.WriteLine("Done!");
        }
        #endregion       

        #region Compression Management
        /*public void SetResourceCompression(ResourceCollection resCol, string filePath, FileCompression compression)
        {
            //SET COMPRESSION
            //TODO
        }*/
        #endregion

        #region Rebuilding Files
        public void RebuildRFAndPatchlist()
        {
            RebuildRFAndPatchlist(true);
        }

        public void RebuildRFAndPatchlist(bool packing)
        {
            string exportFolder = PathHelper.GetProjectExportFolder() + (packing ? "release" : "debug") + GlobalConstants.FOLDER_SEPARATOR + (_CurrentProject.ExportWithDateFolder ? string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + GlobalConstants.FOLDER_SEPARATOR : string.Empty);
            if (Directory.Exists(exportFolder))
                Directory.Delete(exportFolder, true);

            //Copy resources, pack files if needed
            ResourceCollection dataResCol = null;
            foreach (ResourceCollection resCol in _resCols)
            {
                //Cloning to leave the original resourcecollection untouched
                ResourceCollection newCol = (ResourceCollection)resCol.Clone();

                //Build Package
                BuildPackage(newCol, exportFolder, packing);

                //Build Resource
                Console.WriteLine(string.Format("Rebuilding 'resource{0}'...", newCol.Region));
                ResourceCollection collection = null;
                if (!newCol.IsRegion) //No region, take the resource file "as it"
                {
                    collection = newCol;
                    dataResCol = newCol;
                }
                else
                    collection = GetMergedRegionResources(dataResCol, newCol);

                _RfManager.RebuildResourceFile(collection, exportFolder);

                //Save CSV
                ExportCSV(resCol, collection, exportFolder);
            }

            //Patchlist
            Console.WriteLine("Rebuilding 'patchlist'...");
            BuildPatchfile(exportFolder);

            Console.WriteLine("Done!");
        }

        #region CSV Generation
        private void ExportCSV(ResourceCollection originalResCol, ResourceCollection newResCol, string exportFolder)
        {
            if (_CurrentProject.ExportCSVList)
            {
                List<string> outputCSV = new List<string>();
                outputCSV.Add(GetHeader());

                foreach (ResourceItem nItem in newResCol.Resources.Values)
                {
                    if (!originalResCol.Resources.ContainsKey(nItem.Path))
                    {
                        outputCSV.Add(GetCSVLine(null, nItem));
                        continue;
                    }

                    ResourceItem oItem = originalResCol.Resources[nItem.Path];

                    //DecSize
                    if (oItem.DecSize != nItem.DecSize)
                    {
                        outputCSV.Add(GetCSVLine(oItem, nItem));
                        continue;
                    }

                    //CmpSize
                    if (!_CurrentProject.ExportCSVIgnoreCompSize && oItem.CmpSize != nItem.CmpSize)
                    {
                        outputCSV.Add(GetCSVLine(oItem, nItem));
                        continue;
                    }

                    //Flags
                    if (!_CurrentProject.ExportCSVIgnoreFlags && oItem.OriginalFlags != nItem.Flags)
                    {
                        outputCSV.Add(GetCSVLine(oItem, nItem));
                        continue;
                    }

                    //OffsetInPack
                    if (!_CurrentProject.ExportCSVIgnorePackOffsets && oItem.OffInPack != nItem.OffInPack)
                    {
                        outputCSV.Add(GetCSVLine(oItem, nItem));
                        continue;
                    }
                }
                File.WriteAllLines(exportFolder + "~resource" + newResCol.Region + ".csv", outputCSV);
            }
        }

        private string GetHeader()
        {
            List<string> headerParts = new List<string>();
            headerParts.Add("Path");
            headerParts.Add("New decompSize");
            headerParts.Add("Old decompSize");
            headerParts.Add("New compSize");
            if(!_CurrentProject.ExportCSVIgnoreCompSize)
                headerParts.Add("Old compSize");
            headerParts.Add("New flags");
            if (!_CurrentProject.ExportCSVIgnoreFlags)
                headerParts.Add("Old flags");
            if (!_CurrentProject.ExportCSVIgnorePackOffsets)
            {
                headerParts.Add("New offset in pack");
                headerParts.Add("Old offset in pack");
            }
            return string.Join(",", headerParts);
        }

        private string GetCSVLine(ResourceItem oldItem, ResourceItem newItem)
        {
            List<string> lineParts = new List<string>();
            lineParts.Add(newItem.Path);

            //DecSize
            lineParts.Add(newItem.DecSize.ToString());
            if(oldItem == null)
                lineParts.Add(string.Empty);
            else
                lineParts.Add(oldItem.DecSize.ToString());

            //CompSize
            lineParts.Add(newItem.CmpSize.ToString());
            if (!_CurrentProject.ExportCSVIgnoreCompSize)
            {
                if (oldItem == null)
                    lineParts.Add(string.Empty);
                else
                    lineParts.Add(oldItem.CmpSize.ToString());
            }

            //Flags
            lineParts.Add(String.Format("0x{0:X4}", newItem.Flags));
            if (!_CurrentProject.ExportCSVIgnoreFlags)
            {
                if (oldItem == null)
                    lineParts.Add(string.Empty);
                else
                lineParts.Add(String.Format("0x{0:X4}", oldItem.Flags));
            }

            //PackOffset
            if (!_CurrentProject.ExportCSVIgnorePackOffsets)
            {
                lineParts.Add(String.Format("0x{0:X8}", newItem.OffInPack));
                if (oldItem == null)
                    lineParts.Add(string.Empty);
                else
                    lineParts.Add(String.Format("0x{0:X8}", oldItem.OffInPack));
            }
            return string.Join(",", lineParts);
        }
        #endregion CSV Generation

        private ResourceCollection GetMergedRegionResources(ResourceCollection dataResCol, ResourceCollection resCol)
        {
            ResourceCollection outputResCol = new ResourceCollection(resCol.Region);
            Dictionary<string, ResourceItem> resources = outputResCol.Resources;

            Dictionary<string, ResourceItem> mainResources = dataResCol.Resources;
            Dictionary<string, ResourceItem> regionResources = resCol.GetFilteredResources();
            string region = resCol.Region;

            foreach (ResourceItem mainRItem in mainResources.Values)
            {
                if (!resCol.Resources.ContainsKey(mainRItem.Path) && mainRItem.Path.EndsWith("_us_en.flx"))
                    continue;

                ResourceItem rItem = (ResourceItem)mainResources[mainRItem.Path].Clone();
                rItem.ResourceCollection = outputResCol;
                if (regionResources.ContainsKey(mainRItem.Path) && !CurrentProject.IsUnlocalized(region, mainRItem.Path))
                {
                    resources.Add(mainRItem.Path, regionResources[mainRItem.Path]);
                }
                else
                {
                    rItem.Source = FileSource.NotFound;
                    resources.Add(mainRItem.Path, rItem);
                }
            }

            foreach (ResourceItem regionRItem in regionResources.Values)
            {
                if (!resources.ContainsKey(regionRItem.Path))
                    resources.Add(regionRItem.Path, regionRItem);
            }

            return outputResCol;
        }

        public void BuildPackage(ResourceCollection resCol, string exportFolder, bool packing)
        {
            string baseFolder = PathHelper.GetProjectWorkplaceFolder() + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR;
            List<string> filesList = new List<string>();
            if (Directory.Exists(baseFolder))
            {
                filesList.AddRange(Directory.GetDirectories(baseFolder, "*", SearchOption.AllDirectories));
                filesList.AddRange(Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories));
                string[] filesToProcess = filesList.ToArray();
                Array.Sort(filesToProcess, new CustomStringComparer());
                if (filesToProcess.Length > 0)
                {
                    Console.WriteLine(string.Format("Packaging 'data{0}'... to '{1}'", resCol.Region, exportFolder));
                    BuildNewResources(resCol, filesToProcess, exportFolder, packing);
                }
            }
        }

        public void BuildPatchfile(string exportFolder)
        {
            if (Directory.Exists(exportFolder))
            {
                string[] filesInExport = Directory.GetFiles(exportFolder, "*", SearchOption.AllDirectories);

                List<string> filesToAdd = new List<string>();
                List<string> filesToRemove = new List<string>();

                //Files to add
                foreach (string fileInExport in filesInExport)
                {
                    if (!Utils.IsAnAcceptedExtension(fileInExport))
                        continue;
                    string pathToProcess = fileInExport.Replace(exportFolder, string.Empty).Replace(GlobalConstants.FOLDER_SEPARATOR, "/");
                    if (!filesToAdd.Contains(pathToProcess))
                        filesToAdd.Add(pathToProcess);
                }

                //Files to remove
                foreach (Sm4shModItem rModItem in _CurrentProject.Items)
                {
                    foreach (string pathToRemove in rModItem.UnlocalizedPaths)
                        filesToRemove.Add("data" + rModItem.Region + "/" + pathToRemove);
                }
                _RfManager.RebuildPatchListFile(filesToAdd.ToArray(), filesToRemove.ToArray(), exportFolder);
            }
        }
        #endregion

        #region Packing Folder
        public void PackFolder(ResourceCollection resCol, string pathToPack)
        {
            /*if (!resCol.Resources.ContainsKey(pathToPack))
            {
                Console.WriteLine(string.Format("Path {0} not found!", pathToPack));
                return;
            }
            Console.WriteLine(string.Format("Packing resource {0}...", pathToPack));

            ResourceItem rItem = resCol.Resources[pathToPack];

            PackageProcessor processPackage = new PackageProcessor();
            processPackage.TopResource = rItem;
            processPackage.PackedPath = rItem.Path;

            ProcessPackage(processPackage);

            SaveProject();*/
        }
        #endregion

        #region Tools Methods
        public List<string> FilterPath(string[] paths, string filter)
        {
            List<string> files = new List<string>();
            foreach (string path in paths)
            {
                if (path.StartsWith(filter))
                    files.Add(path);
            }
            return files;
        }

        public List<ResourceItem> GetAllResourceFromPath(ResourceCollection resCol, string rootPath)
        {
            List<ResourceItem> nItems = new List<ResourceItem>();
            string[] paths = resCol.GetFilteredResources().Keys.ToArray();
            foreach (string path in paths)
            {
                if (path.StartsWith(rootPath))
                    nItems.Add(resCol.Resources[path]);
            }
            return nItems;
        }

        public string GetWorkspaceFileFromPath(ResourceCollection resCol, string path)
        {
            return PathHelper.GetProjectWorkplaceFolder() + "data" + resCol.Region + GlobalConstants.FOLDER_SEPARATOR + path.Replace("/", GlobalConstants.FOLDER_SEPARATOR);
        }
        #endregion
    }
}
