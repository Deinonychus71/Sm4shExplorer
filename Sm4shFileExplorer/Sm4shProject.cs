using Sm4shFileExplorer.DB;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Sm4shFileExplorer
{
    public class Sm4shProject
    {
        #region Members
        private Sm4shMod _CurrentProject;
        private ResourceCollection[] _resCols;
        private ResourceCollection _resColDataCore;
        private string _ProjectFilePath = string.Empty;
        private RFManager _RfManager;
        private BindingList<Sm4shBasePlugin> _Plugins;
        private string _CachedSDPath;
        #endregion

        #region Properties
        /// <summary>
        /// Object representing the game list of resources (all regions, including core data).
        /// </summary>
        public ResourceCollection[] ResourceDataCollection { get { return _resCols; } }

        /// <summary>
        /// Object representing the game list of resources (only core data).
        /// </summary>
        public ResourceCollection ResourceDataCore { get { return _resColDataCore; } }

        /// <summary>
        /// Configuration of the project
        /// </summary>
        public Sm4shMod CurrentProject { get { return _CurrentProject; } }

        internal BindingList<Sm4shBasePlugin> Plugins { get { return _Plugins; } }
        #endregion

        #region Project Management
        internal Sm4shMod CreateNewProject(string projectFilePath, string gamePath)
        {
            LogHelper.Info(string.Format("Creating a new project: '{0}' with GamePath: '{1}'", projectFilePath, gamePath));

            Sm4shMod newProject = new Sm4shMod();
            newProject.GamePath = gamePath;
            newProject.SkipJunkEntries = true;
            newProject.ExportCSVList = false;
            newProject.ExportCSVIgnoreCompSize = true;
            newProject.ExportCSVIgnoreFlags = true;
            newProject.ExportCSVIgnorePackOffsets = true;
            _CurrentProject = newProject;
            _ProjectFilePath = projectFilePath;

            PathHelper.InitializePathHelper(newProject);

            //Read meta.xml
            try
            {
                string metaFilePath = PathHelper.GetGameFolder(PathHelperEnum.FILE_META);
                XmlDocument fileMeta = new XmlDocument();
                fileMeta.Load(metaFilePath);
                XmlNodeList nodeGameVersion = fileMeta.DocumentElement.SelectNodes("/menu/title_version");
                XmlNodeList nodeGameRegion = fileMeta.DocumentElement.SelectNodes("/menu/region");
                _CurrentProject.GameVersion = Convert.ToInt32(nodeGameVersion[0].InnerText);
                _CurrentProject.GameRegionID = Convert.ToInt32(nodeGameRegion[0].InnerText);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error parsing meta.xml. Sm4shexplorer will assume that you are using the latest patch ({0}) and that your game region is USA. If it isn't the case please update the config file before doing anything. (error: {1})", GlobalConstants.GAME_LAST_PATH_VERSION, e.Message));
                _CurrentProject.GameVersion = GlobalConstants.GAME_LAST_PATH_VERSION;
                _CurrentProject.GameRegionID = 2;
            }

            SaveProject();

            return newProject;
        }

        internal void SaveProject()
        {
            if (_RfManager != null)
            {
                _RfManager.Debug = _CurrentProject.Debug;
                _RfManager.SkipTrashEntries = _CurrentProject.SkipJunkEntries;
                _RfManager.ForceOriginalFlags = _CurrentProject.KeepOriginalFlags;
            }
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Sm4shMod));
                using (TextWriter writer = new StreamWriter(_ProjectFilePath, false))
                    ser.Serialize(writer, _CurrentProject);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error while serializing and saving the project! {0}", e.Message));
            }
        }

        internal Sm4shMod LoadProject(string projectPathFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Sm4shMod));
            Sm4shMod loadedProject = null;
            using (StreamReader reader = new StreamReader(projectPathFile))
                loadedProject = (Sm4shMod)(ser.Deserialize(reader));
            _CurrentProject = loadedProject;
            _ProjectFilePath = projectPathFile;

            LogHelper.InitializeLogHelper(_CurrentProject);
            PathHelper.InitializePathHelper(_CurrentProject);

            LogHelper.Info(string.Format("Loading from game: {0}", _CurrentProject.GamePath));

            if (string.IsNullOrEmpty(_CurrentProject.ProjectExportFolder) || !Directory.Exists(_CurrentProject.ProjectExportFolder))
                _CurrentProject.ProjectExportFolder = PathHelper.FolderExport;
            if (string.IsNullOrEmpty(_CurrentProject.ProjectExportFolder) || !Directory.Exists(_CurrentProject.ProjectExportFolder))
                _CurrentProject.ProjectExtractFolder = PathHelper.FolderExtract;
            if (string.IsNullOrEmpty(_CurrentProject.ProjectTempFolder) || !Directory.Exists(_CurrentProject.ProjectTempFolder))
                _CurrentProject.ProjectTempFolder = PathHelper.FolderTemp;
            if (string.IsNullOrEmpty(_CurrentProject.ProjectWorkplaceFolder) || !Directory.Exists(_CurrentProject.ProjectWorkplaceFolder))
                _CurrentProject.ProjectWorkplaceFolder = PathHelper.FolderWorkplace;

            if (!PathHelper.IsItSmashFolder(_CurrentProject.GamePath) || !PathHelper.DoesItHavePatchFolder(_CurrentProject.GamePath))
            {
                LogHelper.Error(string.Format("There seems to be a problem with the game folder: '{0}', check your config file and make sure this directory contains the game and is accessible.", _CurrentProject.GamePath));
                return null;
            }
            LoadProjectData();

            return loadedProject;
        }

        internal void InitializeDBs()
        {
            try
            {
                LogHelper.Debug("Initializing general DBs...");
                StagesDB.InitializeStageDB(_CurrentProject.GameVersion, _CurrentProject.Is3DS);
                IconsDB.InitializeIconsDB(_CurrentProject.GameVersion, _CurrentProject.Is3DS);
                CharsDB.InitializeCharsDB(_CurrentProject.GameVersion, _CurrentProject.Is3DS);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error while loading the DBs: {0}", e.Message));
            }
        }
        #endregion

        #region Loading Data
        internal void LoadProjectData()
        {
            LogHelper.Debug("Loading data...");

            //Create temp folder
            Directory.CreateDirectory(PathHelper.FolderTemp);

            _RfManager = new RFManager(PathHelper.GetGameFolder(PathHelperEnum.FILE_LS), PathHelper.GetDTFiles(), PathHelper.FolderTemp);
            _RfManager.Debug = _CurrentProject.Debug;
            _RfManager.SkipTrashEntries = _CurrentProject.SkipJunkEntries;
            _RfManager.ForceOriginalFlags = _CurrentProject.KeepOriginalFlags;

            string[] rfFiles = null;
            PatchFileItem[] patchFiles = null;
            if (File.Exists(PathHelper.GetGameFolder(PathHelperEnum.FILE_PATCH_RESOURCE)))
            {
                rfFiles = PathHelper.GetResourceFiles(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_PATCH));
                patchFiles = _RfManager.LoadPatchFile(PathHelper.GetGameFolder(PathHelperEnum.FILE_PATCH_PATCHLIST));
            }
            else
            {
                //LS, todo
                LogHelper.Error(string.Format("Loading the game from LS is not supported yet, sm4shexplorer couldn't find the resource file from the patch: '{0}'", PathHelper.GetGameFolder(PathHelperEnum.FILE_PATCH_RESOURCE)));
                return;
            }

            //Load RF Files
            _resCols = _RfManager.LoadRFFiles(rfFiles);
            _resColDataCore = _resCols.LastOrDefault(p => !p.IsRegion);
            if (_resColDataCore == null)
                LogHelper.Error("Missing core data resource.");

            //Check DT files
            _CurrentProject.DTFilesFound = CheckDTFiles();

            //Init DBS
            InitializeDBs();

            //Load Plugins
            LoadPlugins();
        }
        #endregion

        #region Extract Data
        #region public methods
        /// <summary>
        /// Extract a resource to the "extract" folder
        /// The path should be "data/folder1/folder2/file"
        /// If you wish to extract a folder, don't forget the "/" at the end
        /// </summary>
        /// <param name="absolutePath">Absolute path</param>
        /// <param name="outputFolder">Output folder</param>
        /// <returns>Path to the file or folder extracted</returns>
        public string ExtractResource(string absolutePath, string outputFolder)
        {
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            string relativePath = GetRelativePath(absolutePath);
            return ExtractResource(resCol, relativePath, outputFolder);
        }

        /// <summary>
        /// Extract a resource to the "extract" folder
        /// The path should be "data/folder1/folder2/file"
        /// If you wish to extract a folder, don't forget the "/" at the end
        /// </summary>
        /// <param name="absolutePath">Absolute path</param>
        /// <returns>Path to the file or folder extracted</returns>
        public string ExtractResource(string absolutePath)
        {
            return ExtractResource(absolutePath, PathHelper.FolderExtract);
        }

        /// <summary>
        /// Extract a resource to the "extract" folder
        /// The path should be "folder1/folder2/file"
        /// If you wish to extract a folder, don't forget the "/" at the end
        /// </summary>
        /// <param name="resCol">Resource Collection</param>
        /// <param name="relativePath">Relative path</param>
        /// <param name="outputFolder">Output folder</param>
        /// <returns>Path to the file or folder extracted</returns>
        public string ExtractResource(ResourceCollection resCol, string relativePath, string outputFolder)
        {
            if (!relativePath.EndsWith("/"))
                return ExtractFile(resCol, relativePath, outputFolder);
            else
                ExtractFolder(resCol, relativePath, outputFolder);
            return string.Empty;
        }

        /// <summary>
        /// Extract a resource to the "extract" folder
        /// The path should be "folder1/folder2/file"
        /// If you wish to extract a folder, don't forget the "/" at the end
        /// </summary>
        /// <param name="resCol">Resource Collection</param>
        /// <param name="relativePath">Relative path</param>
        /// <returns>Path to the file or folder extracted</returns>
        public string ExtractResource(ResourceCollection resCol, string relativePath)
        {
            return ExtractResource(resCol, relativePath, PathHelper.FolderExtract);
        }
        #endregion

        #region private methods
        private string ExtractFile(ResourceCollection resCol, string relativePath, string outputFile, bool isFromFolder)
        {
            //Get absolute path
            string absolutePath = GetAbsolutePath(resCol, relativePath);

            //Get workplace file, in case a mod already exist
            string workplaceFile = GetWorkspaceFileFromPath(absolutePath);

            //Get extract path
            string extractFile = outputFile + Path.DirectorySeparatorChar + absolutePath.Replace('/', Path.DirectorySeparatorChar);

            try
            {
                //If file is present in workplace, a mod already exist.
                if (File.Exists(workplaceFile))
                {
                    IOHelper.CopyFile(workplaceFile, extractFile);

                    if (!isFromFolder)
                        LogHelper.Info(string.Format("Extracting file '{0}' from Mod...", absolutePath));

                    return extractFile;
                }

                //If not, we make sure the path given is correct and exist in the resource collection
                ResourceItem rItem = null;
                if (!resCol.Resources.ContainsKey(relativePath))
                {
                    LogHelper.Error(string.Format("Can't find file '{0}' in resource {1}", absolutePath, resCol.PartitionName));
                    return string.Empty;
                }
                rItem = resCol.Resources[relativePath];

                //Special log for single file extraction
                if (!isFromFolder)
                    LogHelper.Info(string.Format("Extracting file '{0}' from {1}...", absolutePath, rItem.Source.ToString()));

                //Extraction method depending of the source
                switch (rItem.Source)
                {
                    case FileSource.LS:
                        //Make sure the DT files were found, warn the user if they couldnt be found
                        if (!_CurrentProject.DTFilesFound)
                            LogHelper.Error(string.Format("Can't extract '{0}' because the DT file(s) could not be found. Please restart Sm4shExplorer with the DT file(s) in the content folder.", absolutePath));
                        else
                            _RfManager.ExtractFileFromLS(rItem, extractFile);
                        break;
                    case FileSource.Patch:
                        _RfManager.ExtractFileFromPatch(rItem, extractFile);
                        break;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error extracting file '{0}': {1}", absolutePath, e.Message));
            }

            if (!isFromFolder)
            {
                _RfManager.ClearCachedDataSources();
                LogHelper.Info("Done!");
            }

            return extractFile;
        }

        private string ExtractFile(ResourceCollection resCol, string relativePath, string outputFolder)
        {
            return ExtractFile(resCol, relativePath, outputFolder, false);
        }

        private string ExtractFolder(ResourceCollection resCol, string relativePath, string outputFolder)
        {
            //Get all the existing resource to extract
            List<string> lResources = FilterRelativePath(resCol.CachedFilteredResources.Keys.ToArray(), relativePath);
            //Get all the existing modded files from workspace
            lResources.AddRange(FilterRelativePath(GetAllWorkplaceRelativePaths(resCol.PartitionName), relativePath));
            //Distinct union
            lResources = lResources.Distinct().ToList();

            LogHelper.Info(string.Format("Extracting {0} files from {1}...", lResources.Count, relativePath));

            foreach (string resource in lResources)
            {
                if (!resource.EndsWith("/"))
                    ExtractFile(resCol, resource, outputFolder, true);
            }
            _RfManager.ClearCachedDataSources();

            LogHelper.Info("Done!");

            return outputFolder;
        }
        #endregion
        #endregion

        #region Add/Remove Files to Workspace
        /// <summary>
        /// Add a new file/folder into the workspace
        /// </summary>
        /// <param name="inputFile">File/Folder you want to add</param>
        /// <param name="absolutePath">Absolute game path where the file should be copied in the workspace</param>
        public void AddFileToWorkspace(string inputFile, string absolutePath)
        {
            LogHelper.Info(string.Format("Adding mod file '{0}' to resource '{1}'...", inputFile, absolutePath));

            List<string> lFiles = new List<string>();
            lFiles.Add(inputFile);

            if (!File.Exists(inputFile))
            {
                string[] subFiles = Directory.GetFiles(inputFile, "*", SearchOption.AllDirectories);
                lFiles.AddRange(subFiles);
            }
            LogHelper.Debug(string.Format("{0} files to add.", lFiles.Count));

            //Plugin ResourcesAddingToWorkspace
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            string relativePath = GetRelativePath(absolutePath);
            foreach (Sm4shBasePlugin plugin in _Plugins)
            {
                PluginActionResult result = plugin.InternalResourcesAddingToWorkspace(resCol, relativePath, inputFile);
                if (result.Cancel)
                {
                    LogHelper.Error(string.Format("Resource adding interrupted, reason: {0}", result.Reason));
                    return;
                }
            }

            //Copy
            string baseToExclude = lFiles[0].Substring(0, lFiles[0].LastIndexOf(Path.DirectorySeparatorChar));
            foreach (string fileToProcess in lFiles)
            {
                if (!Utils.IsAnAcceptedExtension(fileToProcess) || fileToProcess.EndsWith("packed"))
                    LogHelper.Warning(string.Format("The file '{0}' has a forbidden extension, skipping...", fileToProcess));
                else
                {
                    string newFile = GetWorkspaceFileFromPath(absolutePath) + fileToProcess.Replace(baseToExclude, string.Empty);

                    FileAttributes pathAttrs = File.GetAttributes(fileToProcess);
                    if (!pathAttrs.HasFlag(FileAttributes.Directory))
                        IOHelper.CopyFile(fileToProcess, newFile);
                }
            }

            //Plugin ResourcesAddedToWorkspace
            foreach (Sm4shBasePlugin plugin in _Plugins)
                plugin.InternalResourcesAddedToWorkspace(resCol, relativePath, inputFile);

            LogHelper.Info("Done!");
        }

        /// <summary>
        /// Remove a file/folder from the workspace
        /// </summary>
        /// <param name="absolutePath">Absolute game path of the file/folder that must be removed</param>
        /// <returns>true if the file/folder is deleted, otherwise false</returns>
        public bool RemoveFileFromWorkspace(string absolutePath)
        {
            LogHelper.Info(string.Format("Removing mod files from '{0}'...", absolutePath));

            string pathToDelete = GetWorkspaceFileFromPath(absolutePath);

            //Plugin ResourcesRemovingFromWorkspace
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            string relativePath = GetRelativePath(absolutePath);
            foreach (Sm4shBasePlugin plugin in _Plugins)
            {
                PluginActionResult result = plugin.InternalResourcesRemovingFromWorkspace(resCol, relativePath, pathToDelete);
                if (result.Cancel)
                {
                    LogHelper.Error(string.Format("Resource removing interrupted, reason: {0}", result.Reason));
                    return false;
                }
            }

            try
            {
                bool deleted = false;
                if (File.Exists(pathToDelete))
                    deleted = IOHelper.DeleteFile(pathToDelete);
                else if (Directory.Exists(pathToDelete))
                    deleted = IOHelper.DeleteDirectory(pathToDelete);
                if(!deleted)
                    return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error while deleting '{0}': {1}", pathToDelete, e.Message));
                return false;
            }

            //Plugin ResourcesRemovedFromWorkspace
            foreach (Sm4shBasePlugin plugin in _Plugins)
                plugin.InternalResourcesRemovedFromWorkspace(resCol, relativePath, pathToDelete);

            LogHelper.Info("Done!");

            return true;
        }
        #endregion

        #region Unlocalizing Files && Resource Removal
        #region internal methods
        internal bool UnlocalizePath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return UnlocalizePath(resCol, relativePath);
        }

        internal bool UnlocalizePath(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null || !resCol.IsRegion)
                return false;

            //Get list of resources to unlocalize
            List<ResourceItem> lItems = GetResources(resCol, relativePath);
            ResourceCollection dataCol = _resColDataCore;

            LogHelper.Info(string.Format("Unlocalizing {0} resources from {1}...", lItems.Count, relativePath));

            foreach (ResourceItem uItem in lItems)
            {
                if (!dataCol.Resources.ContainsKey(uItem.RelativePath))
                {
                    LogHelper.Warning(string.Format("The resource '{0}' does not exist in the data partition! Skipping it but it might cause an issue.", uItem.RelativePath));
                    continue;
                }

                ResourceItem packedItem = GetPackedPath(uItem.ResourceCollection, uItem.RelativePath);
                if (packedItem != null)
                    _CurrentProject.AddUnlocalized(resCol.PartitionName, packedItem.RelativePath + "packed");
                else
                    _CurrentProject.AddUnlocalized(resCol.PartitionName, uItem.RelativePath);
            }

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }

        internal bool RemoveUnlocalized(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return RemoveUnlocalized(resCol, relativePath);
        }

        internal bool RemoveUnlocalized(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null || !resCol.IsRegion)
                return false;

            LogHelper.Info(string.Format("Removing unlocalization for {0}...", relativePath));

            string[] paths = resCol.Resources.Keys.ToArray();
            foreach (string path in paths)
            {
                if (path.StartsWith(relativePath))
                {
                    ResourceItem dItem = resCol.Resources[path];
                    if (dItem.IsAPackage && dItem.IsFolder)
                        CurrentProject.RemoveUnlocalized(resCol.PartitionName, path + "packed");
                    else
                        CurrentProject.RemoveUnlocalized(resCol.PartitionName, path);
                }
            }

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }


        internal bool RemoveOriginalResource(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return UnlocalizePath(resCol, relativePath);
        }

        internal bool RemoveOriginalResource(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return false;

            //Get list of resources to remove
            List<ResourceItem> lItems = GetResources(resCol, relativePath);
            ResourceCollection dataCol = _resColDataCore;

            LogHelper.Info(string.Format("Removing {0} original resources from {1}...", lItems.Count, relativePath));

            foreach (ResourceItem uItem in lItems)
                _CurrentProject.RemoveOriginalResource(resCol.PartitionName, uItem.RelativePath);

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }

        internal bool ReintroduceOriginalResource(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return ReintroduceOriginalResource(resCol, relativePath);
        }

        internal bool ReintroduceOriginalResource(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return false;

            LogHelper.Info(string.Format("Reintroducing original resource for {0}...", relativePath));

            string[] paths = resCol.Resources.Keys.ToArray();
            foreach (string path in paths)
            {
                if (path.StartsWith(relativePath))
                    CurrentProject.ReintroduceOriginalResource(resCol.PartitionName, path);
            }

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }
        #endregion
        #endregion

        #region Packing Folder
        internal bool SetPackFlagResource(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return UnlocalizePath(resCol, relativePath);
        }

        internal bool SetPackFlagResource(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return false;

            LogHelper.Info(string.Format("Set flag to pack folder '{0}'. Make sure there is no packed subfolder. This folder will now be packed during rebuild.", relativePath));
            _CurrentProject.PackResource(resCol.PartitionName, relativePath);

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }

        internal bool UnsetPackFlagResource(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return false;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return UnsetPackFlagResource(resCol, relativePath);
        }

        internal bool UnsetPackFlagResource(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return false;

            LogHelper.Info(string.Format("Unset flag to pack folder '{0}'.", relativePath));

            CurrentProject.RemovePackResource(resCol.PartitionName, relativePath);

            SaveProject();

            LogHelper.Info("Done!");

            return true;
        }

        internal bool CanBePacked(string absolutePath)
        {
            ResourceItem selRes = GetResource(absolutePath);
            if (selRes != null)
            {
                if (selRes.IsFolder && selRes.OffInPack == 0 && !selRes.IsAPackage)
                {
                    List<ResourceItem> lItems = GetResources(absolutePath);
                    foreach (ResourceItem rItem in lItems)
                    {
                        if (rItem.OffInPack != 0)
                            return false;
                    }
                    return true;
                }
                return false;
            }
            else
            {
                string partition = GetResourceCollection(absolutePath).PartitionName;
                string[] relativePaths = GetAllWorkplaceRelativePaths(absolutePath);
                foreach (string relativePath in relativePaths)
                {
                    if (CurrentProject.IsResourceToBePacked(partition, relativePath))
                        return false;
                }
                return true;
            }
        }
        #endregion

        #region Rebuilding Files
        #region internal methods
        internal string RebuildRFAndPatchlist()
        {
            return RebuildRFAndPatchlist(true);
        }

        internal string RebuildRFAndPatchlist(bool packing)
        {
            LogHelper.Info("----------------------------------------------------------------");
            LogHelper.Info(string.Format("Starting compilation of the mod ({0})", (packing ? "release" : "debug")));

            string exportFolder = PathHelper.FolderExport + (packing ? "release" : "debug") + Path.DirectorySeparatorChar + (_CurrentProject.ExportWithDateFolder ? string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + Path.DirectorySeparatorChar : string.Empty);

            bool deleted = true;
            if (Directory.Exists(exportFolder))
            {
                foreach (string folder in Directory.GetDirectories(exportFolder))
                    if (!IOHelper.DeleteDirectory(folder))
                        deleted = false;
                foreach (string file in Directory.GetFiles(exportFolder))
                    if (!IOHelper.DeleteFile(file))
                        deleted = false;
            }

            if (!deleted)
            {
                LogHelper.Error(string.Format("Error deleting '{0}', please delete it manually before attempting to build the mod again.", exportFolder));
                return string.Empty;
            }

            try
            {
                LogHelper.Debug(string.Format("Export folder: '{0}'", exportFolder));
                string exportPatchFolder = exportFolder + "content" + Path.DirectorySeparatorChar + "patch" + Path.DirectorySeparatorChar;

                //Plugin NewModBuilding hook
                foreach (Sm4shBasePlugin plugin in _Plugins)
                {
                    PluginActionResult result = plugin.InternalNewModBuilding(exportFolder);
                    if (result.Cancel)
                    {
                        LogHelper.Error(string.Format("Build interrupted, reason: {0}", result.Reason));
                        return string.Empty;
                    }
                }

                //Get a map of the workspace folder
                //HashCollection htWorkspace = new HashCollection("ht_workspace", PathHelper.FolderWorkplace, null);

                //Copy resources, pack files if needed
                ResourceCollection dataResCol = null;
                foreach (ResourceCollection resCol in _resCols)
                {
                    LogHelper.Debug(string.Format("Rebuilding partition '{0}'...", resCol.PartitionName));

                    //Cloning to leave the original resourcecollection untouched
                    ResourceCollection newCol = (ResourceCollection)resCol.Clone();

                    //Build Export Files
                    BuildingExportFiles(newCol, exportPatchFolder, packing);

                    //Build Resource
                    LogHelper.Info(string.Format("Rebuilding '{0}'...", newCol.ResourceName));
                    ResourceCollection collection = null;
                    if (!newCol.IsRegion) //No region, take the resource file "as it"
                    {
                        collection = newCol;
                        dataResCol = newCol;
                    }
                    else
                        collection = GetMergedRegionResources(dataResCol, newCol);

                    RemoveOriginalResourcesFromPackage(newCol);

                    LogHelper.Debug(string.Format("Rebuilding resource file '{0}'...", resCol.ResourceName));

                    _RfManager.RebuildResourceFile(collection, exportPatchFolder);

                    //Save CSV
                    ExportCSV(resCol, collection, exportPatchFolder);
                }

                //Patchlist
                LogHelper.Info("Rebuilding 'patchlist'...");
                BuildPatchfile(exportPatchFolder);
            }
            catch(Exception e)
            {
                LogHelper.Error(string.Format("Error while building the mod: {0}", e.Message));
                return string.Empty;
            }

            //Plugin NewModBuilt hook
            foreach (Sm4shBasePlugin plugin in _Plugins)
                plugin.InternalNewModBuilt(exportFolder);

            LogHelper.Info(string.Format("Completed compilation of the mod ({0})", (packing ? "release" : "debug")));
            LogHelper.Info("----------------------------------------------------------------");

            return exportFolder;
        }
        #endregion

        #region private methods
        private void RemoveOriginalResourcesFromPackage(ResourceCollection resCol)
        {
            Sm4shModItem modItem = _CurrentProject.ResourcesToRemove.Find(p => p.Partition == resCol.PartitionName);
            if (modItem == null)
                return;

            foreach (string relativePath in modItem.Paths)
            {
                resCol.Resources.Remove(relativePath);
                LogHelper.Debug(string.Format("Removing resource '{0}' from partition '{1}'", relativePath, resCol.PartitionName));
            }
        }

        private ResourceCollection GetMergedRegionResources(ResourceCollection dataResCol, ResourceCollection resCol)
        {
            if (!resCol.IsRegion)
                throw new Exception("resCol should be a region collection");

            ResourceCollection outputResCol = new ResourceCollection(resCol.PartitionName);
            Dictionary<string, ResourceItem> resources = outputResCol.Resources;

            Dictionary<string, ResourceItem> mainResources = dataResCol.Resources;
            Dictionary<string, ResourceItem> regionResources = resCol.GetFilteredResources();
            string partition = resCol.PartitionName;

            foreach (ResourceItem mainRItem in mainResources.Values)
            {
                if (!resCol.Resources.ContainsKey(mainRItem.RelativePath) && mainRItem.RelativePath.EndsWith("_us_en.flx"))
                    continue;

                ResourceItem rItem = (ResourceItem)mainResources[mainRItem.RelativePath].Clone();
                rItem.ResourceCollection = outputResCol;
                if (regionResources.ContainsKey(mainRItem.RelativePath) && !CurrentProject.IsUnlocalized(partition, mainRItem.RelativePath))
                {
                    resources.Add(mainRItem.RelativePath, regionResources[mainRItem.RelativePath]);
                }
                else
                {
                    rItem.Source = FileSource.NotFound;
                    resources.Add(mainRItem.RelativePath, rItem);
                }
            }

            foreach (ResourceItem regionRItem in regionResources.Values)
            {
                if (!resources.ContainsKey(regionRItem.RelativePath))
                    resources.Add(regionRItem.RelativePath, regionRItem);
            }

            return outputResCol;
        }

        private void BuildingExportFiles(ResourceCollection resCol, string exportFolder, bool packing)
        {
            string baseFolder = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH, resCol.PartitionName);
            List<string> filesList = new List<string>();
            if (Directory.Exists(baseFolder))
            {
                filesList.AddRange(Directory.GetDirectories(baseFolder, "*", SearchOption.AllDirectories));
                filesList.AddRange(Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories));
                string[] filesToProcess = filesList.ToArray();
                Array.Sort(filesToProcess, new CustomStringComparer());
                if (filesToProcess.Length > 0)
                {
                    LogHelper.Info(string.Format("Packaging '{0}'... to '{1}'", resCol.PartitionName, exportFolder));
                    BuildNewResources(resCol, filesToProcess, exportFolder, packing);
                }
            }
        }

        private void BuildPatchfile(string exportFolder)
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
                    string pathToProcess = fileInExport.Replace(exportFolder, string.Empty).Replace(Path.DirectorySeparatorChar, '/');
                    if (!filesToAdd.Contains(pathToProcess))
                        filesToAdd.Add(pathToProcess);
                }

                //Files to remove
                foreach (Sm4shModItem rModItem in _CurrentProject.UnlocalizationItems)
                {
                    foreach (string pathToRemove in rModItem.Paths)
                        filesToRemove.Add(rModItem.Partition + "/" + pathToRemove);
                }
                foreach (Sm4shModItem rModItem in _CurrentProject.ResourcesToRemove)
                {
                    foreach (string pathToRemove in rModItem.Paths)
                     if(!filesToRemove.Contains(rModItem.Partition + "/" + pathToRemove))
                        filesToRemove.Add(rModItem.Partition + "/" + pathToRemove);
                }
                _RfManager.RebuildPatchListFile(filesToAdd.ToArray(), filesToRemove.ToArray(), exportFolder);
            }
        }

        private bool BuildNewResources(ResourceCollection resCol, string[] filesToProcess, string exportFolder, bool packing)
        {
            List<string> listFiles = new List<string>();
            foreach (string file in filesToProcess)
            {
                if (!Utils.IsAnAcceptedExtension(file))
                    LogHelper.Warning(string.Format("The file '{0}' has a forbidden extension, skipping...", file));
                else
                    listFiles.Add(file);
            }
            string[] files = listFiles.ToArray();

            string baseToExclude = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH, resCol.PartitionName);
            List<PackageProcessor> packNeedsRepacking = new List<PackageProcessor>();
            Sm4shModItem resourcesToPack = _CurrentProject.ResourcesToPack.Find(p => p.Partition == resCol.PartitionName);
            foreach (string file in files)
            {
                string relativePath = file.Replace(baseToExclude, string.Empty).Replace(Path.DirectorySeparatorChar, '/');

                //This ensure that we are dealing with a folder or a file.
                FileAttributes pathAttrs = File.GetAttributes(file);
                bool isFolder = false;
                if (pathAttrs.HasFlag(FileAttributes.Directory))
                {
                    relativePath += "/";
                    isFolder = true;
                }

                ResourceItem nItem = GetEditedResource(resCol, relativePath);

                //Force packed
                if (_CurrentProject.IsResourceToBePacked(resCol.PartitionName, relativePath))
                    nItem.IsAPackage = true;
                else if (_CurrentProject.IsResourceInPackage(resCol.PartitionName, relativePath))
                {
                    nItem.OffInPack = 1;
                     nItem.IsAPackage = false;
                }

                //Checking if part of a pack to repack, if yes, lets process it after with PackageProcessor
                ResourceItem resPacked = GetPackedPath(resCol, relativePath);
                if (resPacked != null && packNeedsRepacking.Find(p => p.PackedRelativePath == resPacked.RelativePath) == null)
                    packNeedsRepacking.Add(new PackageProcessor() { TopResource = resPacked, PackedRelativePath = resPacked.RelativePath, ExportFolder = exportFolder });
                if (resPacked != null)
                {
                    packNeedsRepacking.Find(p => p.PackedRelativePath == resPacked.RelativePath).FilesToAdd.Add(file);
                    continue;
                }

                if (!isFolder)
                {
                    uint cmpSize;
                    uint decSize;
                    byte[] fileToWrite;

                    GetFileBinary(nItem, file, out cmpSize, out decSize, out fileToWrite);

                    nItem.CmpSize = cmpSize;
                    nItem.DecSize = decSize;

                    //Copy to export folder
                    string savePath = exportFolder + resCol.PartitionName + Path.DirectorySeparatorChar + relativePath.Replace('/', Path.DirectorySeparatorChar);
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, fileToWrite);
                }
            }

            //packed management
            foreach (PackageProcessor package in packNeedsRepacking)
                ProcessPackage(package, packing);

            return true;
        }

        private ResourceItem GetPackedPath(ResourceCollection resCol, string relativePath)
        {
            if (resCol.Resources.ContainsKey(relativePath) && resCol.Resources[relativePath].IsAPackage && !resCol.Resources[relativePath].IsFolder)
                return null;

            while (!resCol.Resources.ContainsKey(relativePath) || !resCol.Resources[relativePath].IsAPackage)
            {
                int nextPathCalc = relativePath.LastIndexOf("/", relativePath.Length - 2);
                if (nextPathCalc == -1)
                    return null;
                relativePath = relativePath.Substring(0, nextPathCalc) + "/";
            }
            return resCol.Resources[relativePath];
        }

        private ResourceItem GetEditedResource(ResourceCollection resCol, string relativePath)
        {
            ResourceItem nItem = null;
            if (resCol.Resources.ContainsKey(relativePath))
            {
                nItem = resCol.Resources[relativePath];
                nItem.OriginalResourceItem = (ResourceItem)nItem.Clone();
            }
            else
            {
                string fileName = string.Empty;
                if (relativePath.EndsWith("/"))
                {
                    fileName = relativePath.Substring(0, relativePath.Length - 1);
                    fileName = fileName.Substring(fileName.LastIndexOf("/") + 1) + "/";
                }
                else
                    fileName = relativePath.Substring(relativePath.LastIndexOf("/") + 1);
                nItem = new ResourceItem(resCol, fileName, 0, 0, 0, false, relativePath);
                resCol.Resources.Add(relativePath, nItem);
                nItem.ResourceCollection = resCol;
            }

            //Common info
            if (nItem.PatchItem == null)
                nItem.PatchItem = new PatchFileItem(relativePath, resCol.PartitionName + "/" + relativePath, false);
            nItem.Source = FileSource.Mod;

            return nItem;
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

        private void GetFileBinary(ResourceItem rItem, string file, out uint cmpSize, out uint decSize, out byte[] fileToWrite)
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

        private bool CheckIfLastFileInFolder(string[] files, string path, int position)
        {
            for (int i = position + 1; i < files.Length; i++)
            {
                if (files[i].StartsWith(path))
                    return true;
            }
            return false;
        }
        #endregion
        #endregion

        #region Processing packed files
        #region private methods
        private void ProcessPackageUnpacked(ResourceItem rItemTopResource, string[] files, string exportFolder)
        {
            ResourceCollection resCol = rItemTopResource.ResourceCollection;
            string baseTempFolder = PathHelper.FolderTemp + resCol.PartitionName + Path.DirectorySeparatorChar;

            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];

                string resPath = file.Replace(baseTempFolder, string.Empty).Replace(Path.DirectorySeparatorChar, '/');
                bool isFolder = false;
                FileAttributes pathAttrs = File.GetAttributes(file);
                if (pathAttrs.HasFlag(FileAttributes.Directory))
                {
                    resPath += "/";
                    isFolder = true;
                }

                ResourceItem nItem = GetEditedResource(resCol, resPath);
                nItem.OffInPack = 0;
                uint cmpSize = 0;
                uint decSize = 0;

                string unpackedPath = exportFolder + resCol.PartitionName + Path.DirectorySeparatorChar + file.Replace(baseTempFolder, string.Empty);
                if (!isFolder)
                {
                    byte[] fileToWrite;

                    GetFileBinary(nItem, file, out cmpSize, out decSize, out fileToWrite);

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
            CleanTempFolder();

            //Extract all files
            ExtractFolder(package.TopResource.ResourceCollection, package.PackedRelativePath, PathHelper.FolderTemp);

            //Override with new files
            if (package.FilesToAdd != null)
            {
                string baseToExclude = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH);
                foreach (string file in package.FilesToAdd)
                {
                    string savePath = file.Replace(baseToExclude, PathHelper.FolderTemp);//baseTempFolder + newPath.Replace("/", Path.DirectorySeparatorChar);
                    FileAttributes pathAttrs = File.GetAttributes(file);
                    if (pathAttrs.HasFlag(FileAttributes.Directory))
                        Directory.CreateDirectory(savePath);
                    else
                        IOHelper.CopyFile(file, savePath);
                }
            }

            //Sort files with ordinal comparison
            string baseFolder = PathHelper.FolderTemp + package.PackedAbsolutePath.Replace('/', Path.DirectorySeparatorChar);
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
                LogHelper.Info(string.Format("Packing '{0}' with {1} new files...", package.PackedAbsolutePath, package.FilesToAdd.Count));
                using (MemoryStream streamPackage = new MemoryStream())
                {
                    using (BinaryWriter writerPackage = new BinaryWriter(streamPackage))
                    {
                        ResourceItem[] currentFolder = new ResourceItem[20];
                        string[] currentFolderFile = new string[20];

                        string baseToExclude = PathHelper.FolderTemp + resCol.PartitionName + Path.DirectorySeparatorChar;
                        for (int f = 0; f < files.Length; f++)
                        {
                            uint currentPosition = (uint)streamPackage.Position;
                            string file = files[f];
                            uint paddingNeeded = 0;

                            string resPath = file.Replace(baseToExclude, string.Empty).Replace(Path.DirectorySeparatorChar, '/');
                            string fileName = Path.GetFileName(file);
                            FileAttributes pathAttrs = File.GetAttributes(file);
                            bool isFolder = pathAttrs.HasFlag(FileAttributes.Directory);
                            if (isFolder)
                                resPath += "/";

                            ResourceItem nItem = GetEditedResource(resCol, resPath);
                            //nItem.OverridePackedFile = false;
                            int folderDepth = (int)nItem.FolderDepth;
                            uint cmpSize = 0;
                            uint decSize = 0;
                            nItem.IsAPackage = f == 0;

                            if (!isFolder)
                            {
                                byte[] fileToWrite;

                                GetFileBinary(nItem, file, out cmpSize, out decSize, out fileToWrite);

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
                                currentFolderFile[folderDepth] = file + Path.DirectorySeparatorChar;
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
                    string packedPath = package.ExportFolder + files[0].Replace(PathHelper.FolderTemp, string.Empty) + Path.DirectorySeparatorChar + "packed";
                    Directory.CreateDirectory(Path.GetDirectoryName(packedPath));
                    File.WriteAllBytes(packedPath, streamPackage.ToArray());
                }
            }

            CleanTempFolder();
        }
        #endregion
        #endregion

        #region SD sending
        internal string GetSDFolder()
        {
            if (!string.IsNullOrEmpty(_CachedSDPath) && Directory.Exists(_CachedSDPath))
                return _CachedSDPath;

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable && (drive.DriveFormat == "FAT32" || drive.DriveFormat == "FAT"))
                {
                    //SDCafiine new
                    string sdCafiineFolder = string.Format("{0}sdcafiine{1}", drive.Name, Path.DirectorySeparatorChar);
                    if (Directory.Exists(sdCafiineFolder))
                    {
                        string sdCafiineNewFolder = string.Format("{0}{1}{2}content{2}", sdCafiineFolder, _CurrentProject.GameFullID, Path.DirectorySeparatorChar);
                        Directory.CreateDirectory(sdCafiineNewFolder);
                        if (Directory.Exists(sdCafiineNewFolder))
                        {
                            _CachedSDPath = sdCafiineNewFolder;
                            return sdCafiineNewFolder;
                        }
                    }

                    //SDCafiine old
                    string sdCafiineOldFolder = string.Format("{0}{1}{2}", drive.Name, _CurrentProject.GameFullID, Path.DirectorySeparatorChar);
                    if (Directory.Exists(sdCafiineOldFolder))
                    {
                        _CachedSDPath = sdCafiineOldFolder;
                        return sdCafiineOldFolder;
                    }

                    //Loadiine
                    string sdLoadiineGameFolder = string.Format("{0}wiiu{1}games{1}", drive.Name, Path.DirectorySeparatorChar);
                    if (Directory.Exists(sdLoadiineGameFolder))
                    {
                        string sdLoadiineID = "[" + _CurrentProject.GameID + "]";
                        foreach (string folder in Directory.GetDirectories(sdLoadiineGameFolder))
                        {
                            if (folder.Contains(sdLoadiineID))
                            {
                                _CachedSDPath = folder + Path.DirectorySeparatorChar;
                                return folder + Path.DirectorySeparatorChar;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        internal void SendToSD(string exportFolder)
        {
            string sdCardPath = GetSDFolder();
            if(string.IsNullOrEmpty(sdCardPath))
            {
                LogHelper.Warning("No SD card containing either an installation of Loadiine or SDCafiine for Sm4sh was found.");
                return;
            }

            SDMode sdMode = SDMode.SDCafiine;
            if (sdCardPath.Contains(string.Format("wiiu{0}games{0}", Path.DirectorySeparatorChar)))
                sdMode = SDMode.Loadiine;
            SendToSD(sdMode, exportFolder, sdCardPath);
        }

        internal void SendToSD(SDMode sdMode, string exportFolder, string sdFolder)
        {
            LogHelper.Info("----------------------------------------------------------------");
            LogHelper.Info(string.Format("{0}: Copying '{1}' to SD ('{2}')", sdMode, exportFolder, sdFolder));

            try
            {
                LogHelper.Info(string.Format("{0}: Calculating CRC32 values. If this is the first time with this SD card, this operation can take up to 3-8 minutes...", sdMode));

                string[] filters = new string[] { "patch", "sound" };

                //crc values export
                string modContentFolder = exportFolder + "content" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(modContentFolder))
                {
                    LogHelper.Info("No mod found, please select a directory that has an exported mod.");
                    return;
                }
                HashCollection modContentHT = new HashCollection("ht_mod_content", modContentFolder, filters);

                //crc values sd
                string sdContentFolder = sdFolder + (sdMode == SDMode.Loadiine ? "content" + Path.DirectorySeparatorChar : string.Empty);
                HashCollection sdContentHT = new HashCollection("ht_sd_content", sdContentFolder, filters);

                //crc values official, to replace any that should be replaced.
                string gameContentFolder = PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT);
                HashCollection gameContentHT = new HashCollection("ht_game_content", gameContentFolder, filters);

                //With Loadiine, the whole game (and patch) must be on the SD, then the mod.
                if (sdMode == SDMode.Loadiine)
                {
                    //Cleanup - If a file isnt on the sd, or is on the SD BUT isnt official AND isnt part of the upcoming mod, it needs to be replace
                    LogHelper.Info(string.Format("{0}: Adding patch files or replacing previously modded patch files to SD...", sdMode));
                    int i = 0;
                    foreach (HashEntity hEntity in gameContentHT)
                    {
                        if (modContentHT[hEntity.Key] == null && (sdContentHT[hEntity.Key] == null || sdContentHT[hEntity.Key].Crc32 != hEntity.Crc32))
                        {
                            IOHelper.CopyFile(gameContentFolder + hEntity.Key, sdContentFolder + hEntity.Key);
                            i++;
                        }
                    }
                    LogHelper.Info(string.Format("{0}: {1} file(s) were not official patch files and needed to be replaced in SD", sdMode, i));
                }
                else if (sdMode == SDMode.SDCafiine)
                {
                    //Cleanup: All non mod files need to be deleted from the SD to not override the original patch files
                    LogHelper.Info(string.Format("{0}: Removing previously modded patch files from SD...", sdMode));
                    int i = 0;
                    foreach (HashEntity hEntity in gameContentHT)
                    {
                        if (sdContentHT[hEntity.Key] != null && modContentHT[hEntity.Key] == null)
                        {
                            IOHelper.DeleteFile(sdContentFolder + hEntity.Key);
                            i++;
                        }
                    }
                    LogHelper.Info(string.Format("{0}: {1} file(s) were not official patch files and needed to be removed from SD", sdMode, i));
                }

                //Copy mod files if needed
                LogHelper.Info(string.Format("{0}: Adding mod files to SD...", sdMode));
                int j = 0;
                foreach (HashEntity hEntity in modContentHT)
                {
                    if (sdContentHT[hEntity.Key] == null || sdContentHT[hEntity.Key].Crc32 != hEntity.Crc32)
                    {
                        IOHelper.CopyFile(modContentFolder + hEntity.Key, sdContentFolder + hEntity.Key);
                        j++;
                    }
                }
                LogHelper.Info(string.Format("{0}: {1} mod file(s) were added to SD.", sdMode, j));
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error while sending files to SD: {0}", e.Message));
                return;
            }
            //No need to delete non related files

            LogHelper.Info(string.Format("{0}: Operation completed. Please check the io.txt logs if you encountered any issue.", sdMode));
            LogHelper.Info("----------------------------------------------------------------");
        }
        #endregion

        #region Plugin Management
        #region internal methods
        internal void LoadPlugins()
        {
            try
            {
                LogHelper.Info("Load plugins...");
                List<Sm4shBasePlugin> plugins = new List<Sm4shBasePlugin>();
                foreach (string pluginFile in Directory.GetFiles(PathHelper.FolderPlugins, "sm4shplugin_*.dll", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        byte[] AssmBytes = File.ReadAllBytes(pluginFile);
                        Assembly a = Assembly.Load(AssmBytes);
                        Type myType = a.GetTypes().FirstOrDefault(p => p.BaseType.Name == "Sm4shBasePlugin");

                        // Create an instance.
                        Sm4shBasePlugin plugin = (Activator.CreateInstance(myType, this)) as Sm4shBasePlugin;

                        if (plugin != null && plugin.InternalCanBeLoaded())
                        {
                            plugins.Add(plugin);
                            plugin.Filename = Path.GetFileName(pluginFile);
                            plugin.InternalOnLoad();
                            LogHelper.Debug(string.Format("Plugin '{0}' loaded!", plugin.Name));
                        }

                    }
                    catch (Exception e)
                    {
                        //not a plugin
                        LogHelper.Error(string.Format("Error loading plugin '{0}', Reason: {1}", pluginFile, e.Message));
                    }
                }

                //Reorder
                _Plugins = new BindingList<Sm4shBasePlugin>();
                foreach (string pluginFile in _CurrentProject.PluginsOrder)
                {
                    Sm4shBasePlugin plugin = plugins.Find(p => p.Filename.ToLower() == pluginFile.ToLower());
                    if (plugin != null)
                        _Plugins.Add(plugin);
                }
                foreach(Sm4shBasePlugin pluginObj in plugins)
                {
                    if(!_Plugins.Contains(pluginObj))
                        _Plugins.Add(pluginObj);
                }
            }
            catch(Exception e)
            {
                LogHelper.Error(string.Format("Error while loading the plugins: {0}", e.Message));
            }
}
        #endregion
        #endregion

        #region CSV Generation
        #region private methods
        private void ExportCSV(ResourceCollection originalResCol, ResourceCollection newResCol, string exportFolder)
        {
            if (_CurrentProject.ExportCSVList)
            {
                List<string> outputCSV = new List<string>();
                outputCSV.Add(GetHeader());

                foreach (ResourceItem nItem in newResCol.Resources.Values)
                {
                    if (!originalResCol.Resources.ContainsKey(nItem.RelativePath))
                    {
                        outputCSV.Add(GetCSVLine(null, nItem));
                        continue;
                    }

                    ResourceItem oItem = originalResCol.Resources[nItem.RelativePath];

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
                if (!Directory.Exists(exportFolder + "~csv"))
                    Directory.CreateDirectory(exportFolder + "~csv");
                File.WriteAllLines(exportFolder + "~csv" + Path.DirectorySeparatorChar + "~" + newResCol.ResourceName + ".csv", outputCSV);
            }
        }

        private string GetHeader()
        {
            List<string> headerParts = new List<string>();
            headerParts.Add("Path");
            headerParts.Add("New decompSize");
            headerParts.Add("Old decompSize");
            headerParts.Add("New compSize");
            if (!_CurrentProject.ExportCSVIgnoreCompSize)
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
            lineParts.Add(newItem.RelativePath);

            //DecSize
            lineParts.Add(newItem.DecSize.ToString());
            if (oldItem == null)
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
        #endregion
        #endregion CSV Generation

        #region Tools Methods
        #region public methods
        /// <summary>
        /// Get the reference of the ResourceCollection associated to an absolutepath (data/path/to/resource)
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>ResourceCollection instance</returns>
        public ResourceCollection GetResourceCollection(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return null;
            string resColName = absolutePath.Substring(0, absolutePath.IndexOf('/'));
            return _resCols.First(p => p.PartitionName == resColName);
        }

        /// <summary>
        /// Get the reference of the ResourceItem associated to an absolutepath (data/path/to/resource)
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>ResourceItem object, if found</returns>
        public ResourceItem GetResource(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return null;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return GetResource(resCol, relativePath);
        }

        /// <summary>
        /// Get the reference of the ResourceItem associated to an relativepath (path/to/resource) and a Resource Collection instance
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <returns>ResourceItem object, if found</returns>
        public ResourceItem GetResource(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return null;
            if (resCol.Resources.ContainsKey(relativePath))
                return resCol.Resources[relativePath];
            return null;
        }

        /// <summary>
        /// Get the reference of the ResourceItems associated to an absolutepath (data/path/to/resource)
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>ResourceItem list of objects, if found</returns>
        public List<ResourceItem> GetResources(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return null;
            string relativePath = GetRelativePath(absolutePath);
            ResourceCollection resCol = GetResourceCollection(absolutePath);
            return GetResources(resCol, relativePath);
        }

        /// <summary>
        /// Get the reference of the ResourceItems associated to an relativepath (path/to/resource) and a Resource Collection instance
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <returns>ResourceItem list of objects, if found</returns>
        public List<ResourceItem> GetResources(ResourceCollection resCol, string relativePath)
        {
            if (resCol == null)
                return new List<ResourceItem>();
            List<ResourceItem> nItems = new List<ResourceItem>();
            foreach (ResourceItem rItem in resCol.CachedFilteredResources.Values)
            {
                if (rItem.RelativePath.StartsWith(relativePath))
                    nItems.Add(rItem);
            }
            return nItems;
        }

        /// <summary>
        /// Get a physical file to a resource in the workspace, given an absolutepath (data/path/to/resource)
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>Path to a file in the filesystem</returns>
        public string GetWorkspaceFileFromPath(string absolutePath)
        {
            return PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH) + absolutePath.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Get a list of resources in the workspace given given an absolutepath (data/path/to/resource), will look for children folders too.
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>List of relativepaths of the resources found</returns>
        public string[] GetAllWorkplaceRelativePaths(string absolutePath)
        {
            return GetAllWorkplaceRelativePaths(absolutePath, true);
        }

        /// <summary>
        /// Get a list of resources in the workspace given given an absolutepath (data/path/to/resource), with the option to look for children too.
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <param name="recursive">True if you want to look in every level folders</param>
        /// <returns>List of relativepaths of the resources found</returns>
        public string[] GetAllWorkplaceRelativePaths(string absolutePath, bool recursive)
        {
            if (absolutePath.EndsWith("/"))
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 1);
            absolutePath = absolutePath.Replace('/', Path.DirectorySeparatorChar);
            string baseToRemove = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_PATCH, absolutePath);
            if (Directory.Exists(baseToRemove))
            {
                List<string> pathResources = new List<string>();
                List<string> pathResourcesToReturn = new List<string>();
                pathResources.AddRange(Directory.GetFiles(baseToRemove, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                pathResources.AddRange(Directory.GetDirectories(baseToRemove, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                for (int i = 0; i < pathResources.Count; i++)
                {
                    if (!Utils.IsAnAcceptedExtension(pathResources[i]))
                        continue;
                    FileAttributes pathAttrs = File.GetAttributes(pathResources[i]);
                    pathResources[i] = pathResources[i].Replace(baseToRemove, string.Empty).Replace(Path.DirectorySeparatorChar, '/');
                    if (pathAttrs.HasFlag(FileAttributes.Directory) && !pathResources[i].EndsWith("/"))
                        pathResources[i] += "/";
                    pathResourcesToReturn.Add(pathResources[i]);
                }
                return pathResourcesToReturn.ToArray();
            }
            return new string[0];
        }

        /// <summary>
        /// Delete everything in the temp folder
        /// </summary>
        public void CleanTempFolder()
        {
            if (_CurrentProject != null)
                IOHelper.DeleteDirectory(PathHelper.FolderTemp);
        }
        #endregion

        #region private methods
        private bool CheckDTFiles()
        {
            if ((!_CurrentProject.Is3DS && (!File.Exists(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT) + "dt00") ||
                !File.Exists(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT) + "dt01"))) || (_CurrentProject.Is3DS && !File.Exists(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT) + "dt")))
            {
                LogHelper.Warning("Missing DT Files, you will not be able to extract from LS.");
                return false;
            }
            //TODO CHECK FILESIZE 3DS
            //Check filesize per region
            if (!_CurrentProject.Is3DS)
            {
                FileInfo dt00 = new FileInfo(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT) + "dt00");
                FileInfo dt01 = new FileInfo(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_CONTENT) + "dt01");

                if (!_CurrentProject.CheckDTSizeWiiU(dt00.Length, dt01.Length))
                {
                    LogHelper.Warning(string.Format("The filesize of dt00/dt01 doesn't match the region set in your config ({0}). You will not be able to extract from LS.", _CurrentProject.GameRegion));
                    string guessedRegionName = _CurrentProject.GuessRegionFromDTFiles(dt00.Length, dt01.Length);
                    if (!string.IsNullOrEmpty(guessedRegionName))
                        LogHelper.Info(string.Format("It seems that the size of your dt00/dt01 files matches the ({0}) region. You might want to edit your config file: 1 is for JPN, 2 is for USA, 4 is for EUR.", guessedRegionName));
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a relative path of a resource given an absolutepath (data/path/to/resource)
        /// </summary>
        /// <param name="absolutePath">data/path/to/resource</param>
        /// <returns>relativepath (path/to/resource)</returns>
        public string GetRelativePath(string absolutePath)
        {
            if (absolutePath.Contains("data"))
                return absolutePath.Substring(absolutePath.IndexOf("/") + 1);
            return absolutePath;
        }

        /// <summary>
        /// Get an absolute path of a resource given a relativepath (path/to/resource) and a ResourceCollection instance
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <returns>absolutepath (data/path/to/resource)</returns>
        public string GetAbsolutePath(ResourceCollection resCol, string relativePath)
        {
            return resCol.PartitionName + "/" + relativePath;
        }

        private List<string> FilterRelativePath(string[] paths, string relativePath)
        {
            List<string> files = new List<string>();
            foreach (string path in paths)
            {
                if (path.StartsWith(relativePath))
                    files.Add(path);
            }
            return files;
        }
        #endregion
        #endregion
    }
}
