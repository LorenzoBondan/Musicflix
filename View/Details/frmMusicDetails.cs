﻿using Music_Flix.Dtos;
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
        private UserRepository userRepository = new UserRepository();

        public int musicId, userLoggedId;
        public MusicDTO music;

        public frmMusicDetails(int musicId, int userLoggedId)
        {
            this.musicId = musicId;
            this.userLoggedId = userLoggedId;

            InitializeComponent();
            
            music = musicRepository.FindById(musicId, labelMusicName);
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
                MessageBox.Show("Erro ao carregar a imagem:  " + ex.Message);
            }

            FillAuthorsDataGridView(music.authorsIds, dataGridView2);
            FillReviewsDataGridView((int)music.id, dataGridView1);

            CreatePictureBoxes((int)music.averageScore);

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);
            dataGridView2.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);
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
            this.Show();
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

        public void FillReviewsDataGridView(int musicId, DataGridView dataGridView)
        {
            List<ReviewDTO> reviews = reviewRepository.FindAllReviewsByMusic(musicId);
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
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }

        private void SendReview(int musicId, int userId)
        {
            ReviewDTO review = new ReviewDTO();
            review.text = txtReview.Text;
            if (rb0.Checked)
            {
                review.score = 0;
            }
            else if (rb1.Checked)
            {
                review.score = 1;
            }
            else if (rb2.Checked)
            {
                review.score = 2;
            }
            else if (rb3.Checked)
            {
                review.score = 3;
            }
            else if (rb4.Checked)
            {
                review.score = 4;
            }
            else if (rb5.Checked)
            {
                review.score = 5;
            }
            else
            {
                MessageBox.Show("Insira alguma nota.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception();
            }
            review.userId = userId;
            review.musicId = musicId;
            review.moment = DateTime.Now.ToString();
            reviewRepository.Insert(review, labelResult: labelResult);
            FillReviewsDataGridView((int)music.id, dataGridView1);
        }

        private void btnAddAsFavorite_Click(object sender, EventArgs e)
        {
            userRepository.InsertUserMusic(userLoggedId, musicId, labelResult);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendReview(musicId, userLoggedId);
        }

        private void CreatePictureBoxes(int count)
        {
            int spacingX = 5;

            for (int i = 0; i < count; i++)
            {
                PictureBox pictureBox = new PictureBox();

                pictureBox.Width = 23;
                pictureBox.Height = 23;
                pictureBox.Location = new Point(330 + (i * (pictureBox.Width + spacingX)), 185);

                string caminho = Application.StartupPath + @"\star.png";
                pictureBox.Image = Image.FromFile(caminho);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox.Parent = groupBox1;
            }
        }
    }
}
