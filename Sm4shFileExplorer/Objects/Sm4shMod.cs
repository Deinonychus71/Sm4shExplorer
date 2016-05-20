using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sm4shFileExplorer.Objects
{
    public class Sm4shMod
    {
        public string GamePath { get; set; }
        public string ProjectExportFolder { get; set; }
        public string ProjectExtractFolder { get; set; }
        public string ProjectTempFolder { get; set; }
        public string ProjectWorkplaceFolder { get; set; }

        public string ProjectHexEditorFile { get; set; }
        public bool SkipJunkEntries { get; set; } //Default value is true, false will keep the empty folder and empty files.
        public bool Debug { get; set; }
        public bool KeepOriginalFlags { get; set; } //Default value is false, true will force the original flags of the resource (if they exist)
        public bool ExportCSVList { get; set; }
        public bool ExportCSVIgnoreFlags { get; set; }
        public bool ExportCSVIgnorePackOffsets { get; set; }
        public bool ExportCSVIgnoreCompSize { get; set; }
        public bool ExportWithDateFolder { get; set; }

        public List<Sm4shModItem> UnlocalizationItems { get; set; }
        public List<Sm4shModItem> ResourcesToRemove { get; set; }
        public List<string> PluginsOrder { get; set; }

        public bool Is3DS { get; set; } //TODO SUPPORT
        public int GameVersion { get; set; }
        public int GameRegionID { get; set; }

        [XmlIgnore]
        public string GameRegion { get { return GetRegionName(); } }
        [XmlIgnore]
        public string GameID { get { return GetGameID(); } }
        [XmlIgnore]
        public bool DTFilesFound { get; internal set; }


        #region Unlocalize
        internal void AddUnlocalized(string partition, string relativePath)
        {
            AddSm4shModItem(UnlocalizationItems, partition, relativePath);
        }

        internal void RemoveUnlocalized(string partition, string relativePath)
        {
            RemoveSm4shModItem(UnlocalizationItems, partition, relativePath);
        }

        internal bool IsUnlocalized(string partition, string relativePath)
        {
            if (partition == "data")
                return false;
            return IsSm4shModItem(UnlocalizationItems, partition, relativePath);
        }
        #endregion

        #region Resource Removal
        internal void RemoveOriginalResource(string partition, string relativePath)
        {
            AddSm4shModItem(ResourcesToRemove, partition, relativePath);
        }

        internal void ReintroduceOriginalResource(string partition, string relativePath)
        {
            RemoveSm4shModItem(ResourcesToRemove, partition, relativePath);
        }

        internal bool IsResourceRemoved(string partition, string relativePath)
        {
            return IsSm4shModItem(ResourcesToRemove, partition, relativePath);
        }
        #endregion

        #region Generic
        private void AddSm4shModItem(List<Sm4shModItem> list, string partition, string relativePath)
        {
            if (list == null)
                list = new List<Sm4shModItem>();

            Sm4shModItem resCol = list.Find(p => p.Partition == partition);
            if (resCol == null)
            {
                resCol = new Sm4shModItem() { Partition = partition, Paths = new List<string>() };
                list.Add(resCol);
            }
            string checkPathExist = resCol.Paths.Find(p => p == relativePath);
            if (string.IsNullOrEmpty(checkPathExist))
                resCol.Paths.Add(relativePath);
        }

        private void RemoveSm4shModItem(List<Sm4shModItem> list, string partition, string relativePath)
        {
            if (list == null)
                list = new List<Sm4shModItem>();

            Sm4shModItem resCol = list.Find(p => p.Partition == partition);
            if (resCol == null)
                return;
            resCol.Paths.Remove(relativePath);
        }

        private bool IsSm4shModItem(List<Sm4shModItem> list, string partition, string relativePath)
        {
            if (list == null)
                list = new List<Sm4shModItem>();

            Sm4shModItem resCol = list.Find(p => p.Partition == partition);
            if (resCol == null)
                return false;

            string pathFound = resCol.Paths.Find(p => (p.EndsWith("packed") && relativePath.StartsWith(p.Substring(0, p.LastIndexOf("packed"))) || p == relativePath));
            if (!string.IsNullOrEmpty(pathFound))
                return true;
            return false;
        }
        #endregion

        private string GetRegionName()
        {
            return GetRegionName(GameRegionID);
        }

        private string GetRegionName(int regionID)
        {
            switch (GameRegionID)
            {
                case 1:
                    return "JAP";
                case 2:
                    return "USA";
                case 3:
                    return "EUR";
            }
            return "???";
        }

        private string GetGameID()
        {
            switch (GameRegionID)
            {
                case 1:
                    return "AXFJ01";
                case 2:
                    return "AXFE01";
                case 3:
                    return "AXFP01";
            }
            return "???";
        }

        private bool CheckDTSize3DS(int dtsize)
        {
            return false;//TODO
        }

        internal bool CheckDTSizeWiiU(long dt00size, long dt01size)
        {
            return CheckDTSizeWiiU(dt00size, dt01size, GameRegionID);
        }

        internal bool CheckDTSizeWiiU(long dt00size, long dt01size, int regionID)
        {
            switch (regionID)
            {
                case 1:
                    if(dt00size == 4082912512 && dt01size == 2398254253) //JAP
                        return true;
                    break;
                case 2:
                    if(dt00size == 4083470592 && dt01size == 2409697069) //USA
                        return true;
                    break;
                case 3:
                    if (dt00size == 4085073920 && dt01size == 4038462509) //EUR
                        return true;
                    break;
            }
            return false;
        }

        internal string GuessRegionFromDTFiles(long dt00size, long dt01size)
        {
            if (CheckDTSizeWiiU(dt00size, dt01size, 1))
                return GetRegionName(1);
            if (CheckDTSizeWiiU(dt00size, dt01size, 2))
                return GetRegionName(2);
            if (CheckDTSizeWiiU(dt00size, dt01size, 3))
                return GetRegionName(3);
            return string.Empty;
        }
    }

    public class Sm4shModItem
    {
        public string Partition { get; set; }
        public List<string> Paths { get; set; }
    }
}
