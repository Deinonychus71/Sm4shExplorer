using Sm4shFileExplorer.Objects;
using System;
using System.IO;

namespace Sm4shFileExplorer.Globals
{
    public static class LogHelper
    {
        private static Sm4shMod _Project;

        internal static void InitializeLogHelper(Sm4shMod project)
        {
            _Project = project;
        }

        private static void LogMessageFile(string type, string message, bool retry)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                sw = System.IO.File.AppendText(".\\logs\\logs.txt");
                string logLine = System.String.Format("{0:G} {1}: {2}", DateTime.Now, type, message);
                sw.WriteLine(logLine);
            }
            catch(Exception e)
            {
                if (!retry)
                {
                    Directory.CreateDirectory(".\\logs");
                    LogMessageFile(type, message, true);
                    return;
                }
                throw new Exception(e.Message, e);
            }
            finally
            {
                if(sw != null)
                    sw.Close();
            }
        }

        public static void Info(string message)
        {
            Console.WriteLine("INFO  " + message);
            LogMessageFile("INFO ", message, false);
        }

        public static void Error(string message)
        {
            Console.WriteLine("ERROR " + message);
            LogMessageFile("ERROR", message, false);
        }

        public static void Warning(string message)
        {
            Console.WriteLine("WARN  " + message);
            LogMessageFile("WARN ", message, false);
        }

        public static void Debug(string message)
        {
            if(_Project != null && _Project.Debug)
                Console.WriteLine("DEBUG " + message);
            LogMessageFile("DEBUG", message, false);
        }
    }
}
