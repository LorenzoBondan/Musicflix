using Music_Flix.Dtos;
using Music_Flix.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Music_Flix.View
{
    public partial class frmAdminStyles : Form
    {
        private StyleRepository repository = new StyleRepository();

        public frmAdminStyles()
        {
            InitializeComponent();
            repository.CreateDatabase();
            repository.FindAll(dataGridView1);

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
            StyleDTO styleDTO = new StyleDTO();
            styleDTO.description = txtDescription.Text;
            repository.Insert(styleDTO, dataGridView1, labelResultado);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                StyleDTO styleDTO = new StyleDTO();
                styleDTO.id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                styleDTO.description = txtDescription.Text;
                repository.Update(styleDTO, dataGridView1, labelResultado);
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
                string description = selectedRow.Cells[1].Value.ToString();
                txtDescription.Text = description;
            }
            else
            {
                txtDescription.Text = string.Empty;
            }
        }
    }
}
