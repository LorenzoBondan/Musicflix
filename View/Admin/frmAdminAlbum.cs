﻿using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix.View
{
    public partial class frmAdminAlbum : Form
    {
        private AlbumRepository repository = new AlbumRepository();

        public frmAdminAlbum()
        {
            InitializeComponent();
            //repository.CreateDatabase();
            repository.FindAll(dataGridView1);

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
            AlbumDTO albumDTO = new AlbumDTO();
            albumDTO.name = txtName.Text;
            albumDTO.year = int.Parse(txtYear.Text);
            albumDTO.imgUrl = txtImgUrl.Text;
            repository.Insert(albumDTO, dataGridView1, labelResultado);
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                AlbumDTO albumDTO = new AlbumDTO();
                albumDTO.id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                albumDTO.name = txtName.Text;
                albumDTO.year = int.Parse(txtYear.Text);
                albumDTO.imgUrl = txtImgUrl.Text;
                repository.Update(albumDTO, dataGridView1, labelResultado);
                ClearFields();
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
                txtYear.Text = selectedRow.Cells[2].Value.ToString();
                txtImgUrl.Text = selectedRow.Cells[3].Value.ToString();
            }
            else
            {
                txtName.Text = string.Empty;
                txtYear.Text = string.Empty;
                txtImgUrl.Text = string.Empty;
            }
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtImgUrl.Text = "";
            txtYear.Text = "";
            txtName.Focus();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
