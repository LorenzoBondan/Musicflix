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
            try
            {
                UserInsertDTO user = new UserInsertDTO();
                user.name = txtName.Text;
                user.email = txtEmail.Text;
                user.password = txtPassword.Text;
                user.imgUrl = "https://digimedia.web.ua.pt/wp-content/uploads/2017/05/default-user-image.png";
                repository.Insert(user, labelResult: labelResult);
                MessageBox.Show("Usuário cadastrado com sucesso.");
                Close();
            }
            catch
            {
                MessageBox.Show("Erro ao cadastrar o usuário.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
