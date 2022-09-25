using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerraLinhasAereas.Infra.Data.Repository
{
    public class TravelsRepository : ITravelsRepository
    {
        private readonly TravelsDAO _travelsDAO;
        private readonly ClientDAO _clientsDAO;
        private readonly TicketsDAO _ticketsDAO;

        public TravelsRepository()
        {
            _travelsDAO = new TravelsDAO();
            _clientsDAO = new ClientDAO();
            _ticketsDAO = new TicketsDAO();
        }
        public List<Travels> GetAllTravels()
        {
            return _travelsDAO.GetAllTravels();
        }
        public List<Travels> GetAllTravelsFromExistentClient(string cpf)
        {
            var wantedClient = _clientsDAO.GetClientByCpf(cpf);
            if (wantedClient == null)
            {
                throw new Exception("Nenhum cliente encontrado!");
            }
            else
            {
                var listTravels = _travelsDAO.GetAllTravelsFromExistentClient(wantedClient);
                if (listTravels.Count == 0)
                {
                    throw new Exception($"Nenhuma viagem encontrada!");
                }
                else
                {
                    return listTravels;
                }
            }


        }
        public void RegisterTravelGoingAndReturn(Travels newTravel)
        {
            //criando uma lista de passagens
            var ticketList = _ticketsDAO.GetAllTickets();
            //encontrando passagem pelo id
            var goingTicket = ticketList.Find(x => x.Id == newTravel.GoingTicket.Id);
            var returnTicket = ticketList.Find(x => x.Id == newTravel.ReturnTicket.Id);
            //atribuindo passagens para a viagem
            newTravel.GoingTicket = goingTicket;
            newTravel.ReturnTicket = returnTicket;
            //criando uma lsita de viagens
            var travelList = _travelsDAO.GetAllTravels();
            //encontrando passagens dentro de viagens
            var goingTicketTravel = travelList.Find(x => x.Id == newTravel.GoingTicket.Id);
            var returnTicketTravel = travelList.Find(x => x.Id == newTravel.ReturnTicket.Id);

            //verificações
            if (goingTicket.DepartFromDate >= returnTicket.DepartFromDate)
            {
                throw new Exception("Data de ida deve ser menor que data da volta!!");
            }  
            if ((goingTicketTravel != null) || (returnTicketTravel != null))
            {
                throw new Exception("Passagem já está reservada!!!");
            }
            if(!newTravel.VerifyTicketCode(newTravel.TicketCode))
            {
                throw new Exception("Codigo da passagem invalido!!!");
            }
            else
            {

                _travelsDAO.RegisterTravelGoingAndReturn(newTravel);
            }
        }
        public void RegisterTravelJustGoing(Travels newTravel)
        {
            //criando uma lista de passagens
            var ticketList = _ticketsDAO.GetAllTickets();
            //encontrando passagem pelo id
            var goingTicket = ticketList.Find(x => x.Id == newTravel.GoingTicket.Id);
            //atribuindo passagens para a viagem
            newTravel.GoingTicket = goingTicket;
            //criando uma lsita de viagens
            var travelList = _travelsDAO.GetAllTravels();
            //encontrando passagens dentro de viagens
            List<Travels> ticketsInTravels = new List<Travels>();
            foreach (var item in travelList)
            {
                if (item.Id == goingTicket.Id)
                {
                    ticketsInTravels.Add(item);
                }

            }
            //verificações
            if ((ticketsInTravels.Count > 0))
            {
                throw new Exception("Passagem já está reservada!!!");
            }
            if(!newTravel.VerifyTicketCode(newTravel.TicketCode))
            {
                throw new Exception("Codigo da passagem invalido!!!");
            }
            else        
            {
                _travelsDAO.RegisterTravelJustGoing(newTravel);
            }
        }
        public void UpdateTravelGoingAndReturn(int travelId, DateTime departFromGoingDate, DateTime flyingToGoingDate, DateTime departFromReturnDate, DateTime flyingToReturnDate)
        {
            //buscando lista de viagens
            var travelList = _travelsDAO.GetAllTravels();
            //verificando viagem por cpf
            var editedTravel = new Travels();
            foreach (var item in travelList)
            {
                if (item.Id == travelId)
                    editedTravel = item;
            }
            //validando se viagem existe
            if (editedTravel == null)
            {
                throw new Exception("Viagem não encontrada!");
            }

            bool verifyGoingTicket = Tickets.VerifyDate(departFromGoingDate, flyingToGoingDate);
            bool verifyReturnTicket = Tickets.VerifyDate(departFromReturnDate, flyingToReturnDate);

            if (!verifyGoingTicket && !verifyReturnTicket)
            {
                throw new Exception("A data de ida deve ser menor que a data de volta!");
            }

            _travelsDAO.UpdateGoingTicket(travelId, departFromGoingDate, flyingToGoingDate);
            _travelsDAO.UpdateReturnTicket(travelId, departFromReturnDate, flyingToReturnDate);
        }
        public void UpdateTravelJustGoing(int travelId, DateTime departFromDate, DateTime flyingToDate)
        {
            //buscando lista de viagens
            var travelList = _travelsDAO.GetAllTravels();
            //verificando viagem por cpf
            var editedTravel = new Travels();
            foreach (var item in travelList)
            {
                if (item.Id == travelId)
                    editedTravel = item;
            }
            //validando se viagem existe
            if (editedTravel == null)
            {
                throw new Exception("Viagem não encontrada!");
            }

            bool verifytTiket = Tickets.VerifyDate(departFromDate, flyingToDate);

            if (!verifytTiket)
            {
                throw new Exception("A data de ida deve ser menor que a data de volta!");
            }

            _travelsDAO.UpdateGoingTicket(travelId, departFromDate, flyingToDate);
        }
    }
}
