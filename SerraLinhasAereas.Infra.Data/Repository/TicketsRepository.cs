using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data.DAO;
using System;
using System.Collections.Generic;

namespace SerraLinhasAereas.Infra.Data.Repository
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly TicketsDAO _ticketsDAO;

        public TicketsRepository()
        {
            _ticketsDAO = new TicketsDAO();
        }

        public List<Tickets> GetAllTickets()
        {
            return _ticketsDAO.GetAllTickets();
        }

        public List<Tickets> GetTicketByDate(int year, int month, int day)
        {
            return _ticketsDAO.GetTicketByDate(year, month, day);
        }

        public List<Tickets> GetTicketByDepartFrom(string departFrom)
        {
            return _ticketsDAO.GetTicketByDepartFrom(departFrom);
        }

        public List<Tickets> GetTicketByFlyingTo(string flyingTo)
        {
            return _ticketsDAO.GetTicketByFlyingTo(flyingTo);
        }

        public Tickets GetTicketById(int Id)
        {
            return _ticketsDAO.GetTicketById(Id);
        }

        public void RegisterTicket(Tickets newTicket)
        {
            _ticketsDAO.RegisterTicket(newTicket);
        }

        public void UpdateTicket(Tickets editedTicket)
        {
            _ticketsDAO.UpdateTicket(editedTicket);
        }
    }
}
