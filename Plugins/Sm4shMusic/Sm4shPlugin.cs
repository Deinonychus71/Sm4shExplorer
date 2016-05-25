﻿using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shMusic.DB;
using Sm4shMusic.Forms;
using Sm4shMusic.Globals;
using Sm4shMusic.Objects;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Sm4shFileExplorer.Objects;
using System;

namespace Sm4shMusic
{
    public class Sm4shPlugin : Sm4shBasePlugin
    {
        public const string VERSION = "0.6";

        #region Members
        private SoundEntryCollection _SoundEntryCollection;
        private PropertyFile _PropertyFile;
        private UISoundDBFile _UISoundDBFile;
        private MyMusicFile _MyMusicFile;
        private List<SoundMSBTFile> _SoundMSBTFiles;

        private Main _Main;
        #endregion

        #region Properties
        public override string Name
        {
            get { return "Sm4shMusic"; }
        }

        public override string Description
        {
            get { return "Add custom .nus3bank music to stages"; }
        }

        public override string Research
        {
            get { return "Soneek"; }
        }

        public override string GUI
        {
            get { return "deinonychus71"; }
        }

        public override string Version
        {
            get
            {
                return VERSION;
            }
        }

        public override string URL
        {
            get { return "https://github.com/Deinonychus71/Sm4shExplorer"; }
        }

        public override Bitmap[] Icons
        {
            get { return new Bitmap[] { Properties.Resources.icon_music }; }
        }
        #endregion

        public Sm4shPlugin(Sm4shProject project) 
            :base(project)
        {
        }

        private bool LaunchForm()
        {
            _SoundEntryCollection = new SoundEntryCollection();

            BGMFilesDB.InitializeBGMDB(Sm4shProject.CurrentProject.GameVersion, Sm4shProject.CurrentProject.Is3DS);
            BGMStageDB.InitializeBGMMyMusicDB(Sm4shProject.CurrentProject.GameVersion, Sm4shProject.CurrentProject.Is3DS);
            _PropertyFile = new PropertyFile(Sm4shProject, _SoundEntryCollection, "data/sound/config/bgm_property.mpb");
            _SoundEntryCollection.GenerateSoundEntriesBGMDictionary();
            _UISoundDBFile = new UISoundDBFile(Sm4shProject, _SoundEntryCollection, "data/param/ui/ui_sound_db.bin");
            _SoundEntryCollection.GenerateSoundEntriesDictionary();
            _MyMusicFile = new MyMusicFile(Sm4shProject, _SoundEntryCollection, "data/sound/config/bgm_mymusic.mmb");

            //Generation SoundMSBT dictionaries
            _SoundMSBTFiles = new List<SoundMSBTFile>();
            foreach (ResourceCollection resCol in Sm4shProject.ResourceDataCollection)
            {
                if (!resCol.CachedFilteredResources.ContainsKey("ui/message/sound.msbt"))
                    continue;
                SoundMSBTFile soundMSBTFile = new SoundMSBTFile(Sm4shProject, _SoundEntryCollection, resCol.PartitionName + "/ui/message/sound.msbt");
                _SoundMSBTFiles.Add(soundMSBTFile);
            }

            //Generate Dictionaries
            _SoundEntryCollection.GenerateStagesDictionaries();

            //Check stage sound integrity between MyMusic Stages and SoundDB Stages.
            //General rule: All BGMS present in MyMusic stages must be in SoundDB, the opposite isnt true.
            foreach (MyMusicStage myMusicStage in _SoundEntryCollection.MyMusicStages)
            {
                if (myMusicStage.BGMStage.BGMDBID != null)
                {
                    //From MyMusic
                    List<BGMEntry> bgmsMyMusic = new List<BGMEntry>();
                    foreach (MyMusicStageBGM myMusicStageBGM in myMusicStage.BGMs)
                    {
                        //if(myMusicStageBGM.unk4 == 0x0) //Filter songs
                        bgmsMyMusic.Add(myMusicStageBGM.BGMEntry);
                    }

                    //From SoundDB
                    List<BGMEntry> bgmsSoundDB = new List<BGMEntry>();
                    foreach (SoundDBStageSoundEntry sDBStageSoundEntry in _SoundEntryCollection.SoundDBStagesPerID[(int)myMusicStage.BGMStage.BGMDBID].SoundEntries)
                    {
                        if (sDBStageSoundEntry.SoundEntry == null)
                        {
                            LogHelper.Warning(string.Format("SOUNDID '{0}' have an issue with one of its associated stages.", sDBStageSoundEntry.SoundID));
                            continue;
                        }
                        foreach (SoundEntryBGM sEntryBGM in sDBStageSoundEntry.SoundEntry.BGMFiles)
                            bgmsSoundDB.Add(sEntryBGM.BGMEntry);
                    }
                    //HACK FOR KK
                    foreach (SoundEntry sEntry in _SoundEntryCollection.SoundEntries)
                    {
                        if (!sEntry.InSoundTest)
                        {
                            foreach (SoundEntryBGM sEntryBGM in sEntry.BGMFiles)
                            {
                                if(sEntryBGM.BGMEntry != null)
                                    bgmsSoundDB.Add(sEntryBGM.BGMEntry);
                            }
                        }
                    }

                    //Compare
                    foreach (BGMEntry sEntryBGM in bgmsMyMusic)
                    {
                        if (!bgmsSoundDB.Contains(sEntryBGM))
                        {
                            //throw new Exception(string.Format("Error sound integrity between MyMusic Stages and SoundDB Stages for stage '{0}': '{1}' was not present.", myMusicStage.BGMStage.Label, sEntryBGM.BGMTitle));
                            LogHelper.Error(string.Format("Error sound integrity between MyMusic Stages and SoundDB Stages for stage '{0}': '{1}' was not present.", myMusicStage.BGMStage.Label, sEntryBGM.BGMTitle));
                        }
                    }
                }
            }

            //Launch Form
            _Main = new Forms.Main();
            _Main.SoundEntryCollection = _SoundEntryCollection;
            _Main.XMLLoaded += _Main_XMLLoaded;
            _Main.Initialize(Sm4shProject);

            if (_Main.ShowDialog(Application.OpenForms[0]) == DialogResult.OK)
                return true;
            return false;
        }

        private void _Main_XMLLoaded(object sender, EventArgs e)
        {
            _SoundEntryCollection = _Main.SoundEntryCollection;
            _PropertyFile.SoundEntryCollection = _SoundEntryCollection;
            _MyMusicFile.SoundEntryCollection = _SoundEntryCollection;
            _UISoundDBFile.SoundEntryCollection = _SoundEntryCollection;
            foreach(SoundMSBTFile msbtFile in _SoundMSBTFiles)
                msbtFile.SoundEntryCollection = _SoundEntryCollection;
        }

        #region Abstracts methods
        public override void OpenPluginMenu()
        {
            Sm4shProject.CleanTempFolder();
            if (LaunchForm())
                SavePlugin();
            Sm4shProject.CleanTempFolder();
        }

        protected void SavePlugin()
        {
            _SoundEntryCollection.CleanBGMDatabase(true);
            _PropertyFile.BuildFile();
            foreach (SoundMSBTFile soundMSBTFile in _SoundMSBTFiles)
                soundMSBTFile.BuildFile();
            _UISoundDBFile.BuildFile();
            _MyMusicFile.BuildFile();

            LogHelper.Info(Strings.INFO_COMPILED);
            _Main.Close();
        }

        public override void NewModBuilt(string exportFolder)
        {
            if (Directory.Exists(PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_SOUND_BGM)))
            {
                string exportPathSoundBGM = exportFolder + "content" + Path.DirectorySeparatorChar + "sound" + Path.DirectorySeparatorChar + "bgm" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(exportPathSoundBGM);
                foreach (string file in Directory.GetFiles(PathHelper.GetWorkplaceFolder(PathHelperEnum.FOLDER_SOUND_BGM)))
                    File.Copy(file, exportPathSoundBGM + Path.GetFileName(file), true);
            }
        }

        public override bool CanBeLoaded()
        {
            //3DS
            if (Sm4shProject.CurrentProject.Is3DS)
                return false;

            //Version
            if (Sm4shProject.CurrentProject.GameVersion < 208)
            {
                LogHelper.Warning("Cannot load plugin 'Sm4shMusic', Reason: The plugin requires the game to use at least version 208");
                return false;
            }

            //Files check

            if (!Directory.Exists(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM)) || Directory.GetFiles(PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM), "*.nus3bank", SearchOption.TopDirectoryOnly).Length < 593)
            {
                LogHelper.Warning(string.Format("For best compatibility with its features, Sm4shMusic needs to be able to access the bgm from the game folder in order to work. Please copy all the official nus3bank files (593 files) in '{0}'", PathHelper.GetGameFolder(PathHelperEnum.FOLDER_SOUND_BGM)));
                return true;
            }
            return true;
        }

        public override int CanResourceBeLoaded(ResourceCollection resCol, string relativePath)
        {
            if (relativePath == "sound/config/bgm_property.mpb" || relativePath == "param/ui/ui_sound_db.bin" ||
                relativePath == "sound/config/bgm_mymusic.mmb" || relativePath == "ui/message/sound.msbt")
                return 0;
            return -1;
        }

        public override bool ResourceSelected(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            if (CanResourceBeLoaded(resCol, relativePath) != -1)
            {
                if (LaunchForm())
                    SavePlugin();
                return true;
            }
            return false;
        }
        #endregion
    }
}
