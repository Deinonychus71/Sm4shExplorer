using Sm4shFileExplorer;
using Sm4shFileExplorer.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sm4shFileExplorer.Objects;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace Sm4shParamEditor
{
    public class Sm4shPlugin : Sm4shBasePlugin
    {
        private string _ParamApplicationFile;

        #region Properties
        public override string Name
        {
            get { return "Sm4shParamEditor"; }
        }

        public override string Description
        {
            get { return "Edit various parameter files"; }
        }

        public override string Research
        {
            get { return "Sammi-Husky"; }
        }

        public override string GUI
        {
            get { return "Sammi-Husky"; }
        }

        public override string URL
        {
            get { return "https://github.com/Sammi-Husky/Sm4sh-Tools"; }
        }

        public override bool ShowInPluginList
        {
            get { return false; }
        }

        public override Bitmap[] Icons
        {
            get { return new Bitmap[] { Resources.Resource.icon_parameditor }; }
        }
        #endregion

        public Sm4shPlugin(Sm4shProject project)
            : base(project)
        {
            _ParamApplicationFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tools" + Path.DirectorySeparatorChar + "PARAM.exe";
        }

        public override bool ResourceSelected(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            if(IsParamFile(relativePath) != -1 && File.Exists(extractedFile))
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
            if(!File.Exists(_ParamApplicationFile))
            {
                LogHelper.Error(string.Format("Cannot load plugin 'Sm4shParamEditor', Reason: '{0}' cannot be found.", _ParamApplicationFile));
                return false;
            }
            return true;
        }

        private int IsParamFile(string relativePath)
        {
            if (!relativePath.EndsWith(".bin"))
                return -1;

            //Per folder
            if (relativePath.StartsWith("param/") ||
                relativePath.StartsWith("render/default/") ||
                (relativePath.StartsWith("stage/") && relativePath.Contains("param/")) ||
                relativePath.StartsWith("ui/render/")
                )
                return 0;

            //By name
            string name = relativePath.Substring(relativePath.LastIndexOf("/") + 1);
            if (name == "CollisionAttribute_cafe.bin" ||
                name == "StageParam_test1.bin" ||
                name == "StageParam_test2.bin" ||
                name == "render_common_param.bin" ||
                name == "light_set_param.bin" ||
                name == "render_param.bin" ||
                 name == "render_special_param.bin")
                return 0;

            return -1;
        }
    }
}
