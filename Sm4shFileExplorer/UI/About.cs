using Sm4shFileExplorer.Globals;
using Sm4shFileExplorer.UI.Objects;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sm4shFileExplorer.UI
{
    internal partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            this.lblTitle.Text += string.Format(" {0}", GlobalConstants.VERSION);
            this.lblThanks.Text += "\r\n" + UIStrings.INFO_THANKS;
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
