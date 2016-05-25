using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sm4shMusic.Objects;
using Sm4shMusic.EventHandlers;
using Sm4shMusic.Globals;

namespace Sm4shMusic.UserControls
{
    public partial class ListEntries : UserControl
    {
        public event ItemSelected ItemSelected;
        public event ItemAdded ItemAdded;
        public event ItemRemoved ItemRemoved;
        private object _DataSource;
        private bool _Refreshing = false;
        private bool _VisibleActions = true;
        private object currentSelected = null;

        public bool VisibleActions { get { return _VisibleActions; } set { _VisibleActions = value; btnAdd.Visible = _VisibleActions; btnRemove.Visible = _VisibleActions; } }
        public string Label { get { return lblTitle.Text; } set { lblTitle.Text = value; } }
        public object DataSource { get { return _DataSource; } set { _DataSource = value; RefreshList(); } }

        public ListEntries()
        {
            InitializeComponent();
        }

        public void RefreshList()
        {
            listBox.ClearSelected();
            listBox.DataSource = null;
            listBox.Items.Clear();
            listBox.DataSource = _DataSource;

            if(_DataSource != null)
                listBox.SelectedIndex = 0;

            listBox.DisplayMember = "ListTitle";
            listBox.ValueMember = "ListValue";

            lblEntries.Text = "Entries : " + listBox.Items.Count;
        }

        public void RefreshSelectedItem()
        {
            _Refreshing = true;
            listBox.RefreshItem(listBox.SelectedIndex);
            _Refreshing = false;
        }

        public void SelectItem(object item)
        {
            listBox.SelectedItem = item;
           // listBox.SelectedIndex = listBox.SelectedValue = id;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemSelected != null && !_Refreshing)
            {
                if (currentSelected != listBox.SelectedItem)
                {
                    currentSelected = listBox.SelectedItem;
                    this.ItemSelected(this, new ListEntryArgs(listBox.SelectedItem));
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ItemAdded != null)
                this.ItemAdded(this, new ListEntryArgs(null));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ItemRemoved != null && MessageBox.Show(string.Format(Strings.INFO_CONFIRM_DELETE, listBox.SelectedItem.ToString()), Strings.CAPTION_INFO, MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.ItemRemoved(this, new ListEntryArgs(listBox.SelectedItem));
        }
    }
}
