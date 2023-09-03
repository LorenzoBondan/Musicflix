using Music_Flix.Dtos;
using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Music_Flix.Repositories
{
    public class StyleRepository
    {
        public void CreateDatabase()
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";
            SqlCeEngine db = new SqlCeEngine(strConnection);
            if (!File.Exists(baseDados))
            {
                db.CreateDatabase();
            }
            db.Dispose();
            SqlCeConnection conexao = new SqlCeConnection();
            conexao.ConnectionString = strConnection;
            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "CREATE TABLE tb_style (id INT NOT NULL PRIMARY KEY, description NVARCHAR(60))";
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
            finally
            {
                conexao.Close();
            }
        }

        public List<StyleDTO> FindAll(DataGridView dataGridView = null)
        {
            List<StyleDTO> styles = new List<StyleDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }
            
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_style"; // Consulta SQL para selecionar todos os dados da tabela com o nome informado pelo usuário 

                DataTable dados = new DataTable(); // Cria um objeto DataTable que irá armazenar os dados retornados pela consulta SQL 

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection); // Cria uma instância da classe SqlCeDataAdapter, que representa um conjunto de comandos e uma conexão usados para preencher um objeto DataTable

                conexao.Open(); // Abre a conexão com o banco

                adaptador.Fill(dados); // Preenche o objeto DataTable com os dados retornados pela consulta SQL

                foreach (DataRow linha in dados.Rows) // Para cada linha do dataTable
                {
                    StyleDTO style = new StyleDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        description = linha["Description"].ToString(),
                    };
                    styles.Add(style);

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                    }
                }
            }
            catch (Exception ex) // Em caso de erro
            {
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear(); // Limpar o dataGrid
                }
                MessageBox.Show(ex.Message); // Exibir o erro
            }
            finally
            {
                conexao.Close(); // Fecha a conexão com o banco
            }

            return styles;
        }

        public Style FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            Style styleEncontrado = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, description FROM tb_style WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    styleEncontrado = new Style
                    {
                        id = Convert.ToInt32(reader["id"]),
                        description = reader["description"].ToString()
                    };
                }

                comando.Dispose();
            }
            catch (Exception ex)
            {
                if(labelResult != null)
                {
                    labelResult.Text = ex.Message;
                }
                
            }
            finally
            {
                if(labelResult != null)
                {
                    labelResult.Text = styleEncontrado.description;
                }
                conexao.Close();
            }

            return styleEncontrado;
        }

        public void Insert(StyleDTO styleDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_style (id, description) VALUES (" + styleDTO.id + ", '" + styleDTO.description + "')";

                comando.ExecuteNonQuery();

                labelResult.Text = "Registro inserido.";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
            }
            finally
            {
                conexao.Close();
                FindAll(dataGridView);
            }
        }

        public void Update(StyleDTO styleDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_style SET description = '" + styleDTO.description + "' WHERE id = '" + styleDTO.id + "' ";
                comando.ExecuteNonQuery();

                labelResult.Text = "Registro alterado na base de dados!";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
            }
            finally
            {
                conexao.Close();
                FindAll(dataGridView);
            }
        }

        public void Delete(int id, DataGridView dataGridView, Label labelResult)
        {
            DialogResult dialogResult = MessageBox.Show("Deseja excluir o item selecionado?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
                string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

                SqlCeConnection conexao = new SqlCeConnection(strConnection);

                try
                {
                    conexao.Open();

                    SqlCeCommand comando = new SqlCeCommand();
                    comando.Connection = conexao;

                    comando.CommandText = "DELETE FROM tb_style WHERE id = '" + id + "' ";

                    comando.ExecuteNonQuery();
                    labelResult.Text = "Excluído.";
                    comando.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conexao.Close();
                    FindAll(dataGridView);
                }
            }
        }

        public void Filter(string description, DataGridView dataGridView, Label labelResult)
        {
            List<StyleDTO> stylesDTO = new List<StyleDTO>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, description FROM tb_style WHERE description LIKE '" + description + "%' ";

                DataTable dt = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                adaptador.Fill(dt);

                dataGridView.Rows.Clear();

                stylesDTO.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    StyleDTO styleDTO = new StyleDTO
                    {
                        id = Convert.ToInt32(row["id"]),
                        description = row["description"].ToString()
                    };
                    stylesDTO.Add(styleDTO);

                    dataGridView.Rows.Add(styleDTO.id, styleDTO.description);
                }

                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
