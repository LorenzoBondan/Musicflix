using Music_Flix.Dtos;
using Music_Flix.Entities;
using Music_Flix.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Music_Flix.View.Details
{
    public partial class frmMusicDetails : Form
    {
        private MusicRepository musicRepository = new MusicRepository();
        private AuthorRepository authorRepository = new AuthorRepository();
        private AlbumRepository albumRepository = new AlbumRepository();
        private ReviewRepository reviewRepository = new ReviewRepository();

        public frmMusicDetails(int musicId)
        {
            InitializeComponent();
            
            MusicDTO music = musicRepository.FindById(musicId, labelMusicName);
            lblMusicExplicit.Text = music.isExplicit == 'Y' ? "Explicit" : "Non explicit";
            lblMusicDuration.Text = music.minutes.ToString() + ":" + music.seconds.ToString();
            lblMusicYear.Text = music.year.ToString();

            Album album = albumRepository.FindById((int)music.albumId, lblMusicAlbum);

            string imageUrl = album.imgUrl;
            string localImagePath = Path.Combine(Application.StartupPath, "images", " '" + album.name + "' '" + DateTime.Now.Ticks + ".png"); // Caminho local para salvar a imagem
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

            FillAuthorsDataGridView(music.authorsIds, dataGridView2);
            FillReviewsDataGridView(music.reviews, dataGridView1);

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            // Fonte
            //dataGridView2.DefaultCellStyle.Font = new Font("Century Gothic",8);

            // Bordas
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            // Cabeçalho
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho
            dataGridView2.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            #endregion
            
        }
        public void FillAuthorsDataGridView(List<long> authorsIds, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            foreach (long authorId in authorsIds)
            {
                Author author = authorRepository.FindById((int)authorId);
                dataGridView.Rows.Add(author.id, author.name, author.birthDate, author.imgUrl, author.getAverageScore());
            }
        }

        public void FillReviewsDataGridView(List<ReviewDTO> reviews, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            foreach (ReviewDTO review in reviews)
            {
                dataGridView.Rows.Add(review.id, review.text, review.moment, review.score, review.userId, review.musicId);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
                {
                    DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                    string idCellValue = selectedRow.Cells[0].Value.ToString();

                    if (int.TryParse(idCellValue, out int authorId))
                    {
                        frmAuthorDetails f = new frmAuthorDetails(authorId);
                        f.Show();
                    }
                    else
                    {
                        MessageBox.Show("Valor da célula 'Id' não é um número inteiro válido.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }
    }
}
