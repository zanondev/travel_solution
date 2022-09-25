using SerraLinhasAereas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereas.Domain.Interfaces
{
    public interface ITicketsRepository
    {


        void RegisterTicket(Tickets newTicket);
        List<Tickets> GetAllTickets();
        List<Tickets> GetTicketByDate(int year, int month, int day);
        List<Tickets> GetTicketByDepartFrom(string departFrom);
        List<Tickets> GetTicketByFlyingTo(string flyingTo);
        void UpdateTicket(Tickets editedTicket);
        Tickets GetTicketById(int Id);


    }
}
