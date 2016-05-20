using Sm4shSound.Globals;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sm4shSound.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.lblTitle.Text += string.Format(" {0}", Sm4shMusic.VERSION);
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
