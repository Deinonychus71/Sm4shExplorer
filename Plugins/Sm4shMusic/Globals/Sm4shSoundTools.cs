using Sm4shFileExplorer.Globals;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sm4shMusic.Globals
{
    public class Sm4shSoundTools
    {
        #region Sound Files
        private static List<string> _SoundFiles;

        public static List<string> SoundFiles { get { if (_SoundFiles == null) RefreshSoundFiles(); return _SoundFiles; } }

        public static void RefreshSoundFiles()
        {
            if (_SoundFiles == null)
                _SoundFiles = new List<string>();
            else
                _SoundFiles.Clear();

            //Original BGMs
            foreach (string file in Directory.GetFiles(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM), "*.nus3bank", SearchOption.TopDirectoryOnly))
                _SoundFiles.Add(Path.GetFileNameWithoutExtension(file).Replace("snd_bgm_", string.Empty));

            //Addon BGMS
            string workplaceBGMs = PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_SOUND_BGM);
            if (!Directory.Exists(workplaceBGMs))
                Directory.CreateDirectory(workplaceBGMs);

            foreach (string file in Directory.GetFiles(workplaceBGMs, "snd_bgm_*.nus3bank", SearchOption.TopDirectoryOnly))
            {
                string formated = Path.GetFileNameWithoutExtension(file).Replace("snd_bgm_", string.Empty);
                if (formated.Length <= 0x3c) //Limit in property file
                {
                    if (!_SoundFiles.Contains(formated))
                        _SoundFiles.Add(formated);
                }
            }
        }
        #endregion

        private static  int GetGroupFor3(string base36label)
        {
            int group = 0;
            if (base36label.Length > 3)
                throw new Exception("Label can have up to 3 characters");
            for (int i = 0; i < 3; i++)
                group += (int)(Math.Pow(59, i)) * (int)(base36label[base36label.Length - 1 - i]);
            return group % 101;
        }
    }
}
