﻿using Music_Flix.Dtos;
using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Music_Flix.Repositories
{
    public class AuthorRepository
    {
        private AlbumRepository albumRepository = new AlbumRepository();
        private ReviewRepository reviewRepository = new ReviewRepository();

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

                comando.CommandText = "CREATE TABLE tb_author (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, name NVARCHAR(60), birthDate NVARCHAR(10), imgUrl NVARCHAR(256))";
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

        public List<AuthorDTO> FindAll(DataGridView dataGridView = null)
        {
            List<AuthorDTO> authors = new List<AuthorDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT * FROM tb_author"; 

                DataTable dados = new DataTable(); 

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection); 

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows) 
                {
                    AuthorDTO authorDTO = new AuthorDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        name = linha["name"].ToString(),
                        birthDate = linha["birthDate"].ToString(), 
                        imgUrl = linha["imgUrl"].ToString()
                    };
                    authors.Add(authorDTO);

                    double averageScore = 0;

                    List<MusicDTO> musicList = FindAllByAuthor((int)authorDTO.id);
                    foreach (MusicDTO music in musicList)
                    {
                        averageScore += music.averageScore;
                    }
                    averageScore = averageScore / musicList.Count;
                    averageScore = Math.Round(averageScore, 2); // arredonda para duas casas decimais

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
            }

            return authors;
        }

        public Author FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            Author authorEncontrado = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, birthDate, imgUrl FROM tb_author WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    authorEncontrado = new Author
                    {
                        id = Convert.ToInt32(reader["id"]),
                        name = reader["name"].ToString(),
                        birthDate = reader["birthDate"].ToString(), 
                        imgUrl = reader["imgUrl"].ToString(),
                        musics = new List<Music>()
                    };
                }

                comando.Dispose();

                // Get the musics in a separated method

                // Get the albums 

                SqlCeCommand comando2 = new SqlCeCommand();
                comando2.Connection = conexao;

                comando2.CommandText = "SELECT album_id FROM tb_album_author WHERE author_id = '" + id + "'";
                SqlCeDataReader reader2 = comando2.ExecuteReader();

                while (reader2.Read())
                {
                    int albumIdEncontrado = Convert.ToInt32(reader2["album_id"]);
                    authorEncontrado.albums.Add(albumRepository.FindById(albumIdEncontrado));
                }
                reader2.Close();
            }
            catch (Exception ex)
            {
                if (labelResult != null)
                {
                    labelResult.Text = ex.Message;
                    labelResult.ForeColor = System.Drawing.Color.DarkRed;
                }

            }
            finally
            {
                if (labelResult != null)
                {
                    labelResult.Text = authorEncontrado.name;
                    labelResult.ForeColor = System.Drawing.Color.DarkGreen;
                }
                conexao.Close();
            }

            return authorEncontrado;
        }

        public void Insert(AuthorDTO authorDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_author (name, birthDate, imgUrl) VALUES ('" + authorDTO.name + "', '" + authorDTO.birthDate + "', '" + authorDTO.imgUrl + "')";

                comando.ExecuteNonQuery();

                labelResult.Text = "Registro inserido.";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
                labelResult.ForeColor = System.Drawing.Color.DarkRed;
            }
            finally
            {
                conexao.Close();
                FindAll(dataGridView);
            }
        }

        public void Update(AuthorDTO authorDTO, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "UPDATE tb_author SET name = '" + authorDTO.name + "', birthDate = '" + authorDTO.birthDate + "', imgUrl = '" + authorDTO.imgUrl + "' WHERE id = '" + authorDTO.id + "' ";
                comando.ExecuteNonQuery();

                labelResult.Text = "Registro alterado na base de dados!";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
                labelResult.ForeColor = System.Drawing.Color.DarkRed;
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

                    comando.CommandText = "DELETE FROM tb_author WHERE id = '" + id + "' ";

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
            List<AuthorDTO> authorsDTO = new List<AuthorDTO>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, birthDate, imgUrl FROM tb_author WHERE name LIKE '" + name + "%' ";

                DataTable dt = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                adaptador.Fill(dt);

                dataGridView.Rows.Clear();

                authorsDTO.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    AuthorDTO authorDTO = new AuthorDTO
                    {
                        id = Convert.ToInt32(row["id"]),
                        name = row["description"].ToString(),
                        birthDate = row["birthDate"].ToString(), 
                        imgUrl = row["imgUrl"].ToString()
                    };
                    authorsDTO.Add(authorDTO);

                    dataGridView.Rows.Add(authorDTO.id, authorDTO.name, authorDTO.birthDate, authorDTO.imgUrl);
                }

                comando.Dispose();
            }
            catch (Exception ex)
            {
                labelResult.Text = ex.Message;
                labelResult.ForeColor = System.Drawing.Color.DarkRed;
            }
            finally
            {
                conexao.Close();
            }
        }

        public List<long> GetMusicsIds(long authorId) // separated method with musics Ids to don't throw stack over flow error
        {
            List<long> musicsIds = new List<long>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            
            try
            {
                conexao.Open();

                SqlCeCommand comando2 = new SqlCeCommand();
                comando2.Connection = conexao;

                comando2.CommandText = "SELECT music_id FROM tb_music_author WHERE author_id = '" + authorId + "'";
                SqlCeDataReader reader2 = comando2.ExecuteReader();

                while (reader2.Read())
                {
                    int musicIdEncontrada = Convert.ToInt32(reader2["music_id"]);
                    musicsIds.Add(musicIdEncontrada);
                }
                reader2.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            return musicsIds;
        }

        public double GetMusicsReviewsAverageScore(int authorId)
        {
            double scoreSum = 0;
            int count = 0;

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando2 = new SqlCeCommand();
                comando2.Connection = conexao;

                comando2.CommandText = "SELECT r.score as score FROM tb_review r " +
                    "INNER JOIN tb_music m ON r.musicId = m.id " +
                    "INNER JOIN tb_music_author ma ON ma.music_id = m.id " +
                    "WHERE ma.author_id = '" + authorId + "' ";
                SqlCeDataReader reader2 = comando2.ExecuteReader();

                while (reader2.Read())
                {
                    scoreSum += Convert.ToInt32(reader2["score"]);
                    count++;
                }
                reader2.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            if (count > 0)
            {
                double averageScore = scoreSum / count;
                return averageScore;
            }
            else
            {
                return 0;
            }
        }

        public List<MusicDTO> FindAllByAuthor(int authorId, DataGridView dataGridView = null)
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
                string query = "SELECT * FROM tb_music m INNER JOIN tb_music_author ma ON ma.music_id = m.id WHERE ma.author_id = '" + authorId + "' ";

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

                    double totalScore = 0;
                    int count = 0;

                    List<ReviewDTO> reviews = reviewRepository.FindAllReviewsByMusic((int)musicDTO.id);
                    foreach (ReviewDTO review in reviews)
                    {
                        totalScore += review.score;
                        count++;
                    }
                    musicDTO.averageScore = totalScore / count;

                    if (dataGridView != null)
                    {
                        dataGridView.Rows.Add(linha.ItemArray);
                        if (musicDTO.averageScore > 0)
                        {
                            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["AverageScore"].Value = musicDTO.averageScore;
                        }
                        else
                        {
                            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["AverageScore"].Value = 0;
                        }
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
    }
}
