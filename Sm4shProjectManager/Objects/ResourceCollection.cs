using Sm4shProjectManager.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sm4shProjectManager.Objects
{
    public class ResourceCollection
    {
        #region Members
        private string _region = string.Empty;
        #endregion

        #region Properties
        public Dictionary<string, ResourceItem> Resources
        {
            get;
            set;
        }
        public bool IsRegion
        {
            get
            { 
                if(!string.IsNullOrEmpty(_region))
                    return true; 
                return false;
            }
        }

        public string Region
        {
            get { return _region; }
        }
        #endregion


        public ResourceCollection()
        {
            Resources = new Dictionary<string, ResourceItem>();
        }

        public ResourceCollection(string region)
        {
            _region = region;
            Resources = new Dictionary<string, ResourceItem>();
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

        public Dictionary<string, ResourceItem> GetFilteredResources()
        {
            Dictionary<string, ResourceItem> filteredResourcesWithFolders = new Dictionary<string, ResourceItem>();

            foreach (string path in Resources.Keys)
            {
                ResourceItem rItem = Resources[path];
                if (rItem.Source != FileSource.NotFound)
                    filteredResourcesWithFolders.Add(path, rItem);
            }

            return filteredResourcesWithFolders;
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
