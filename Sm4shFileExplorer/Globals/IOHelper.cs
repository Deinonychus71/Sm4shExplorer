using System;
using System.IO;
using System.Threading;

namespace Sm4shFileExplorer.Globals
{
    public static class IOHelper
    {
        public static bool CopyFile(string source, string destination)
        {
            try
            {
                LogHelper.Debug(string.Format("Copying file '{0}' to '{1}'...", source, destination), LogHelper.PATH_LOG_IO);
                if (File.Exists(source))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                    File.Copy(source, destination, true);
                    File.SetLastWriteTimeUtc(destination, DateTime.Now);
                }
                else
                    return false;
            }
            catch(Exception e)
            {
                LogHelper.Error(string.Format("Could not copy file '{0}' to '{1}', check logs for details.", source, destination), LogHelper.PATH_LOG_IO);
                LogHelper.Debug(e.Message, LogHelper.PATH_LOG_IO);
                return false;
            }
            return true;
        }

        public static bool DeleteFile(string file)
        {
            try
            {
                LogHelper.Debug(string.Format("Deleting file '{0}'...", file), LogHelper.PATH_LOG_IO);
                if(File.Exists(file))
                    File.Delete(file);
                else
                    return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Could not delete file '{0}', check logs for details.", file), LogHelper.PATH_LOG_IO);
                LogHelper.Debug(e.Message, LogHelper.PATH_LOG_IO);
                return false;
            }
            return true;
        }

        public static bool DeleteDirectory(string folder)
        {
            try
            {
                LogHelper.Debug(string.Format("Deleting directory '{0}'...", folder), LogHelper.PATH_LOG_IO);
                if (Directory.Exists(folder))
                    DeleteDirectoryRec(folder);
                else
                    return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Could not delete directory '{0}', check logs for details.", folder), LogHelper.PATH_LOG_IO);
                LogHelper.Debug(e.Message, LogHelper.PATH_LOG_IO);
                return false;
            }
            return true;
        }

        private static void DeleteDirectoryRec(string folder)
        {
            foreach (string directory in Directory.GetDirectories(folder))
            {
                DeleteDirectoryRec(directory);
            }

            try
            {
                Thread.Sleep(0);
                Directory.Delete(folder, true);
            }
            catch (IOException)
            {
                Thread.Sleep(100);
                Directory.Delete(folder, true);
            }
            catch (UnauthorizedAccessException)
            {
                Thread.Sleep(100);
                Directory.Delete(folder, true);
            }
        }
    }
}
