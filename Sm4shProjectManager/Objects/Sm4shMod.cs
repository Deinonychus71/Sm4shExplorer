using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sm4shProjectManager.Objects
{
    public class Sm4shMod
    {
        public string GamePath { get; set; }
        public string ProjectExportPath { get; set; }
        public string ProjectExtractPath { get; set; }
        public string ProjectTempPath { get; set; }
        public string ProjectWorkplacePath { get; set; }

        public string ProjectHexEditorPath { get; set; }
        public bool SkipJunkEntries { get; set; } //Default value is true, false will keep the empty folder and empty files.
        public bool Debug { get; set; }
        public bool KeepOriginalFlags { get; set; } //Default value is false, true will force the original flags of the resource (if they exist)
        public bool ExportCSVList { get; set; }
        public bool ExportCSVIgnoreFlags { get; set; }
        public bool ExportCSVIgnorePackOffsets { get; set; }
        public bool ExportCSVIgnoreCompSize { get; set; }
        public bool ExportWithDateFolder { get; set; }

        public List<Sm4shModItem> Items { get; set; }

        [XmlIgnore]
        public string GameID { get; set; }
        [XmlIgnore]
        public string GameVersion { get; set; }
        [XmlIgnore]
        public string GameRegion { get; set; }

        public List<string> UnlocalizedPaths { get; set; }

        public void AddUnlocalized(string region, string path)
        {
            if (Items == null)
                Items = new List<Sm4shModItem>();

            Sm4shModItem resCol = Items.Find(p => p.Region == region);
            if (resCol == null)
            {
                resCol = new Sm4shModItem() { Region = region, UnlocalizedPaths = new List<string>() };
                Items.Add(resCol);
            }
            string checkPathExist = resCol.UnlocalizedPaths.Find(p => p == path);
            if (string.IsNullOrEmpty(checkPathExist))
                resCol.UnlocalizedPaths.Add(path);
        }

        public void RemoveUnlocalized(string region, string path)
        {
            if (Items == null)
                Items = new List<Sm4shModItem>();

            Sm4shModItem resCol = Items.Find(p => p.Region == region);
            if (resCol == null)
                return;
            resCol.UnlocalizedPaths.Remove(path);
        }

        public bool IsUnlocalized(string region, string path)
        {
            if (string.IsNullOrWhiteSpace(region))
                return false;

            if (Items == null)
                Items = new List<Sm4shModItem>();

            Sm4shModItem resCol = Items.Find(p => p.Region == region);
            if (resCol == null)
                return false;

            string pathFound = resCol.UnlocalizedPaths.Find(p => (p.EndsWith("packed") && path.StartsWith(p.Substring(0, p.LastIndexOf("packed"))) || p == path));
            if (!string.IsNullOrEmpty(pathFound))
                return true;
            return false;
        }
    }

    public class Sm4shModItem
    {
        public string Region { get; set; }
        public List<string> UnlocalizedPaths { get; set; }
    }
}
