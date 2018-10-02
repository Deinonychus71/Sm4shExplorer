using Sm4shFileExplorer.Objects;
using System;
using System.IO;

namespace Sm4shFileExplorer.Globals
{
    public static class LogHelper
    {
        private static Sm4shMod _Project;
        public const string PATH_LOG_GENERAL = ".\\logs\\logs.txt";
        public const string PATH_LOG_IO = ".\\logs\\io.txt";

        internal static void InitializeLogHelper(Sm4shMod project)
        {
            _Project = project;
        }

        private static void LogMessageFile(string type, string message, bool retry, string file)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                sw = System.IO.File.AppendText(file);
                string logLine = System.String.Format("{0:G} {1}: {2}", DateTime.Now, type, message);
                sw.WriteLine(logLine);
            }
            catch(Exception e)
            {
                if (!retry)
                {
                    Directory.CreateDirectory(".\\logs");
                    LogMessageFile(type, message, true, file);
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
            Info(message, PATH_LOG_GENERAL);
        }

        public static void Info(string message, string file)
        {
            Console.WriteLine("INFO  " + message);
            LogMessageFile("INFO ", message, false, file);
        }


        public static void Error(string message)
        {
            Error(message, PATH_LOG_GENERAL);
        }

        public static void Error(string message, string file)
        {
            Console.WriteLine("ERROR " + message);
            LogMessageFile("ERROR", message, false, file);
        }


        public static void Warning(string message)
        {
            Warning(message, PATH_LOG_GENERAL);
        }

        public static void Warning(string message, string file)
        {
            Console.WriteLine("WARN  " + message);
            LogMessageFile("WARN ", message, false, file);
        }


        public static void Debug(string message)
        {
            Debug(message, PATH_LOG_GENERAL);
        }

        public static void Debug(string message, string file)
        {
            if (_Project != null && _Project.Debug)
                Console.WriteLine("DEBUG " + message);
            LogMessageFile("DEBUG", message, false, file);
        }
    }
}
