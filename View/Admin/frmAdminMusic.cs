using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix.View
{
    public partial class frmAdminMusic : Form
    {
        private MusicRepository repository = new MusicRepository();
        // carregar as comboboxes com todos os styles and albums
        private StyleRepository styleRepository = new StyleRepository();
        private AlbumRepository albumRepository = new AlbumRepository();

        public frmAdminMusic()
        {
            InitializeComponent();
            repository.CreateDatabase();
            repository.FindAll(dataGridView1);
            FillStyleComboBox(cbStyle);
            FillAlbumComboBox(cbAlbum);

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
            MusicDTO musicDTO = new MusicDTO();
            musicDTO.name = txtName.Text;
            musicDTO.year = int.Parse(txtYear.Text);
            musicDTO.isExplicit = cbExplicit.Checked ? 'Y' : 'N';
            musicDTO.minutes = int.Parse(txtMinutes.Text);
            musicDTO.seconds = int.Parse(txtSeconds.Text);
            if (cbStyle.SelectedItem != null && long.TryParse(cbStyle.SelectedValue.ToString(), out long styleId))
            {
                musicDTO.styleId = styleId;
            }
            if (cbAlbum.SelectedItem != null && long.TryParse(cbAlbum.SelectedValue.ToString(), out long albumId))
            {
                musicDTO.albumId = albumId;
            }
            repository.Insert(musicDTO, dataGridView1, labelResultado);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                MusicDTO musicDTO = new MusicDTO();
                musicDTO.id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                musicDTO.name = txtName.Text;
                musicDTO.year = int.Parse(txtYear.Text);
                musicDTO.isExplicit = cbExplicit.Checked ? 'Y' : 'N';
                musicDTO.minutes = int.Parse(txtMinutes.Text);
                musicDTO.seconds = int.Parse(txtSeconds.Text);
                if (cbStyle.SelectedItem != null && long.TryParse(cbStyle.SelectedValue.ToString(), out long styleId))
                {
                    musicDTO.styleId = styleId;
                }
                if (cbAlbum.SelectedItem != null && long.TryParse(cbAlbum.SelectedValue.ToString(), out long albumId))
                {
                    musicDTO.albumId = albumId;
                }
                repository.Update(musicDTO, dataGridView1, labelResultado);
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                txtName.Text = selectedRow.Cells[1].Value.ToString();
                cbExplicit.Checked = selectedRow.Cells[2].Value.ToString() == "Y" ? true : false; 
                txtYear.Text = selectedRow.Cells[3].Value.ToString();
                txtMinutes.Text = selectedRow.Cells[4].Value.ToString();
                txtSeconds.Text = selectedRow.Cells[5].Value.ToString();
                cbStyle.SelectedIndex = (int)selectedRow.Cells[6].Value - 1;
                cbAlbum.SelectedIndex = (int)selectedRow.Cells[7].Value - 1;
            }
            else
            {
                txtName.Text = string.Empty;
                txtYear.Text = string.Empty;
                cbExplicit.Checked = false;
                txtMinutes.Text = string.Empty;
                txtSeconds.Text = string.Empty;
                cbStyle.SelectedIndex = 0;
                cbAlbum.SelectedIndex = 0;
            }
        }

        private List<AlbumDTO> GetAlbums()
        {
            List<AlbumDTO> albums = albumRepository.FindAll();
            return albums;
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

        private void FillAlbumComboBox(ComboBox comboBox)
        {
            List<AlbumDTO> albums = GetAlbums();
            comboBox.DataSource = albums;
            comboBox.DisplayMember= "name";
            comboBox.ValueMember = "id";
        }
    }
}
