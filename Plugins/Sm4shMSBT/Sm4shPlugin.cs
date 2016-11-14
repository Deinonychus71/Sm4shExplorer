
using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.Objects;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Sm4shMSBT
{
    public class Sm4shPlugin : Sm4shBasePlugin
    {
        private string _ParamApplicationFile;

        #region Properties
        public override string Name
        {
            get { return "Sm4shMSBTReloaded"; }
        }

        public override string Description
        {
            get { return "Edit msbt files"; }
        }

        public override string Research
        {
            get { return "IcySon55"; }
        }

        public override string GUI
        {
            get { return "IcySon55"; }
        }

        public override string URL
        {
            get { return "https://github.com/IcySon55/3DLandMSBTeditor/"; }
        }

        public override string Version
        {
            get
            {
                return "0.1";
            }
        }

        public override bool ShowInPluginList
        {
            get { return false; }
        }

        public override Bitmap[] Icons
        {
            get { return new Bitmap[] { Resources.Resource.icon_msbt }; }
        }
        #endregion

        public Sm4shPlugin(Sm4shProject project)
            : base(project)
        {
            _ParamApplicationFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tools" + Path.DirectorySeparatorChar + "MsbtEditor.exe";
        }

        public override bool ResourceSelected(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            if (IsParamFile(relativePath) != -1 && File.Exists(extractedFile))
            {
                Process process = Process.Start(_ParamApplicationFile, "\"" + extractedFile + "\"");
                process.WaitForExit();
                return true;
            }
            return false;
        }

        public override int CanResourceBeLoaded(ResourceCollection resCol, string relativePath)
        {
            return IsParamFile(relativePath);
        }

        public override bool CanBeLoaded()
        {
            if (!File.Exists(_ParamApplicationFile))
            {
                LogHelper.Error(string.Format("Cannot load plugin 'Sm4shMSBTReloaded', Reason: '{0}' cannot be found.", _ParamApplicationFile));
                return false;
            }
            return true;
        }

        private int IsParamFile(string relativePath)
        {
            if (relativePath.EndsWith(".msbt"))
                return 0;

            return -1;
        }
    }
}
