using Music_Flix.Dtos;
using System;
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

        public void Insert(ReviewDTO reviewDTO, Action<DataGridView> findAll, DataGridView dataGridView, Label labelResult)
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

                if (existencia > 0)
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

                    labelResult.Text = "Registro inserido.";
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
                findAll(dataGridView); // inserir o método de atualização de dados necessário
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
