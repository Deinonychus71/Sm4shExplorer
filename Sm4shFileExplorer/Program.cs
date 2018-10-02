using Sm4shFileExplorer.UI;
using Sm4shFileExplorer.UI.Objects;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Sm4shFileExplorer
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            if (Args.Length == 0)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Main main = new Main(Args);
                if (main.MainLoaded)
                    Application.Run(main);
            }
            else
            {
                
                Sm4shProject _ProjectManager = new Sm4shProject();
                switch (Args[0])
                {
                    case "help":
                    case "--help":
                        Console.WriteLine("Possible arguments:");
                        Console.WriteLine("To setup configuration: \'gamedump [path/to/gameDump]\'");
                        Console.WriteLine("To build with packing: \'build\'");
                        break;

                    case "gamedump":
                    case "--gamedump":
                        if (Args.Length != 2)
                        {
                            Console.WriteLine("Expected arguments: \'gamedump [path/to/gameDump]\'");
                            return;
                        }

                        string gameFolder = Args[1];
                        if (gameFolder[gameFolder.Length - 1] != Path.DirectorySeparatorChar)
                            gameFolder += Path.DirectorySeparatorChar;

                        if (!PathHelper.IsItSmashFolder(gameFolder))
                        {
                            Console.WriteLine(UIStrings.ERROR_LOADING_GAME_FOLDER);
                            return;
                        }
                        if (!PathHelper.DoesItHavePatchFolder(gameFolder))
                        {
                            Console.WriteLine(UIStrings.ERROR_LOADING_GAME_PATCH_FOLDER);
                            return;
                        }

                        LogHelper.Info("Creating configuration file...");
                        Sm4shMod newProject = _ProjectManager.CreateNewProject(GlobalConstants.CONFIG_FILE, gameFolder);
                        Console.WriteLine(UIStrings.CREATE_PROJECT_SUCCESS);
                        break;

                    case "build":
                    case "--build":
                        if (!File.Exists(GlobalConstants.CONFIG_FILE))
                        {
                            Console.WriteLine("No configuration exists. Run \'sm4shexplorer.exe gamedump [path/to/gameDump]\'");
                            return;
                        }
                        _ProjectManager.LoadProject(GlobalConstants.CONFIG_FILE);
                        if (_ProjectManager.CurrentProject == null)
                        {
                            return;
                        }

                        Options _Options = new Options(_ProjectManager.CurrentProject);
                        _ProjectManager.RebuildRFAndPatchlist();
                        break;

                    default:
                        Console.WriteLine("Unsupported argument. Supported arguments include: help, gamedump, build");
                        break;
                }
            }
        }
    }
}
