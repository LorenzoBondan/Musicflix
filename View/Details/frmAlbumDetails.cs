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
    public partial class frmAlbumDetails : Form
    {
        private AlbumRepository albumRepository = new AlbumRepository();
        private MusicRepository musicRepository = new MusicRepository();

        public frmAlbumDetails(int albumId)
        {
            InitializeComponent();
            Album album = albumRepository.FindById(albumId, lblAlbumName);
            lblAlbumYear.Text = album.year.ToString();

            string imageUrl = album.imgUrl;
            string localImagePath = Path.Combine(Application.StartupPath, "images", " '" + album.name + "' '" + DateTime.Now.Ticks + "'.png"); // Caminho local para salvar a imagem
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

            FillMusicsDataGridView(albumId, dataGridView1);

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

        public void FillMusicsDataGridView(long albumId, DataGridView dataGridView)
        {
            List<long> musicsIds = albumRepository.GetMusicsIds(albumId);

            dataGridView.Rows.Clear();
            foreach (long musicId in musicsIds)
            {
                MusicDTO music = musicRepository.FindById((int)musicId);
                dataGridView.Rows.Add(music.id, music.name, music.isExplicit, music.year, music.minutes, music.seconds, music.styleId, music.albumId, music.averageScore);
            }
        }
    }
}
