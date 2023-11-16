using Music_Flix.View;
using Music_Flix.View.Details;
using Music_Flix.View.Home;
using Music_Flix.View.Login;
using System;
using System.Windows.Forms;

namespace Music_Flix
{
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAdminAlbum f = new frmAdminAlbum();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAdminMusic f = new frmAdminMusic();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmAdminAuthor f = new frmAdminAuthor();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmAdminMusicAuthor f = new frmAdminMusicAuthor();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmAdminAlbumAuthor f = new frmAdminAlbumAuthor();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmAdminUser f = new frmAdminUser();
            f.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmAdminStyles f = new frmAdminStyles();
            f.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmHome f = new frmHome(new Entities.User());
            f.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmAuthorDetails f = new frmAuthorDetails(1);
            f.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            frmAlbumDetails f = new frmAlbumDetails(1);
            f.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
