﻿using Music_Flix.Entities;
using Music_Flix.Repositories;
using System.IO;
using System.Net;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Music_Flix.Dtos;

namespace Music_Flix.View.Details
{
    public partial class frmAuthorDetails : Form
    {
        private AuthorRepository authorRepository = new AuthorRepository();
        private MusicRepository musicRepository = new MusicRepository();

        public frmAuthorDetails(int authorId)
        {
            InitializeComponent();

            Author author = authorRepository.FindById(authorId, lblAuthorName);
            lblAuthorBirthdate.Text = author.birthDate.ToString();
            lblAverageScore.Text = authorRepository.GetMusicsReviewsAverageScore(authorId).ToString("N2");


            string imageUrl = author.imgUrl;
            string localImagePath = Path.Combine(Application.StartupPath, "images", " '" + author.name + "' '" + DateTime.Now.Ticks + "'.png"); // Caminho local para salvar a imagem
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

            FillMusicsDataGridView(author.id, dataGridView1);
            FillTopThreeMusicsDataGridView(author, dataGridView2);
            FillAlbumsDataGridView(author, dataGridView3);

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView3.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView3.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView3.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);
            dataGridView2.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);
            dataGridView3.DefaultCellStyle.ForeColor = Color.FromArgb(75, 75, 75);

            // Fonte
            //dataGridView2.DefaultCellStyle.Font = new Font("Century Gothic",8);

            // Bordas
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView3.CellBorderStyle = DataGridViewCellBorderStyle.None;

            // Cabeçalho
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);
            dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8, FontStyle.Bold);

            dataGridView1.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho
            dataGridView2.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho
            dataGridView3.EnableHeadersVisualStyles = false; // Habilita a edição do cabeçalho

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            #endregion
        }

        public void FillMusicsDataGridView(long authorId, DataGridView dataGridView)
        {
            List<long> musicsIds = authorRepository.GetMusicsIds(authorId);

            dataGridView.Rows.Clear();
            foreach (long musicId in musicsIds)
            {
                MusicDTO music = musicRepository.FindById((int)musicId);
                dataGridView.Rows.Add(music.id, music.name, music.isExplicit, music.year, music.minutes, music.seconds, music.styleId, music.albumId, music.averageScore);
            }
        }

        public void FillTopThreeMusicsDataGridView(Author author, DataGridView dataGridView)
        {
            List<MusicDTO> musics = author.getTopThreeMusics();
            dataGridView.Rows.Clear();
            foreach (MusicDTO music in musics)
            {
                dataGridView.Rows.Add(music.id, music.name, music.isExplicit, music.year, music.minutes, music.seconds, music.styleId, music.albumId, music.averageScore);
            }
        }

        public void FillAlbumsDataGridView(Author author, DataGridView dataGridView)
        {
            foreach (Album album in author.albums)
            {
                dataGridView.Rows.Add(album.id, album.name, album.year, album.imgUrl);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView3.Rows.Count)
                {
                    DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];
                    string idCellValue = selectedRow.Cells[0].Value.ToString();

                    if (int.TryParse(idCellValue, out int albumId))
                    {
                        frmAlbumDetails f = new frmAlbumDetails(albumId);
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

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}
