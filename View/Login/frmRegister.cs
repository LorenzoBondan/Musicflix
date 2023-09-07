using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Windows.Forms;

namespace Music_Flix.View.Login
{
    public partial class frmRegister : Form
    {
        private UserRepository repository = new UserRepository();

        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            UserInsertDTO user = new UserInsertDTO();
            user.name = txtName.Text;
            user.email = txtEmail.Text;
            user.password = txtPassword.Text;
            user.imgUrl = null;
            repository.Insert(user, labelResult: labelResult);
            ClearFields();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
        }
    }
}
