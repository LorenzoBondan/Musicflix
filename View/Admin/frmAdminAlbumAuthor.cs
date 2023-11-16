using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix.View
{
    public partial class frmAdminAlbumAuthor : Form
    {
        private AlbumRepository albumRepository = new AlbumRepository();
        private AuthorRepository authorRepository = new AuthorRepository();

        public frmAdminAlbumAuthor()
        {
            InitializeComponent();
            //albumRepository.CreateAlbumAuthorDatabase();
            albumRepository.FindAllAlbumAuthor(dataGridView1);
            FillAuthorsComboBox(cbAuthor);
            FillAlbumsComboBox(cbAlbum);

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);

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

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (cbAlbum.SelectedItem != null && cbAuthor.SelectedItem != null)
            {
                if (long.TryParse(cbAlbum.SelectedValue.ToString(), out long albumId) &&
                    long.TryParse(cbAuthor.SelectedValue.ToString(), out long authorId))
                {
                    albumRepository.InsertAlbumAuthor((int)albumId, (int)authorId, dataGridView1, labelResultado);
                }
                else
                {
                    MessageBox.Show("Erro ao converter valores selecionados.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um álbum e um autor.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (cbAlbum.SelectedItem != null && cbAuthor.SelectedItem != null)
            {
                if (long.TryParse(cbAlbum.SelectedValue.ToString(), out long albumId) &&
                    long.TryParse(cbAuthor.SelectedValue.ToString(), out long authorId))
                {
                    albumRepository.UpdateAlbumAuthor((int)albumId, (int)authorId, dataGridView1, labelResultado);
                }
                else
                {
                    MessageBox.Show("Erro ao converter valores selecionados.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um álbum e um autor.");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int albumId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            int authorId = (int)dataGridView1.SelectedRows[0].Cells[1].Value;
            albumRepository.DeleteAlbumAuthor(albumId, authorId, dataGridView1, labelResultado);
        }

        private List<AuthorDTO> GetAuthors()
        {
            List<AuthorDTO> authors = authorRepository.FindAll();
            return authors;
        }

        private List<AlbumDTO> GetAlbums()
        {
            List<AlbumDTO> albums = albumRepository.FindAll();
            return albums;
        }

        private void FillAuthorsComboBox(ComboBox comboBox)
        {
            List<AuthorDTO> authors = GetAuthors();
            comboBox.DataSource = authors;
            comboBox.DisplayMember = "name";
            comboBox.ValueMember = "id";
        }

        private void FillAlbumsComboBox(ComboBox comboBox)
        {
            List<AlbumDTO> albums = GetAlbums();
            comboBox.DataSource = albums;
            comboBox.DisplayMember = "name";
            comboBox.ValueMember = "id";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
