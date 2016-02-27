using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sm4shProjectManager.Globals
{
    public enum PathHelperEnum
    {
        FOLDER_CODE = 0,
        FOLDER_CONTENT = 1,
        FOLDER_META = 2,
        FOLDER_MOVIE = 3,
        FOLDER_SOUND = 4,
        FOLDER_PATCH = 5,
        FOLDER_PATCH_DATA = 6,
        FILE_LS = 7,
        FILE_RPX = 8,
        FILE_META = 9,
        FILE_PATCH_RESOURCE = 10,
        FILE_PATCH_PATCHLIST = 11,
    }

    public static class PathHelper
    {
        public static bool IsItSmashFolder(string folder)
        {
            if (!File.Exists(PathHelper.GetPath(folder, PathHelperEnum.FILE_LS)))// ||
                    //!File.Exists(PathHelper.GetPath(folder, PathHelperEnum.FILE_META)) ||
                    //!File.Exists(PathHelper.GetPath(folder, PathHelperEnum.FILE_RPX)))
                return false;
            return true;
        }

        public static bool DoesItHavePatchFolder(string folder)
        {
            if (!File.Exists(PathHelper.GetPath(folder, PathHelperEnum.FILE_PATCH_RESOURCE)))
                return false;
            return true;
        }

        public static string GetPath(string rootPath, PathHelperEnum path)
        {
            switch(path)
            {
                case PathHelperEnum.FOLDER_CODE:
                    return rootPath + "code" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_CONTENT:
                    return rootPath + "content" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_META:
                    return rootPath + "meta" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_MOVIE:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_CONTENT) + "movie" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_SOUND:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_CONTENT) + "sound" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_PATCH:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_CONTENT) + "patch" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FOLDER_PATCH_DATA:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_PATCH) + "data" + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FILE_LS:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_CONTENT) + "ls";
                case PathHelperEnum.FILE_RPX:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_CODE) + "cross_f.rpx";
                case PathHelperEnum.FILE_META:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_META) + "meta.xml";
                case PathHelperEnum.FILE_PATCH_RESOURCE:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_PATCH) + "resource";
                case PathHelperEnum.FILE_PATCH_PATCHLIST:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_PATCH) + "patchlist";
                default:
                    return rootPath;
            }
        }

        public static string GetPath(string rootPath, PathHelperEnum path, string region)
        {
            switch (path)
            {
                case PathHelperEnum.FOLDER_PATCH_DATA:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_PATCH) + "data" + region + GlobalConstants.FOLDER_SEPARATOR;
                case PathHelperEnum.FILE_PATCH_RESOURCE:
                    return GetPath(rootPath, PathHelperEnum.FOLDER_PATCH) + "resource" + region;
                default:
                    return GetPath(rootPath, path);
            }
        }

        public static string GetGamePath(PathHelperEnum path)
        {
            return GetPath(Sm4shProject.CurrentProject.GamePath, path);
        }

        public static string GetGamePath(PathHelperEnum path, string region)
        {
            return GetPath(Sm4shProject.CurrentProject.GamePath, path, region);
        }

        public static string GetProjectTempFolder()
        {
            if(string.IsNullOrEmpty(Sm4shProject.CurrentProject.ProjectTempPath))
                return Path.GetTempPath() + "sm4shexplorer" + GlobalConstants.FOLDER_SEPARATOR;
            return Sm4shProject.CurrentProject.ProjectTempPath;
        }

        public static string GetProjectWorkplaceFolder()
        {
            if (string.IsNullOrEmpty(Sm4shProject.CurrentProject.ProjectWorkplacePath))
                return Directory.GetCurrentDirectory() + GlobalConstants.FOLDER_SEPARATOR + "workspace" + GlobalConstants.FOLDER_SEPARATOR;
            return Sm4shProject.CurrentProject.ProjectWorkplacePath;
        }

        public static string GetProjectExportFolder()
        {
            if (string.IsNullOrEmpty(Sm4shProject.CurrentProject.ProjectExportPath))
                return Directory.GetCurrentDirectory() + GlobalConstants.FOLDER_SEPARATOR + "export" + GlobalConstants.FOLDER_SEPARATOR;
            return Sm4shProject.CurrentProject.ProjectExportPath;
        }

        public static string GetProjectExtractFolder()
        {
            if (string.IsNullOrEmpty(Sm4shProject.CurrentProject.ProjectExtractPath))
                return Directory.GetCurrentDirectory() + GlobalConstants.FOLDER_SEPARATOR + "extract" + GlobalConstants.FOLDER_SEPARATOR;
            return Sm4shProject.CurrentProject.ProjectExtractPath;
        }

        public static string[] GetDTFiles()
        {
            List<string> lDtList = new List<string>();
            string contentPath = GetGamePath(PathHelperEnum.FOLDER_CONTENT);
            int i = 0;
            //3DS
            if (File.Exists(contentPath + "dt"))
            {
                lDtList.Add(contentPath + "dt");
            }
            else
            {
                //WiiU
                while (File.Exists(contentPath + "dt0" + i))
                {
                    lDtList.Add(contentPath + "dt0" + i);
                    i++;
                }
            }
            return lDtList.ToArray();
        }

        public static string[] GetResourceFiles(string folder)
        {
            List<string> listResourceFiles = new List<string>();
            foreach (string resourceFile in Directory.GetFiles(folder))
            {
                if (!Path.GetFileName(resourceFile).StartsWith("resource") || Path.GetFileName(resourceFile).Contains("."))
                    continue;
                listResourceFiles.Add(resourceFile);
            }
            return listResourceFiles.ToArray();
        }
    }
}
