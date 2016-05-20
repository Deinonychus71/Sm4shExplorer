using Sm4shFileExplorer.Globals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sm4shFileExplorer.Objects
{
    public class ResourceCollection
    {
        #region Members
        private string _name = string.Empty;
        private Dictionary<string, ResourceItem> _CachedFilteredResources;
        #endregion

        #region Properties
        public List<ResourceItem> Nodes { get; set; }

        public Dictionary<string, ResourceItem> Resources
        {
            get;
            set;
        }
        public Dictionary<string, ResourceItem> CachedFilteredResources
        {
            get
            {
                if (_CachedFilteredResources == null)
                    return GetFilteredResources();
                return _CachedFilteredResources;
            }
        }
        public bool IsRegion
        {
            get
            { 
                if(_name != "data")
                    return true; 
                return false;
            }
        }

        public string PartitionName
        {
            get { return _name; }
        }

        public string ResourceName
        {
            get { return _name.Replace("data", "resource"); }
        }
        #endregion


        public ResourceCollection()
        {
            Resources = new Dictionary<string, ResourceItem>();
            Nodes = new List<ResourceItem>();
        }

        public ResourceCollection(string name)
        {
            _name = name;
            Resources = new Dictionary<string, ResourceItem>();
            Nodes = new List<ResourceItem>();
        }

        public void SortResourcesDictionary()
        {
            Dictionary<string, ResourceItem> output = new Dictionary<string, ResourceItem>();

            string[] sortedPaths = Resources.Keys.ToArray();
            if(GlobalConstants.SORT_RESOURCE)
                Array.Sort(sortedPaths, new CustomStringComparer());
            foreach (string sortedPath in sortedPaths)
                output.Add(sortedPath, Resources[sortedPath]);

            Resources = output;
        }

        internal Dictionary<string, ResourceItem> GetFilteredResources()
        {
            Dictionary<string, ResourceItem> filteredResourcesWithFolders = new Dictionary<string, ResourceItem>();

            foreach (string path in Resources.Keys)
            {
                ResourceItem rItem = Resources[path];
                if (rItem.Source != FileSource.NotFound)
                    filteredResourcesWithFolders.Add(path, rItem);
            }

            _CachedFilteredResources = filteredResourcesWithFolders;
            return filteredResourcesWithFolders;
        }

        private ResourceItem GetResourceItemWithoutRegion(string resourcePath)
        {
            return Resources[this.PartitionName + resourcePath];
        }

        public object Clone()
        {
            object output = this.MemberwiseClone();
            ResourceCollection resCol = (ResourceCollection)output;
            resCol.Resources = resCol.Resources.ToDictionary(entry => entry.Key, entry => (ResourceItem)(entry.Value.Clone()));
            foreach (ResourceItem rItem in resCol.Resources.Values)
                rItem.ResourceCollection = resCol;
            return resCol;
        }
    }
}
