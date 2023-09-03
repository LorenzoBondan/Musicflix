using Music_Flix.Entities;
using Music_Flix.Repositories;
using System.IO;
using System.Net;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Music_Flix.View.Details
{
    public partial class frmAuthorDetails : Form
    {
        private AuthorRepository authorRepository = new AuthorRepository();
        

        public frmAuthorDetails(int authorId)
        {
            InitializeComponent();

            Author author = authorRepository.FindById(authorId, lblAuthorName);
            lblAuthorBirthdate.Text = author.birthDate.ToString();
            lblAverageScore.Text = author.getAverageScore().ToString();

            string imageUrl = author.imgUrl;
            string localImagePath = Path.Combine(Application.StartupPath, "images", " '" + author.name + "'.png"); // Caminho local para salvar a imagem
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

            FillMusicsDataGridView(author.musics, dataGridView1);

        }

        public void FillMusicsDataGridView(List<Music> musics, DataGridView dataGridView)
        {
            /*
            dataGridView.Rows.Clear();
            foreach (Music music in musics)
            {
                dataGridView.Rows.Add(music.id, music.name, music.isExplicit, music.year, music.minutes, music.seconds, music.style.id, music.album.id, music.getAverageScore());
            }
            */
        }


    }
}
