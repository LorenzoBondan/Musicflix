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
    public class AlbumRepository
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

                comando.CommandText = "CREATE TABLE tb_album (id INT NOT NULL PRIMARY KEY, name NVARCHAR(60), year INT, imgUrl NVARCHAR(256))";
                comando.ExecuteNonQuery();
                MessageBox.Show("Table creaded");
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

        public List<AlbumDTO> FindAll(DataGridView dataGridView = null)
        {
            List<AlbumDTO> albums = new List<AlbumDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_album";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    AlbumDTO albumDTO = new AlbumDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        name = linha["Name"].ToString(),
                        year = Convert.ToInt32(linha["Year"]),
                        imgUrl = linha["ImgUrl"].ToString()
                    };
                    albums.Add(albumDTO);

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

            return albums;
        }

        public Album FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            Album albumEncontrado = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, year, imgUrl FROM tb_album WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    albumEncontrado = new Album
                    {
                        id = Convert.ToInt32(reader["id"]),
                        name = reader["name"].ToString(),
                        year = Convert.ToInt32(reader["year"]),
                        imgUrl = reader["imgUrl"].ToString()
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
                    labelResult.Text = albumEncontrado.name;
                }
                conexao.Close();
            }

            return albumEncontrado;
        }

        public void Insert(AlbumDTO albumDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_album (id, name, year, imgUrl) " +
                    "VALUES (" + albumDTO.id + ", '" + albumDTO.name + "' , '" + albumDTO.year + "', '" + albumDTO.imgUrl + "')";

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

        public void Update(AlbumDTO albumDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_album SET " +
                    "name = '" + albumDTO.name + "', " +
                    "year = '" + albumDTO.year + "', " +
                    "imgUrl = '" + albumDTO.imgUrl + "' " +
                    "WHERE id = '" + albumDTO.id + "' ";
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

                    comando.CommandText = "DELETE FROM tb_album WHERE id = '" + id + "' ";

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
            List<AlbumDTO> albumsDTO = new List<AlbumDTO>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, description FROM tb_album WHERE description LIKE '" + description + "%' ";

                DataTable dt = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                adaptador.Fill(dt);

                dataGridView.Rows.Clear();

                albumsDTO.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    AlbumDTO albumDTO = new AlbumDTO
                    {
                        id = Convert.ToInt32(row["id"]),
                        name = row["name"].ToString()
                    };
                    albumsDTO.Add(albumDTO);

                    dataGridView.Rows.Add(albumDTO.id, albumDTO.name);
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

        // ######### Album - Author Relation Many To Many

        public void CreateAlbumAuthorDatabase()
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

                comando.CommandText = "CREATE TABLE tb_album_author " +
                    "(album_id INT NOT NULL, author_id INT NOT NULL, " +
                    "PRIMARY KEY (album_id, author_id), " +
                    "FOREIGN KEY (album_id) REFERENCES tb_album (id), " +
                    "FOREIGN KEY (author_id) REFERENCES tb_author (id) )";
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

        public void FindAllAlbumAuthor(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_album_author";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    dataGridView.Rows.Add(linha.ItemArray);
                }
            }
            catch (Exception ex)
            {
                dataGridView.Rows.Clear();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public void InsertAlbumAuthor(int albumId, int authorId, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_album_author (album_id, author_id) VALUES ('" + albumId + "', '" + authorId + "')";
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
                FindAllAlbumAuthor(dataGridView);
            }
        }

        public void UpdateAlbumAuthor(int albumId, int authorId, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_album_author SET album_id = '" + albumId + "', author_id = '" + authorId + "'";
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
                FindAllAlbumAuthor(dataGridView);
            }
        }

        public void DeleteAlbumAuthor(int albumId, int authorId, DataGridView dataGridView, Label labelResult)
        {
            DialogResult dialogResult = MessageBox.Show("Deseja excluir o item selecionado?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
                string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

                SqlCeConnection conexao = new SqlCeConnection(strConection);

                try
                {
                    conexao.Open();

                    SqlCeCommand comando = new SqlCeCommand();
                    comando.Connection = conexao;

                    comando.CommandText = "DELETE FROM tb_album_author WHERE album_id = '" + albumId + "' AND author_id = '" + authorId + "'";
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
                    FindAllAlbumAuthor(dataGridView);
                }
            }
        }
    }
}
