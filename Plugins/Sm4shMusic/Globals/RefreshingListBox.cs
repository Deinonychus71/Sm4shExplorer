using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sm4shMusic.Globals
{
    public class RefreshingListBox : ListBox
    {
        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }

        public new void RefreshItems()
        {
            base.RefreshItems();
        }
    }
}
