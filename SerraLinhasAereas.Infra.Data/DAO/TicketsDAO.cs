using SerraLinhasAereas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SerraLinhasAereas.Infra.Data.DAO
{
    public class TicketsDAO
    {
        private const string _connectionString = @"Data Source=.\SQLEXPRESS;initial catalog=SERRALINHASAEREASDB;uid=sa;pwd=tunico;";

        internal void RegisterTicket(Tickets newTicket)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"INSERT INTO TICKETS (DEPART_FROM, FLYING_TO, PRICE, DEPART_FROM_DATE, FLYING_TO_DATE) VALUES (@DEPART_FROM, @FLYING_TO, @PRICE, @DEPART_FROM_DATE, @FLYING_TO_DATE);";
                    ConvertObjectToSql(newTicket, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }

        internal List<Tickets> GetTicketByDate(int year, int month, int day)
        {
            var ticketList = new List<Tickets>();
            var wantedData = new DateTime(year, month, day);
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT ID, DEPART_FROM, FLYING_TO, PRICE, DEPART_FROM_DATE, FLYING_TO_DATE FROM TICKETS WHERE CONVERT(DATE, DEPART_FROM_DATE) = @DEPART_FROM_DATE";
                    DoCommand.CommandText = sql;
                    DoCommand.Parameters.AddWithValue("@DEPART_FROM_DATE", wantedData.ToShortDateString());
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Tickets wantedTicket = ConvertSqlToObjetc(reader);
                        ticketList.Add(wantedTicket);
                    }
                }
            }
            return ticketList;
        }

        internal List<Tickets> GetTicketByFlyingTo(string flyingTo)
        {
            var ticketList = new List<Tickets>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT ID, DEPART_FROM, FLYING_TO, PRICE, DEPART_FROM_DATE, FLYING_TO_DATE FROM TICKETS WHERE FLYING_TO = @FLYING_TO";
                    DoCommand.CommandText = sql;
                    DoCommand.Parameters.AddWithValue("@FLYING_TO", flyingTo);
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Tickets wantedTicket = ConvertSqlToObjetc(reader);
                        ticketList.Add(wantedTicket);
                    }
                }
            }
            return ticketList;
        }

        internal Tickets GetTicketById(int id)
        {
            var wantedTicket = new Tickets();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, DEPART_FROM, FLYING_TO, PRICE, DEPART_FROM_DATE, FLYING_TO_DATE FROM TICKETS WHERE ID = @ID";
                    comando.CommandText = sql;
                    comando.Parameters.AddWithValue("@ID", id);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        wantedTicket = ConvertSqlToObjetc(reader);
                    };
                    return wantedTicket;
                }
            }
        }

        internal void UpdateTicket(Tickets editedTicket)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"UPDATE TICKETS SET        
                                        DEPART_FROM = @DEPART_FROM,
                                        FLYING_TO = @FLYING_TO,
                                        PRICE = @PRICE,
                                        DEPART_FROM_DATE = @DEPART_FROM_DATE,
                                        FLYING_TO_DATE = @FLYING_TO_DATE
                                 WHERE ID = @ID;";
                    DoCommand.Parameters.AddWithValue("@ID", editedTicket.Id);
                    ConvertObjectToSql(editedTicket, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }

        internal List<Tickets> GetTicketByDepartFrom(string departFrom)
        {
            var ticketList = new List<Tickets>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT ID, DEPART_FROM, FLYING_TO, PRICE, DEPART_FROM_DATE, FLYING_TO_DATE FROM TICKETS WHERE DEPART_FROM = @DEPART_FROM";
                    DoCommand.CommandText = sql;
                    DoCommand.Parameters.AddWithValue("@DEPART_FROM", departFrom);
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Tickets wantedTicket = ConvertSqlToObjetc(reader);
                        ticketList.Add(wantedTicket);
                    }
                }
            }
            return ticketList;
        }

        internal List<Tickets> GetAllTickets()
        {
            var ticketList = new List<Tickets>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT * FROM TICKETS";
                    DoCommand.CommandText = sql;
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Tickets wantedTicket = ConvertSqlToObjetc(reader);
                        ticketList.Add(wantedTicket);
                    }
                }
            }
            return ticketList;
        }

        private Tickets ConvertSqlToObjetc(SqlDataReader reader)
        {
            Tickets wantedTicket = new Tickets();
            wantedTicket.Id = Convert.ToInt32(reader["ID"].ToString());
            wantedTicket.DepartFrom = reader["DEPART_FROM"].ToString();
            wantedTicket.FlyingTo = reader["FLYING_TO"].ToString();
            wantedTicket.Price = Convert.ToDecimal(reader["PRICE"].ToString());
            wantedTicket.DepartFromDate = Convert.ToDateTime(reader["DEPART_FROM_DATE"].ToString());
            wantedTicket.FlyingToDate = Convert.ToDateTime(reader["FLYING_TO_DATE"].ToString());
            return wantedTicket;


        }

        private void ConvertObjectToSql(Tickets newTicket, SqlCommand doCommand)
        {
            doCommand.Parameters.AddWithValue("@DEPART_FROM", newTicket.DepartFrom);
            doCommand.Parameters.AddWithValue("@FLYING_TO", newTicket.FlyingTo);
            doCommand.Parameters.AddWithValue("@PRICE", newTicket.Price);
            doCommand.Parameters.AddWithValue("@DEPART_FROM_DATE", newTicket.DepartFromDate);
            doCommand.Parameters.AddWithValue("@FLYING_TO_DATE", newTicket.FlyingToDate);
        }
    }
}
