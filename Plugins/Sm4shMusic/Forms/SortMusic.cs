using Sm4shMusic.Globals;
using Sm4shMusic.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sm4shMusic.Forms
{
    public partial class SortMusic : Form
    {
        private enum TypeSort
        {
            SoundTestOrder,
            StageCreationOrder
        }

        private TypeSort _TypeSort;
        private IEnumerable _CurrentList;
        private IEnumerable _CurrentListOriginal;

        public string FormTitle { get { return this.Text; } set { this.Text = value; } }

        public SortMusic()
        {
            InitializeComponent();
        }

        #region Common Methods
        private void MoveUp(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);
                }
            }
            else if (node.TreeView.Nodes.Contains(node)) //root node
            {
                int index = view.Nodes.IndexOf(node);
                if (index > 0)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index - 1, node);
                }
            }
            treeView.SelectedNode = node;
        }

        private void MoveDown(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);
                }
            }
            else if (view != null && view.Nodes.Contains(node)) //root node
            {
                int index = view.Nodes.IndexOf(node);
                if (index < view.Nodes.Count - 1)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index + 1, node);
                }
            }
            treeView.SelectedNode = node;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                MoveDown(treeView.SelectedNode);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                MoveUp(treeView.SelectedNode);
        }

        private void btnMoveDown10_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                for (int i = 0; i < 10; i++) MoveDown(treeView.SelectedNode);
        }

        private void btnMoveUp10_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                for (int i = 0; i < 10; i++) MoveUp(treeView.SelectedNode);
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (_TypeSort)
            {
                case TypeSort.StageCreationOrder:
                    SaveStageCreationOrder();
                    break;
                case TypeSort.SoundTestOrder:
                    SaveSoundTestOrder();
                    break;
            }
            this.Close();
        }

        private void btnRestoreOrder_Click(object sender, EventArgs e)
        {
            switch (_TypeSort)
            {
                case TypeSort.StageCreationOrder:
                    RestoreStageCreationOrder();
                    break;
                case TypeSort.SoundTestOrder:
                    RestoreSoundTestOrder();
                    break;
            }
        }
        #endregion

        #region Load
        public void LoadSoundTestOrder(List<SoundEntry> soundEntries, List<SoundEntry> soundEntriesBackup)
        {
            treeView.Nodes.Clear();
            foreach (SoundEntry sEntry in soundEntries.OrderBy(p => p.SoundTestOrder))
                treeView.Nodes.Add(sEntry.SoundID, sEntry.Title);
            _TypeSort = TypeSort.SoundTestOrder;
            _CurrentList = soundEntries;
            _CurrentListOriginal = soundEntriesBackup;
        }

        public void LoadStageCreationOrder(List<SoundEntry> soundEntries, List<SoundEntry> soundEntriesBackup)
        {
            treeView.Nodes.Clear();
            foreach (SoundEntry sEntry in soundEntries.OrderBy(p => p.StageCreationOrder))
            {
                TreeNode group = null;
                string groupKey = "g_" + sEntry.StageCreationGroupID;
                if (!treeView.Nodes.ContainsKey(groupKey))
                    group = treeView.Nodes.Add(groupKey, sEntry.StageCreationGroupName);
                else
                    group = treeView.Nodes[groupKey];
                group.Nodes.Add(sEntry.SoundID, sEntry.Title);
            }
            _TypeSort = TypeSort.StageCreationOrder;
            _CurrentList = soundEntries;
            _CurrentListOriginal = soundEntriesBackup;
        }
        #endregion

        #region Save
        public void SaveSoundTestOrder()
        {
            int index = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (TreeNode nodeSound in treeView.Nodes)
            {
                dict.Add(nodeSound.Name, index);
                index++;
            }
            IEnumerator enumerator = _CurrentList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SoundEntry sEntry = (SoundEntry)enumerator.Current;
                sEntry.SoundTestOrder = dict[sEntry.SoundID];
            }
        }
        
        public void SaveStageCreationOrder()
        {
            int index = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (TreeNode nodeGroup in treeView.Nodes)
            {
                foreach (TreeNode nodeSound in nodeGroup.Nodes)
                {
                    dict.Add(nodeSound.Name, index);
                    index++;
                }
            }
            IEnumerator enumerator = _CurrentList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SoundEntry sEntry = (SoundEntry)enumerator.Current;
                sEntry.StageCreationOrder = dict[sEntry.SoundID];
            }
        }
        #endregion

        #region Restore
        public void RestoreSoundTestOrder()
        {
            if (MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Dictionary<string, int> dictOriginal = new Dictionary<string, int>();
                List<SoundEntry> list = new List<SoundEntry>();
                List<SoundEntry> listOriginal = new List<SoundEntry>();

                IEnumerator enumeratorOriginal = _CurrentListOriginal.GetEnumerator();
                while (enumeratorOriginal.MoveNext())
                {
                    SoundEntry sEntry = (SoundEntry)enumeratorOriginal.Current;
                    dictOriginal.Add(sEntry.SoundID, sEntry.SoundTestOrder);
                    listOriginal.Add(sEntry);
                }

                IEnumerator enumerator = _CurrentList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    SoundEntry sEntry = (SoundEntry)enumerator.Current;
                    sEntry.SoundTestOrder = dictOriginal[sEntry.SoundID];
                    list.Add(sEntry);
                }
                LoadSoundTestOrder(list, listOriginal);
            }
        }

        public void RestoreStageCreationOrder()
        {
            if (MessageBox.Show(Strings.WARNING_RESTORE_DATA, Strings.CAPTION_WARNING, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Dictionary<string, int> dictOriginal = new Dictionary<string, int>();
                List<SoundEntry> list = new List<SoundEntry>();
                List<SoundEntry> listOriginal = new List<SoundEntry>();

                IEnumerator enumeratorOriginal = _CurrentListOriginal.GetEnumerator();
                while (enumeratorOriginal.MoveNext())
                {
                    SoundEntry sEntry = (SoundEntry)enumeratorOriginal.Current;
                    dictOriginal.Add(sEntry.SoundID, sEntry.StageCreationOrder);
                    listOriginal.Add(sEntry);
                }

                IEnumerator enumerator = _CurrentList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    SoundEntry sEntry = (SoundEntry)enumerator.Current;
                    sEntry.StageCreationOrder = dictOriginal[sEntry.SoundID];
                    list.Add(sEntry);
                }
                LoadStageCreationOrder(list, listOriginal);
            }
        }
        #endregion
    }
}
