using Music_Flix.Dtos;
using Music_Flix.Entities;
using Music_Flix.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Music_Flix.View.Profile
{
    public partial class frmProfile : Form
    {
        private UserRepository repository = new UserRepository();
        private ReviewRepository reviewRepository = new ReviewRepository();
        private MusicRepository musicRepository = new MusicRepository();

        public User userLogged;

        public frmProfile(User user)
        {
            userLogged = user;
            InitializeComponent();

            lblName.Text = user.name;
            lblEmail.Text = user.email;
            txtImgUrl.Text = user.imgUrl;

            LoadUserProfilePicture(user.imgUrl);

            FillReviewsDataGridView(user.id, dataGridView2);
            FillFavoritedMusicsDataGridView(user.id, dataGridView1);

            #region CUSTOMIZAÇÃO DO DATAGRID
            // Linhas alternadas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);
            // Linha selecionada
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 125, 33);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
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
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 21);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            #endregion
        }

        private void LoadUserProfilePicture(string imgUrl)
        {
            string localImagePath = Path.Combine(Application.StartupPath, "images", " profile '" + DateTime.Now.Ticks + "'.png"); // Caminho local para salvar a imagem
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(imgUrl, localImagePath);
                }
                pictureBox1.Image = Image.FromFile(localImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
            }
        }

        private void btnUpdateImage_Click(object sender, EventArgs e)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.email = userLogged.email; // used in update query
            userDTO.name = userLogged.name;
            userDTO.imgUrl = txtImgUrl.Text;
            userDTO.admin = userLogged.admin;
            repository.Update(userDTO);
            LoadUserProfilePicture(userDTO.imgUrl);
        }

        public void FillReviewsDataGridView(int userId, DataGridView dataGridView)
        {
            List<ReviewDTO> reviews = reviewRepository.FindAllReviewsByUser(userId);
            dataGridView.Rows.Clear();
            foreach (ReviewDTO review in reviews)
            {
                dataGridView.Rows.Add(review.id, review.text, review.moment, review.score, review.userId, review.musicId);
            }
        }

        public void FillFavoritedMusicsDataGridView(int userId, DataGridView dataGridView)
        {
            List<long> musicsIds = repository.GetUserFavoritedMusicsIds(userId);
            dataGridView.Rows.Clear();
            foreach (long musicId in musicsIds)
            {
                MusicDTO music = musicRepository.FindById((int)musicId);
                dataGridView.Rows.Add(music.id, music.name, music.isExplicit, music.year, music.minutes, music.seconds, music.styleId, music.albumId, music.averageScore);
            }
        }
    }
}
