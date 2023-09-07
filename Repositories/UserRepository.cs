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
    public class UserRepository
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

                comando.CommandText = "CREATE TABLE tb_user (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, name NVARCHAR(60), email NVARCHAR(60) UNIQUE, password NVARCHAR(60), imgUrl NVARCHAR(256), admin NVARCHAR(3))";
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

        public List<UserDTO> FindAll(DataGridView dataGridView = null)
        {
            List<UserDTO> users = new List<UserDTO>();

            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
            }

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + ";Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                string query = "SELECT id, name, email, imgUrl, admin FROM tb_user";

                DataTable dados = new DataTable();

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConnection);

                conexao.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    UserDTO userDTO = new UserDTO
                    {
                        id = Convert.ToInt32(linha["Id"]),
                        name = linha["Name"].ToString(),
                        email = linha["Email"].ToString(),
                        imgUrl = linha["ImgUrl"].ToString(),
                        admin = linha["Admin"].ToString()
                    };
                    users.Add(userDTO);

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

            return users;
        }

        public User FindById(int id, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);
            User userEncontrado = null;

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, email, imgUrl, admin FROM tb_user WHERE id = '" + id + "' ";

                SqlCeDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    userEncontrado = new User
                    {
                        id = Convert.ToInt32(reader["id"]),
                        name = reader["name"].ToString(),
                        email = reader["Email"].ToString(),
                        imgUrl = reader["ImgUrl"].ToString(),
                        admin = reader["Admin"].ToString()
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
                    labelResult.Text = userEncontrado.name;
                }
                conexao.Close();
            }

            return userEncontrado;
        }

        public void Insert(UserInsertDTO user, DataGridView dataGridView = null, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                string admin = "N"; // by default, user is created with nonAdmin role
                comando.CommandText = "INSERT INTO tb_user (name, password, email, imgUrl, admin) " +
                    "VALUES ('" + user.name + "' , '" + user.password + "', '" + user.email + "', '" + user.imgUrl + "', '" + admin + "')";

                comando.ExecuteNonQuery();

                if (labelResult != null)
                {
                    labelResult.Text = "Registro inserido.";
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
                conexao.Close();
                if (dataGridView != null)
                {
                    FindAll(dataGridView);
                }
            }
        }

        public void Update(UserDTO userDTO, DataGridView dataGridView = null, Label labelResult = null)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                // email and password are nonEditable fields
                comando.CommandText = "UPDATE tb_user SET " +
                    "name = '" + userDTO.name + "', " +
                    "imgUrl = '" + userDTO.imgUrl + "', " + 
                    "admin = '" + userDTO.admin + "' " +   
                    "WHERE email = '" + userDTO.email + "' "; 
                comando.ExecuteNonQuery();

                if (labelResult != null)
                {
                    labelResult.Text = "Registro alterado na base de dados!";
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
                conexao.Close();
                if (dataGridView != null)
                {
                    FindAll(dataGridView);
                }
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

                    comando.CommandText = "DELETE FROM tb_user WHERE id = '" + id + "' ";

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
            List<UserDTO> usersDTO = new List<UserDTO>();

            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConnection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConnection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, name, email, imgUrl, admin FROM tb_user WHERE name LIKE '" + name + "%' ";

                DataTable dt = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                adaptador.Fill(dt);

                dataGridView.Rows.Clear();

                usersDTO.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    UserDTO userDTO = new UserDTO
                    {
                        id = Convert.ToInt32(row["id"]),
                        name = row["name"].ToString(),
                        email = row["Email"].ToString(),
                        imgUrl = row["ImgUrl"].ToString(),
                        admin = row["Admin"].ToString()
                    };
                    usersDTO.Add(userDTO);

                    dataGridView.Rows.Add(userDTO.id, userDTO.name, userDTO.email, userDTO.imgUrl, userDTO.admin);
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

        ////// ##### User - Music Relation Many to Many
        
        public void CreateUserMusicDatabase()
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

                comando.CommandText = "CREATE TABLE tb_user_music " +
                    "(user_id INT NOT NULL, music_id INT NOT NULL, " +
                    "PRIMARY KEY (user_id, music_id), " +
                    "FOREIGN KEY (user_id) REFERENCES tb_user (id), " +
                    "FOREIGN KEY (music_id) REFERENCES tb_music (id) )";
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

        public void InsertUserMusic(int userId, int musicId, Action<DataGridView> findAll, DataGridView dataGridView, Label labelResult)
        {
            string baseDados = Application.StartupPath + @"\BancoDeDados.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "INSERT INTO tb_user_music (user_id, music_id) VALUES ('" + userId + "', '" + musicId + "')";
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
                findAll(dataGridView);
            }
        }

        public void DeleteUserMusic(int userId, int musicId, Action<DataGridView> findAll, DataGridView dataGridView, Label labelResult)
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

                    comando.CommandText = "DELETE FROM tb_user_music WHERE user_id = '" + userId + "' AND music_id = '" + musicId + "'";
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
                    findAll(dataGridView);
                }
            }
        }
    }
}
