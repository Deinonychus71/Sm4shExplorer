using Sm4shMusic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shMusic.EventHandlers
{
    public class MoveItemArgs : EventArgs
    {
        private object _ListEntry;
        private int _Index;
        private int _Direction;

        public MoveItemArgs(object listEntry, int index, int direction)
        {
            _ListEntry = listEntry;
            _Index = index;
            _Direction = direction;
        }

        public object ListEntry
        {
            get
            {
                return _ListEntry;
            }
        }

        public int Index
        {
            get
            {
                return _Index;
            }
        }

        public int Direction
        {
            get
            {
                return _Direction;
            }
        }
    }
}
