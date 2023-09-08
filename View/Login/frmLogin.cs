using Music_Flix.Entities;
using Music_Flix.Repositories;
using Music_Flix.View.Home;
using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;

namespace Music_Flix.View.Login
{
    public partial class frmLogin : Form
    {
        private UserRepository repository = new UserRepository();

        public frmLogin()
        {
            InitializeComponent();
            //repository.CreateDatabase();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void Login()
        {
            string UsuarioDigitado = txtUsername.Text;
            string SenhaDigitada = txtPassword.Text;

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            conexao.Open();

            try
            {
                int userId;
                string query1 = "SELECT id FROM tb_user WHERE email = '" + UsuarioDigitado + "'  ";
                SqlCeCommand comando1 = new SqlCeCommand(query1, conexao);
                try
                {
                    userId = (int)comando1.ExecuteScalar();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Usuário não cadastrado.");
                    return;
                }

                string query2 = "SELECT password FROM tb_user WHERE email = '" + UsuarioDigitado + "'  ";

                SqlCeCommand comando = new SqlCeCommand(query2, conexao);
                string senhaBanco = comando.ExecuteScalar().ToString();

                if (senhaBanco != SenhaDigitada)
                {
                    txtPassword.Text = "";
                    txtPassword.Focus();
                    MessageBox.Show("Senha incorreta.");
                    return;
                }

                User user = repository.FindById(userId);

                MessageBox.Show("Logado com sucesso.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                frmHome f = new frmHome(user);
                f.Show();

                Hide();
                //Close();
                //Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            frmRegister f = new frmRegister();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Leave(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
