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
    public class MusicRepository
    {
        private StyleRepository styleRepository = new StyleRepository();
        private AlbumRepository albumRepository = new AlbumRepository();
        private AuthorRepository authorRepository = new AuthorRepository();

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

                comando.CommandText = "CREATE TABLE tb_music " +
                    "(id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, " +
                    "name NVARCHAR(60), " +
                    "isExplicit NVARCHAR(1), " +
                    "year INT, " +
                    "minutes INT, " +
                    "seconds INT, " +
                    "styleId INT, " +
                    "albumId INT, " +
                    "FOREIGN KEY (styleId) REFERENCES tb_style (id), " +
                    "FOREIGN KEY (albumId) REFERENCES tb_album (id) )";
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

        public List<MusicDTO> FindAll(DataGridView dataGridView = null)
        {
            List<MusicDTO> musics = new List<MusicDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_music"; 

                DataTable dados = new DataTable(); 

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection); 

                conexao.Open(); 

                adaptador.Fill(dados); 

                foreach (DataRow linha in dados.Rows) 
                {
                    MusicDTO musicDTO = new MusicDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        name = linha["Name"].ToString(),
                        isExplicit = linha["isExplicit"].ToString()[0],
                        year = Convert.ToInt32(linha["year"]),
                        minutes = Convert.ToInt32(linha["minutes"]),
                        seconds = Convert.ToInt32(linha["seconds"]),
                        styleId = Convert.ToInt32(linha["styleId"]),
                        albumId = Convert.ToInt32(linha["albumId"])
                    };
                    musics.Add(musicDTO);

                    double averageScore = musicDTO.averageScore;

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                        dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["AverageScore"].Value = averageScore;
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
                dataGridView?.ClearSelection();
            }

            return musics;
        }

        public List<MusicDTO> FindAllByStyle(string styleDescription, DataGridView dataGridView = null)
        {
            List<MusicDTO> musics = new List<MusicDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_music m INNER JOIN tb_style s ON m.styleId = s.id WHERE s.description LIKE '" + styleDescription + "' ";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    MusicDTO musicDTO = new MusicDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        name = linha["Name"].ToString(),
                        isExplicit = linha["isExplicit"].ToString()[0],
                        year = Convert.ToInt32(linha["year"]),
                        minutes = Convert.ToInt32(linha["minutes"]),
                        seconds = Convert.ToInt32(linha["seconds"]),
                        styleId = Convert.ToInt32(linha["styleId"]),
                        albumId = Convert.ToInt32(linha["albumId"])
                    };
                    musics.Add(musicDTO);

                    double averageScore = musicDTO.averageScore;

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                        dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["AverageScore"].Value = averageScore;
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
                dataGridView?.ClearSelection();
            }

            return musics;
        }

        public MusicDTO FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            Music musicEncontrada = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, isExplicit, year, minutes, seconds, styleId, albumId FROM tb_music WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    musicEncontrada = new Music
                    {
                        id = Convert.ToInt32(reader["id"]),
                        name = reader["name"].ToString(),
                        isExplicit = reader["isExplicit"].ToString()[0],
                        year = Convert.ToInt32(reader["year"]),
                        minutes = Convert.ToInt32(reader["minutes"]),
                        seconds = Convert.ToInt32(reader["seconds"]),
                        style = styleRepository.FindById(Convert.ToInt32(reader["styleId"])),
                        album = albumRepository.FindById(Convert.ToInt32(reader["albumId"])),
                        authors = new List<Author>()
                    };
                }

                comando.Dispose();

                // Get the authors 

                SqlCeCommand comando2 = new SqlCeCommand();
                comando2.Connection = conexao;

                comando2.CommandText = "SELECT author_id FROM tb_music_author WHERE music_id = '" + id + "'";
                SqlCeDataReader reader2 = comando2.ExecuteReader();

                while (reader2.Read())
                {
                    int authorIdEncontrado = Convert.ToInt32(reader2["author_id"]);
                    musicEncontrada.authors.Add(authorRepository.FindById(authorIdEncontrado));
                }
                reader2.Close();
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
                    labelResult.Text = musicEncontrada.name;
                }
                conexao.Close();
            }

            return new MusicDTO(musicEncontrada);
        }

        public void Insert(MusicDTO musicDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_music (name, isExplicit, year, minutes, seconds, styleId, albumId) " +
                    "VALUES (" +
                    "'" + musicDTO.name + "', " +
                    "'" + musicDTO.isExplicit + "', " +
                    "'" + musicDTO.year + "', " +
                    "'" + musicDTO.minutes + "', " +
                    "'" + musicDTO.seconds + "', " +
                    "'" + musicDTO.styleId + "', " +
                    "'" + musicDTO.albumId + "')";
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

        public void Update(MusicDTO musicDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_music SET " +
                    "name = '" + musicDTO.name + "', " +
                    "isExplicit = '" + musicDTO.isExplicit + "', " +
                    "year = '" + musicDTO.year + "', " +
                    "minutes = '" + musicDTO.minutes + "', " +
                    "seconds = '" + musicDTO.seconds + "', " +
                    "styleId = '" + musicDTO.styleId + "', " +
                    "albumId = '" + musicDTO.albumId + "' " +
                    "WHERE id = '" + musicDTO.id + "' ";
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

                    comando.CommandText = "DELETE FROM tb_music WHERE id = '" + id + "' ";

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

        public void Filter(string name, DataGridView dataGridView, Label labelResult)
        {
            List<MusicDTO> musicsDTO = new List<MusicDTO>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, isExplicit, year, minutes, seconds, styleId, albumId FROM tb_music WHERE name LIKE '" + name + "%' ";

                DataTable dt = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                adaptador.Fill(dt);

                dataGridView.Rows.Clear();

                musicsDTO.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    MusicDTO musicDTO = new MusicDTO
                    {
                        id = Convert.ToInt32(row["id"]),
                        name = row["name"].ToString(),
                        isExplicit = row["isExplicit"].ToString()[0],
                        year = Convert.ToInt32(row["year"]),
                        minutes = Convert.ToInt32(row["minutes"]),
                        seconds = Convert.ToInt32(row["seconds"]),
                        styleId = Convert.ToInt32(row["styleId"]),
                        albumId = Convert.ToInt32(row["albumId"])
                    };
                    musicsDTO.Add(musicDTO);
                    
                    dataGridView.Rows.Add(musicDTO.id, musicDTO.name, musicDTO.isExplicit, musicDTO.year, musicDTO.minutes, musicDTO.seconds, musicDTO.styleId, musicDTO.albumId, musicDTO.averageScore);
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

        // ################ Music - Author Relation Many to many

        public void CreateMusicAuthorDatabase() 
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

                comando.CommandText = "CREATE TABLE tb_music_author " +
                    "(music_id INT NOT NULL, author_id INT NOT NULL, " +
                    "PRIMARY KEY (music_id, author_id), " +
                    "FOREIGN KEY (music_id) REFERENCES tb_music (id), " +
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

        public void FindAllMusicAuthor(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_music_author";

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

        public void InsertMusicAuthor(int musicId, int authorId, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_music_author (music_id, author_id) VALUES ('" + musicId + "', '" + authorId + "')";
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
                FindAllMusicAuthor(dataGridView);
            }
        }

        public void UpdateMusicAuthor(int musicId, int authorId, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_music_author SET music_id = '" + musicId + "', author_id = '" + authorId + "'";
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
                FindAllMusicAuthor(dataGridView);
            }
        }

        public void DeleteMusicAuthor(int musicId, int authorId, DataGridView dataGridView, Label labelResult)
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

                    comando.CommandText = "DELETE FROM tb_music_author WHERE music_id = '" + musicId + "' AND author_id = '" + authorId + "'";
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
                    FindAllMusicAuthor(dataGridView);
                }
            }
        }
    }
}
