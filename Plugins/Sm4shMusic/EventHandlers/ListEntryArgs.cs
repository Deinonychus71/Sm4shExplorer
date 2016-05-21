using Sm4shMusic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shMusic.EventHandlers
{
    public class ListEntryArgs : EventArgs
    {
        private object _ListEntry;

        public ListEntryArgs(object listEntry)
        {
            _ListEntry = listEntry;
        }

        public object ListEntry
        {
            get
            {
                return _ListEntry;
            }
        }
    }
}
