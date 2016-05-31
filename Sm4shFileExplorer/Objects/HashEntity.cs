using Sm4shFileExplorer.Globals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Collections;

namespace Sm4shFileExplorer.Objects
{
    public class HashCollection : IEnumerable<HashEntity>
    {
        private Dictionary<string, HashEntity> _HashEntityCollection;
        private string[] _FilterPaths;

        public string CollectionID { get; private set; }
        public string CSVPath { get { return PathHelper.FolderCache + "csv" + Path.DirectorySeparatorChar + CollectionID + ".csv"; } }
        public string Folder { get; private set; }

        public HashEntity this[string key]
        {
            get
            {
                if(_HashEntityCollection.ContainsKey(key))
                    return _HashEntityCollection[key];
                return null;
            }
        }

        public HashCollection(string collectionID, string folderToInspect, string[] filterPaths)
        {
            CollectionID = collectionID;
            Folder = folderToInspect;
            Dictionary<string, HashEntity> OldHashEntityCollection = new Dictionary<string, HashEntity>();
            _HashEntityCollection = new Dictionary<string, HashEntity>();
            _FilterPaths = filterPaths;

            string fileCache = CSVPath;
            bool needSave = true;
            if (File.Exists(fileCache))
            {
                try
                {
                    string[] lines = File.ReadAllLines(fileCache);
                    foreach (string line in lines)
                    {
                        string[] splitLine = line.Split(",".ToCharArray());
                        OldHashEntityCollection.Add(splitLine[0], new HashEntity(splitLine[0], DateTime.ParseExact(splitLine[2], "o", CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), Convert.ToUInt32(splitLine[1])));
                    }
                    needSave = false;
                }
                catch (Exception e)
                {
                    LogHelper.Error(string.Format("CSV parsing error, regenerating the hashtable for '{0}'...", fileCache));
                    LogHelper.Debug(e.Message);
                }
            }

            foreach (string file in Directory.GetFiles(folderToInspect, "*", SearchOption.AllDirectories))
            {
                string key = file.Substring(folderToInspect.Length);
                if (!IsValidKey(key))
                    continue;
                FileInfo fileInfo = new FileInfo(file);
                if (!OldHashEntityCollection.ContainsKey(key))
                    _HashEntityCollection.Add(key, new HashEntity(key, DateTime.MinValue));
                else
                    _HashEntityCollection.Add(key, OldHashEntityCollection[key]);
                if (_HashEntityCollection[key].LastWriteUtc != fileInfo.LastWriteTimeUtc)
                {
                    uint crc32 = Crc32.Compute(File.ReadAllBytes(file));
                    _HashEntityCollection[key].Crc32 = crc32;
                    _HashEntityCollection[key].LastWriteUtc = fileInfo.LastWriteTimeUtc;
                    needSave = true;
                }
            }

            if (needSave)
                Save();
        }

        private bool IsValidKey(string key)
        {
            if (!Utils.IsAnAcceptedExtension(key))
                return false;

            if (_FilterPaths != null)
            {
                foreach (string path in _FilterPaths)
                    if (key.StartsWith(path))
                        return true;
                return false;
            }

            return true;
        }

        public void Save()
        {
            List<string> linesToCSV = new List<string>();
            foreach (string key in _HashEntityCollection.Keys)
            {
                linesToCSV.Add(string.Format("{0},{1},{2}", key, _HashEntityCollection[key].Crc32, _HashEntityCollection[key].LastWriteUtc.ToString("o", CultureInfo.InvariantCulture)));
            }
            Directory.CreateDirectory(Path.GetDirectoryName(CSVPath));
            File.WriteAllLines(CSVPath, linesToCSV);
        }

        public IEnumerator<HashEntity> GetEnumerator()
        {
            return _HashEntityCollection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class HashEntity
    {
        public string Key { get; private set; }
        public uint Crc32 { get; set; }
        public DateTime LastWriteUtc { get; set; }

        public HashEntity(string key, DateTime lastWriteUtc, uint crc32)
        {
            Key = key;
            LastWriteUtc = lastWriteUtc;
            Crc32 = crc32;
        }

        public HashEntity(string key, DateTime lastWriteUtc)
        {
            Key = key;
            LastWriteUtc = lastWriteUtc;
        }
    }
}
