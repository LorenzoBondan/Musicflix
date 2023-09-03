using Music_Flix.Dtos;
using Music_Flix.Repositories;
using Music_Flix.View;
using Music_Flix.View.Details;
using Music_Flix.View.Home;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix
{
    public partial class frmAdmin : Form
    {
        private StyleRepository repository = new StyleRepository();

        public frmAdmin()
        {
            InitializeComponent();
            //repository.CreateDatabase();
            repository.FindAll(dataGridView1);

            /*
             * pictureBox SizeMode = stretchImage
            string imageUrl = "https://www.cnnbrasil.com.br/wp-content/uploads/sites/12/2023/04/52803317585_0ca7776240_o-e1690386882737.jpg?w=1200&h=675&crop=1";
            string localImagePath = Path.Combine(Application.StartupPath, "images", "temp_image.png"); // Caminho local para salvar a imagem
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(imageUrl, localImagePath);
                }
                pictureBox1.Image = Image.FromFile(localImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
            }
            */

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



        private void btnFindById_Click(object sender, EventArgs e)
        {
            repository.FindById(int.Parse(txtId.Text), labelResultado);
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            StyleDTO styleDTO = new StyleDTO();
            styleDTO.id = (dataGridView1.Rows.Count + 1);
            styleDTO.description = txtStyle.Text;
            repository.Insert(styleDTO, dataGridView1, labelResultado);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                StyleDTO styleDTO = new StyleDTO();
                styleDTO.id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                styleDTO.description = txtStyle.Text;
                repository.Update(styleDTO, dataGridView1, labelResultado);
            }
            else
            {
                MessageBox.Show("Nenhuma linha selecionada para edição.");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            repository.Delete(id, dataGridView1, labelResultado);
        }

        private void txtNome_KeyUp(object sender, KeyEventArgs e)
        {
            string description = txtNome.Text;
            repository.Filter(description, dataGridView1, labelResultado);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string description = selectedRow.Cells[1].Value.ToString();
                txtStyle.Text = description;
            }
            else
            {
                txtStyle.Text = string.Empty;
            }
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
            frmHome f = new frmHome();
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
    }
}
