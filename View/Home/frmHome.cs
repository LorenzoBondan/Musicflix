using Music_Flix.Dtos;
using Music_Flix.Entities;
using Music_Flix.Repositories;
using Music_Flix.View.Details;
using Music_Flix.View.Login;
using Music_Flix.View.Profile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Music_Flix.View.Home
{
    public partial class frmHome : Form
    {
        private MusicRepository musicRepository = new MusicRepository();
        private StyleRepository styleRepository = new StyleRepository();
        public User userLogged; // used to call frmProfile

        public frmHome(User user)
        {
            userLogged = user;
            InitializeComponent();
            FillStyleComboBox(cbStyle);
            cbStyle.SelectedItem = null;
            musicRepository.FindAll(dataGridView1);
            dataGridView1.ClearSelection();

            CleanFolder(Path.Combine(Application.StartupPath, "images")); // limpa a pasta das imagens geradas
            
            lblUsername.Text = user.name;

            string imageUrl = user.imgUrl;
            string localImagePath = Path.Combine(Application.StartupPath, "images", " '" + user.name + "' '" + DateTime.Now.Ticks + "'.png"); // Caminho local para salvar a imagem
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(imageUrl, localImagePath);
                }
                userPictureBox.Image = Image.FromFile(localImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
            }

            if (user.admin == "Y")
            {
                btnAdmin.Enabled = true;
            }

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);

            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Fonte
            //dataGridView2.DefaultCellStyle.Font = new Font("Century Gothic",8);

            // Bordas
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;

            // Cabeçalho
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);

            dataGridView1.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            #endregion
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            string name = txtName.Text;
            musicRepository.Filter(name, dataGridView1, labelResultado);
        }

        private List<StyleDTO> GetStyles()
        {
            List<StyleDTO> styles = styleRepository.FindAll();
            return styles;
        }

        private void FillStyleComboBox(ComboBox comboBox)
        {
            List<StyleDTO> styles = GetStyles();
            comboBox.DataSource = styles;
            comboBox.DisplayMember = "description";
            comboBox.ValueMember = "id";
        }

        private void cbStyle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cbStyle.SelectedItem is StyleDTO selectedStyle)
            {
                musicRepository.FindAllByStyle(selectedStyle.description, dataGridView1);
                dataGridView1.ClearSelection();
            }
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cbStyle.SelectedItem = null;
            txtName.Text = "";
            musicRepository.FindAll(dataGridView1);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                frmMusicDetails f = new frmMusicDetails((int)dataGridView1.SelectedRows[0].Cells[0].Value, userLogged.id);
                f.Show();
            }
            catch (Exception)
            {
                //
            }
        }

        public static void CleanFolder(string folder)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    string[] files = Directory.GetFiles(folder);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    MessageBox.Show("A pasta não existe.");
                }
            }
            catch (Exception ex)
            {
                // ignora e não deleta
            }
        }

        private void userPictureBox_Click(object sender, EventArgs e)
        {
            frmProfile frmProfile = new frmProfile(userLogged);
            frmProfile.Show();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            frmAdmin frmAdmin = new frmAdmin();
            frmAdmin.Show();
        }
    }
}
