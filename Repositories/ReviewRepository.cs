using Music_Flix.Dtos;
using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;

namespace Music_Flix.Repositories
{
    public class ReviewRepository
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

                comando.CommandText = "CREATE TABLE tb_review " +
                    "(id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, " +
                    "text NVARCHAR(256), " +
                    "moment NVARCHAR(30), " +
                    "score INT, " +
                    "userId INT, musicId INT, " +
                    "FOREIGN KEY (userId) REFERENCES tb_user (id), " +
                    "FOREIGN KEY (musicId) REFERENCES tb_music (id) )";
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

        public List<ReviewDTO> FindAllReviewsByUser(int userId, DataGridView dataGridView = null)
        {
            List<ReviewDTO> reviews = new List<ReviewDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_review r WHERE r.userId = '" + userId + "' ";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    ReviewDTO reviewDTO = new ReviewDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        text = linha["text"].ToString(),
                        moment = linha["moment"].ToString(),
                        score = Convert.ToInt32(linha["score"]),
                        userId = Convert.ToInt32(linha["userId"]),
                        musicId = Convert.ToInt32(linha["musicId"])
                    };
                    reviews.Add(reviewDTO);

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear();
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            return reviews;
        }

        public List<ReviewDTO> FindAllReviewsByMusic(int musicId, DataGridView dataGridView = null)
        {
            List<ReviewDTO> reviews = new List<ReviewDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_review r WHERE r.musicId = '" + musicId + "' ";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    ReviewDTO reviewDTO = new ReviewDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        text = linha["text"].ToString(),
                        moment = linha["moment"].ToString(),
                        score = Convert.ToInt32(linha["score"]),
                        userId = Convert.ToInt32(linha["userId"]),
                        musicId = Convert.ToInt32(linha["musicId"])
                    };
                    reviews.Add(reviewDTO);

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear();
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            return reviews;
        }

        public ReviewDTO FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            ReviewDTO reviewEncontrado = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT * FROM tb_review WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    reviewEncontrado = new ReviewDTO
                    {
                        id = Convert.ToInt32(reader["Id"]),
                        text = reader["text"].ToString(),
                        moment = reader["moment"].ToString(),
                        score = Convert.ToInt32(reader["score"]),
                        userId = Convert.ToInt32(reader["userId"]),
                        musicId = Convert.ToInt32(reader["musicId"])
                    };
                }

                comando.Dispose();
            }
            catch (Exception ex)
            {
                if (labelResult != null)
                {
                    labelResult.Text = ex.Message;
                }

            }
            finally
            {
                if (labelResult != null)
                {
                    labelResult.Text = reviewEncontrado.text;
                }
                conexao.Close();
            }

            return reviewEncontrado;
        }

        public void Insert(ReviewDTO reviewDTO, Action<DataGridView> findAll = null, DataGridView dataGridView = null, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                // Verifique se já existe uma revisão para esse usuário e música
                SqlCeCommand verificaExistencia = new SqlCeCommand();
                verificaExistencia.Connection = conexao;
                verificaExistencia.CommandText = "SELECT COUNT(*) FROM tb_review WHERE userId = '" + reviewDTO.userId + "' AND musicId = '" + reviewDTO.musicId + "'";

                int existencia = (int)verificaExistencia.ExecuteScalar();

                if (existencia > 0 && labelResult != null)
                {
                    labelResult.Text = "Música já avaliada.";
                }
                else
                {
                    SqlCeCommand comando = new SqlCeCommand();
                    comando.Connection = conexao;

                    comando.CommandText = "INSERT INTO tb_review (text, moment, score, userId, musicId) " +
                        "VALUES ('" + reviewDTO.text + "' , '" + reviewDTO.moment + "', '" + reviewDTO.score + "' , '" + reviewDTO.userId + "', '" + reviewDTO.musicId + "')";

                    comando.ExecuteNonQuery();

                    if (labelResult != null)
                    {
                        labelResult.Text = "Avaliação inserida.";
                    }
                    comando.Dispose();
                }
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
            }
            finally
            {
                conexao.Close();
                if (findAll != null && dataGridView != null)
                {
                    findAll(dataGridView); // inserir o método de atualização de dados necessário
                }
            }
        }

        public void Delete(int id, Action<DataGridView> findAll, DataGridView dataGridView, Label labelResult)
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

                    comando.CommandText = "DELETE FROM tb_review WHERE id = '" + id + "' ";

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
                    findAll(dataGridView);
                }
            }
        }
    }
}
