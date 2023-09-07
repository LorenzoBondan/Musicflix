using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix.View
{
    public partial class frmAdminMusicAuthor : Form
    {
        private MusicRepository musicRepository = new MusicRepository();
        private AuthorRepository authorRepository = new AuthorRepository();

        public frmAdminMusicAuthor()
        {
            InitializeComponent();
            musicRepository.CreateMusicAuthorDatabase();
            musicRepository.FindAllMusicAuthor(dataGridView1);

            FillAuthorsComboBox(cbAuthor);
            FillMusicsComboBox(cbMusic);

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

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (cbMusic.SelectedItem != null && cbAuthor.SelectedItem != null)
            {
                if (long.TryParse(cbMusic.SelectedValue.ToString(), out long musicId) &&
                    long.TryParse(cbAuthor.SelectedValue.ToString(), out long authorId))
                {
                    musicRepository.InsertMusicAuthor((int)musicId, (int)authorId, dataGridView1, labelResultado);
                }
                else
                {
                    MessageBox.Show("Erro ao converter valores selecionados.");
                }
            }
            else
            {
                MessageBox.Show("Selecione uma música e um autor.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (cbMusic.SelectedItem != null && cbAuthor.SelectedItem != null)
            {
                if (long.TryParse(cbMusic.SelectedValue.ToString(), out long musicId) &&
                    long.TryParse(cbAuthor.SelectedValue.ToString(), out long authorId))
                {
                    musicRepository.UpdateMusicAuthor((int)musicId, (int)authorId, dataGridView1, labelResultado);
                }
                else
                {
                    MessageBox.Show("Erro ao converter valores selecionados.");
                }
            }
            else
            {
                MessageBox.Show("Selecione uma música e um autor.");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int musicId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            int authorId = (int)dataGridView1.SelectedRows[0].Cells[1].Value;
            musicRepository.DeleteMusicAuthor(musicId, authorId, dataGridView1, labelResultado);
        }

        private List<AuthorDTO> GetAuthors()
        {
            List<AuthorDTO> authors = authorRepository.FindAll();
            return authors;
        }

        private List<MusicDTO> GetMusics()
        {
            List<MusicDTO> musics = musicRepository.FindAll();
            return musics;
        }

        private void FillAuthorsComboBox(ComboBox comboBox)
        {
            List<AuthorDTO> authors = GetAuthors();
            comboBox.DataSource = authors;
            comboBox.DisplayMember = "name";
            comboBox.ValueMember = "id";
        }

        private void FillMusicsComboBox(ComboBox comboBox)
        {
            List<MusicDTO> musics = GetMusics();
            comboBox.DataSource = musics;
            comboBox.DisplayMember = "name";
            comboBox.ValueMember = "id";
        }
    }
}
