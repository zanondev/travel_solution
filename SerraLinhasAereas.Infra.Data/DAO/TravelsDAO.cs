using SerraLinhasAereas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SerraLinhasAereas.Infra.Data.DAO
{
    public class TravelsDAO
    {
        private ClientDAO _clientDAO = new ClientDAO();
        private TicketsDAO _ticketsDAO = new TicketsDAO();

        private const string _connectionString = @"Data Source=.\SQLEXPRESS;initial catalog=SERRALINHASAEREASDB;uid=sa;pwd=tunico;";
        internal List<Travels> GetAllTravelsFromExistentClient(Clients wantedClient)
        {
            var travelsList = new List<Travels>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT TR.TICKET_CODE, TR.GOING_AND_RETURN, TR.TICKET_GOING, TR.TICKET_RETURN, TI_G.DEPART_FROM AS G_DEPARTFROM, TI_G.DEPART_FROM_DATE AS G_DEPARTFROM_DATE, TI_G.FLYING_TO AS G_FLYINGTO, TI_G.FLYING_TO_DATE AS G_FLYINGTO_DATE, TI_R.DEPART_FROM AS R_DEPARTFROM, TI_R.DEPART_FROM_DATE AS R_DEPARTFROM_DATE, TI_R.FLYING_TO R_FLYINGTO, TI_R.FLYING_TO_DATE AS R_FLYINGTO_DATE, C.CLIENT_NAME, C.CPF 
                    FROM TRAVELS TR 
                    JOIN CLIENTS C ON C.ID = TR.CLIENT_ID 
                    JOIN TICKETS TI_G ON TI_G.ID = TR.TICKET_GOING 
                    LEFT JOIN TICKETS TI_R ON TI_R.ID = TR.TICKET_RETURN
                    WHERE C.CPF = @CPF";
                    DoCommand.CommandText = sql;
                    DoCommand.Parameters.AddWithValue("@CPF", wantedClient.Cpf);
                    var reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Travels wantedTravel = ConvertSqlToObjetcByClient(reader);
                        travelsList.Add(wantedTravel);
                    }
                }
            }
            return travelsList;
        }
        private Travels ConvertSqlToObjetcByClient(SqlDataReader reader)
        {
            Travels wantedTravel = new Travels();
            wantedTravel.TicketCode = reader["TICKET_CODE"].ToString();
            var goingAndReturn = Convert.ToBoolean(reader["GOING_AND_RETURN"].ToString());
            var returnTicketEmpty = new Tickets();

            wantedTravel.GoingTicket = new Tickets()
            {
                Id = Convert.ToInt32(reader["TICKET_GOING"].ToString())
            };

            if (goingAndReturn == true)
            {
                wantedTravel.ReturnTicket = new Tickets()
                {
                    Id = Convert.ToInt32(reader["TICKET_RETURN"].ToString())
                };
            }

            var goingTicket = _ticketsDAO.GetTicketById(wantedTravel.GoingTicket.Id);
            goingTicket.DepartFrom = reader["G_DEPARTFROM"].ToString();
            goingTicket.DepartFromDate = Convert.ToDateTime(reader["G_DEPARTFROM_DATE"].ToString());
            goingTicket.FlyingTo = reader["G_FLYINGTO"].ToString();
            goingTicket.FlyingToDate = Convert.ToDateTime(reader["G_FLYINGTO_DATE"].ToString());

            if (goingAndReturn == true)
            {
                var returnTicket = _ticketsDAO.GetTicketById(wantedTravel.ReturnTicket.Id);
                returnTicket.DepartFrom = reader["R_DEPARTFROM"].ToString();
                returnTicket.DepartFromDate = Convert.ToDateTime(reader["R_DEPARTFROM_DATE"].ToString());
                returnTicket.FlyingTo = reader["R_FLYINGTO"].ToString();
                returnTicket.FlyingToDate = Convert.ToDateTime(reader["R_FLYINGTO_DATE"].ToString());
                wantedTravel.ReturnTicket = returnTicket;
            }

            var client = _clientDAO.GetClientById(wantedTravel.ClientId);
            client.Name = reader["CLIENT_NAME"].ToString();
            client.Cpf = reader["CPF"].ToString();

            wantedTravel.GoingTicket = goingTicket;
            wantedTravel.Client = client;

            return wantedTravel;
        }
        internal void RegisterTravelGoingAndReturn(Travels newTravel)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"INSERT TRAVELS VALUES 
                    (@TICKET_CODE, @ORDERTIME, @TOTALPRICE, @CLIENT_ID, @GOING_AND_RETURN, @TICKET_GOING, @TICKET_RETURN);";
                    ConvertObjectToSqlGoingAndReturn(newTravel, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
        private void ConvertObjectToSqlGoingAndReturn(Travels newTravel, SqlCommand doCommand)
        {
            doCommand.Parameters.AddWithValue("@TICKET_CODE", newTravel.TicketCode);
            doCommand.Parameters.AddWithValue("@ORDERTIME", newTravel.OrderTime = DateTime.Now);
            doCommand.Parameters.AddWithValue("@TOTALPRICE", newTravel.TotalPrice);
            doCommand.Parameters.AddWithValue("@CLIENT_ID", newTravel.ClientId);
            doCommand.Parameters.AddWithValue("@GOING_AND_RETURN", newTravel.GoingAndReturn = true);
            doCommand.Parameters.AddWithValue("@TICKET_GOING", newTravel.GoingTicket.Id);
            doCommand.Parameters.AddWithValue("@TICKET_RETURN", newTravel.ReturnTicket.Id);
        }
        internal void RegisterTravelJustGoing(Travels newTravel)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"INSERT TRAVELS VALUES 
                    (@TICKET_CODE, @ORDERTIME, @TOTALPRICE, @CLIENT_ID, @GOING_AND_RETURN, @TICKET_GOING, @TICKET_RETURN);";
                    ConvertObjectToSqlJustGoing(newTravel, DoCommand);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
        private void ConvertObjectToSqlJustGoing(Travels newTravel, SqlCommand doCommand)
        {
            doCommand.Parameters.AddWithValue("@TICKET_CODE", newTravel.TicketCode);
            doCommand.Parameters.AddWithValue("@ORDERTIME", newTravel.OrderTime = DateTime.Now);
            doCommand.Parameters.AddWithValue("@TOTALPRICE", newTravel.TotalPrice);
            doCommand.Parameters.AddWithValue("@CLIENT_ID", newTravel.ClientId);
            doCommand.Parameters.AddWithValue("@GOING_AND_RETURN", newTravel.GoingAndReturn = false);
            doCommand.Parameters.AddWithValue("@TICKET_GOING", newTravel.GoingTicket.Id);
            doCommand.Parameters.AddWithValue("@TICKET_RETURN", newTravel.ReturnTicket.Id == 0 ? DBNull.Value : DBNull.Value);
        }
        internal List<Travels> GetAllTravels()
        {
            var travelList = new List<Travels>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"SELECT ID, TICKET_CODE, ORDERTIME, CLIENT_ID, GOING_AND_RETURN, TICKET_GOING, TICKET_RETURN FROM TRAVELS";
                    DoCommand.CommandText = sql;
                    SqlDataReader reader = DoCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Travels wantedTravel = ConvertSqlToObjetcAll(reader);
                        travelList.Add(wantedTravel);
                    }
                }
            }
            return travelList;
        }
        private Travels ConvertSqlToObjetcAll(SqlDataReader reader)
        {
            Travels wantedTravel = new Travels();
            wantedTravel.Id = Convert.ToInt32(reader["ID"].ToString());
            wantedTravel.TicketCode = reader["TICKET_CODE"].ToString();
            wantedTravel.OrderTime = Convert.ToDateTime(reader["ORDERTIME"].ToString());
            wantedTravel.ClientId = Convert.ToInt32(reader["CLIENT_ID"].ToString());
            wantedTravel.GoingAndReturn = Convert.ToBoolean(reader["GOING_AND_RETURN"].ToString());
            var returnTicketEmpty = new Tickets();

            wantedTravel.GoingTicket = new Tickets()
            {
                Id = Convert.ToInt32(reader["TICKET_GOING"].ToString())
            };

            if (wantedTravel.GoingAndReturn == true)
            {
                wantedTravel.ReturnTicket = new Tickets()
                {
                    Id = Convert.ToInt32(reader["TICKET_RETURN"].ToString())
                };
            }
            //else
            //{
            //    returnTicketEmpty.Id = Convert.ToInt32(reader["TICKET_RETURN"].ToString());
            //}


            return wantedTravel;
        }
        private void ConvertObjectToSqlUpdate(Travels editedTravel, SqlCommand doCommand)
        {
            doCommand.Parameters.AddWithValue("@ORDERTIME", editedTravel.OrderTime = DateTime.Now);
            doCommand.Parameters.AddWithValue("@TICKET_GOING", editedTravel.GoingTicket.Id);
            doCommand.Parameters.AddWithValue("@TICKET_RETURN", editedTravel.ReturnTicket.Id);
            doCommand.Parameters.AddWithValue("@TOTALPRICE", editedTravel.TotalPrice);
        }
        internal void UpdateGoingTicket(int travelId, DateTime departFromGoingDate, DateTime flyingToGoingDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"UPDATE TICKETS SET        
                                        DEPART_FROM_DATE = @DEPART_FROM_DATE,
                                        FLYING_TO_DATE = @FLYING_TO_DATE
                                        FROM TICKETS TI 
                                        INNER JOIN TRAVELS TR
                                        ON TI.ID = TR.TICKET_GOING
                                        WHERE TR.ID = @TRAVEL_ID;";
                    DoCommand.Parameters.AddWithValue("@DEPART_FROM_DATE", departFromGoingDate);
                    DoCommand.Parameters.AddWithValue("@FLYING_TO_DATE", flyingToGoingDate);
                    DoCommand.Parameters.AddWithValue("@TRAVEL_ID", travelId);
                    DoCommand.CommandText = sql;
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
        internal void UpdateReturnTicket(int travelId, DateTime departFromReturnDate, DateTime flyingToReturnDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var DoCommand = new SqlCommand())
                {
                    DoCommand.Connection = connection;
                    string sql = @"UPDATE TICKETS SET        
                                        DEPART_FROM_DATE = @DEPART_FROM_DATE,
                                        FLYING_TO_DATE = @FLYING_TO_DATE
                                        FROM TICKETS TI 
                                        INNER JOIN TRAVELS TR
                                        ON TI.ID = TR.TICKET_GOING
                                        WHERE TR.ID = @TRAVEL_ID;";
                    DoCommand.Parameters.AddWithValue("@DEPART_FROM_DATE", departFromReturnDate);
                    DoCommand.Parameters.AddWithValue("@FLYING_TO_DATE", flyingToReturnDate);
                    DoCommand.Parameters.AddWithValue("@TRAVEL_ID", travelId);
                    DoCommand.CommandText = sql;    
                    DoCommand.ExecuteNonQuery();
                }
            }
        }
    }
}

