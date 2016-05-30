using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shFileExplorer.UI.Objects
{
    internal enum BackgroundWorkerMode
    {
        ProjectLoading = 0,
        BuildProject = 1,
        SendToSD = 2,
        Extract = 3
    }

    internal class BackgroundWorkerInstance
    {
        public BackgroundWorkerMode Mode { get; private set; }
        public object Object { get; private set; }

        public BackgroundWorkerInstance(BackgroundWorkerMode mode, object obj)
        {
            Mode = mode;
            Object = obj;
        }
    }
}
