using Sm4shFileExplorer.UI;
using System;
using System.Windows.Forms;

namespace Sm4shFileExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Main main = new Main();
            if (main.MainLoaded)
                Application.Run(main);
        }
    }
}
