using Sm4shFileExplorer.Globals;
using Sm4shProjectManager.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sm4shFileExplorer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            this.lblTitle.Text += string.Format(" {0}", GlobalConstants.VERSION);
            this.lblThanks.Text += "\r\n" + Strings.INFO_THANKS;
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = this.lkRepo.Text;
            lkRepo.Links.Add(link);
        }

        private void lkRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }
    }
}
