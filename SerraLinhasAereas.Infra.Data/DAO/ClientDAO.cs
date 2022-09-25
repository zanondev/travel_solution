using SerraLinhasAereas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SerraLinhasAereas.Infra.Data.DAO
{
    public class ClientDAO
    {
        private const string _connectionString = @"Data Source=.\SQLEXPRESS;initial catalog=SERRALINHASAEREASDB;uid=sa;pwd=tunico;";

        public void RegisterClient(Clients newClient)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"INSERT CLIENTS VALUES (@CPF, @CLIENT_NAME, @NICKNAME, @CEP, @STREET, @DISTRICT, @NUMBER, @COMPLEMENT);";
                    ConvertObjectToSql(newClient, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();


                }
            }
        }
        public Clients GetClientByCpf(string cpf)
        {
            var wantedClient = new Clients();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, CPF, CLIENT_NAME, NICKNAME, CEP, STREET, DISTRICT, NUMBER, COMPLEMENT FROM CLIENTS WHERE CPF = @CPF";
                    comando.CommandText = sql;
                    comando.Parameters.AddWithValue("@CPF", cpf);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        wantedClient = ConvertSqlToObjetc(reader);
                    };
                    return wantedClient;
                }
            }
        }
        public Clients GetClientById(int id)
        {
            var wantedClient = new Clients();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, CPF, CLIENT_NAME, NICKNAME, CEP, STREET, DISTRICT, NUMBER, COMPLEMENT FROM CLIENTS WHERE ID = @ID";
                    comando.CommandText = sql;
                    comando.Parameters.AddWithValue("@ID", id);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        wantedClient = ConvertSqlToObjetc(reader);
                    };
                    return wantedClient;
                }
            }
        }
        public List<Clients> GetAllClients()
        {
            var clientList = new List<Clients>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT * FROM CLIENTS";
                    DoCommand.CommandText = sql;
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Clients wantedClient = ConvertSqlToObjetc(reader);
                        clientList.Add(wantedClient);
                    }
                }
            }
            return clientList;
        }
        public void DeleteCliente(string cpf)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"DELETE FROM CLIENTS WHERE CPF = @CPF;";
                    DoCommand.Parameters.AddWithValue("@CPF", cpf);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
        public void UpdateClient(Clients wantedClient)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"UPDATE CLIENTS SET            
                                        CPF = @CPF,
                                        CLIENT_NAME = @CLIENT_NAME,
                                        NICKNAME = @NICKNAME,
                                        CEP = @CEP,
                                        STREET = @STREET,
                                        DISTRICT = @DISTRICT,
                                        NUMBER = @NUMBER,
                                        COMPLEMENT = @COMPLEMENT
                                 WHERE ID = @ID;";
                    DoCommand.Parameters.AddWithValue("@ID", wantedClient.Id);
                    ConvertObjectToSql(wantedClient, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
        private Clients ConvertSqlToObjetc(SqlDataReader reader)
        {
            Clients wantedClient = new Clients();
            wantedClient.Id = Convert.ToInt32(reader["ID"].ToString());
            wantedClient.Cpf = reader["CPF"].ToString();
            wantedClient.Name = reader["CLIENT_NAME"].ToString();
            wantedClient.Nickname = reader["NICKNAME"].ToString();
            wantedClient.Address = new Address()
            {
                Cep = reader["CEP"].ToString(),
                Street = reader["STREET"].ToString(),
                District = reader["DISTRICT"].ToString(),
                Number = Convert.ToInt32(reader["NUMBER"].ToString()),
                Complement = reader["COMPLEMENT"].ToString(),
            };
            return wantedClient;
        }
        private void ConvertObjectToSql(Clients newClient, SqlCommand doCommand)
        {
            doCommand.Parameters.AddWithValue("@CPF", newClient.Cpf);
            doCommand.Parameters.AddWithValue("@CLIENT_NAME", newClient.Name);
            doCommand.Parameters.AddWithValue("@NICKNAME", newClient.Nickname);
            doCommand.Parameters.AddWithValue("@CEP", newClient.Address.Cep);
            doCommand.Parameters.AddWithValue("@STREET", newClient.Address.Street);
            doCommand.Parameters.AddWithValue("@DISTRICT", newClient.Address.District);
            doCommand.Parameters.AddWithValue("@NUMBER", newClient.Address.Number);
            doCommand.Parameters.AddWithValue("@COMPLEMENT", newClient.Address.Complement);
        }


    }


}


