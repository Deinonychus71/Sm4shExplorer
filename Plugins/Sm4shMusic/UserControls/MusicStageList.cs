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
using System.Collections;
using Sm4shMusic.EventHandlers;
using Sm4shMusic.Globals;

namespace Sm4shMusic.UserControls
{
    public partial class MusicStageList : UserControl
    {
        public event ItemSelected ItemSelected;
        public event ItemAdded ItemAdded;
        public event ItemRemoved ItemRemoved;
        public event ItemMoving ItemMoving;

        private object _DataSource;
        private object _Items;
        private string _VGMStreamFile;

        #region Public Properties
        public object DataSource { get { return _DataSource; } set { _DataSource = value; RefreshData(); } }
        public object Items { get { return _Items; } set { _Items = value; RefreshItems(); } }
        public int MaxItems { get; set; }
        public string VGMStreamFile { get { return _VGMStreamFile; } set { _VGMStreamFile = value; RefreshVGMPlayer();  } }
        #endregion

        public MusicStageList()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            ddlEntries.DataSource = null; 
            ddlEntries.DataSource = _DataSource; 
            ddlEntries.DisplayMember = "ListTitle"; 
            ddlEntries.ValueMember = "ListValue";
        }

        public void RefreshItems()
        {
            listBox.DataSource = null; 
            listBox.DataSource = _Items; 
            listBox.DisplayMember = "ListTitle"; 
            listBox.ValueMember = "ListValue";

            lblEntries.Text = listBox.Items.Count + "/" + MaxItems;
        }

        private void RefreshVGMPlayer()
        {
            if (VGMStreamPlayer.CurrentVGMStreamPlayer == vgmStream)
                VGMStreamPlayer.StopCurrentVGMStreamPlayback();

            if (string.IsNullOrEmpty(_VGMStreamFile))
                vgmStream.File = null;
            else
                vgmStream.File = _VGMStreamFile;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listBox.Items.Count > MaxItems)
            {
                MessageBox.Show(string.Format(Strings.ERROR_STAGE_ITEM_MAX, MaxItems), Strings.CAPTION_ERROR);
                return;
            }
            if (ItemAdded != null)
                this.ItemAdded(this, new ListEntryArgs(ddlEntries.SelectedItem));
            RefreshItems();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == listBox.Items.Count - 1)
                return;
            object item = listBox.SelectedItem;
            if (ItemMoving != null)
                this.ItemMoving(this, new MoveItemArgs(listBox.SelectedItem, listBox.SelectedIndex, 1));
            RefreshItems();
            listBox.SelectedItem = item;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == 0)
                return;
            object item = listBox.SelectedItem;
            if (ItemMoving != null)
                this.ItemMoving(this, new MoveItemArgs(listBox.SelectedItem, listBox.SelectedIndex, -1));
            RefreshItems();
            listBox.SelectedItem = item;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ItemRemoved != null)
                this.ItemRemoved(this, new ListEntryArgs(listBox.SelectedItem));
            RefreshItems();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemSelected != null)
                this.ItemSelected(this, new ListEntryArgs(listBox.SelectedItem));
        }

        private void help_Click(object sender, EventArgs e)
        {
            if (this.Name == "SoundDB")
                MessageBox.Show(Strings.HELP_MUSIC_STAGE_SOUND, Strings.CAPTION_HELP);
            else
                MessageBox.Show(Strings.HELP_MUSIC_STAGE_MYMUSIC, Strings.CAPTION_HELP);
        }

        #region Methods
        private void MoveItem(int direction)
        {
        }
        #endregion
    }
}
