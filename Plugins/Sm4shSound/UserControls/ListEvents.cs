using Sm4shSound.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sm4shSound.UserControls
{
    public delegate void ItemSelected(object sender, ListEntryArgs e);
    public delegate void ItemAdded(object sender, ListEntryArgs e);
    public delegate void ItemRemoved(object sender, ListEntryArgs e);
    public delegate void ItemMoving(object sender, MoveItemArgs e);
}
