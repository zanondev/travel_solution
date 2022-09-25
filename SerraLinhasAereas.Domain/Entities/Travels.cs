using System;

namespace SerraLinhasAereas.Domain.Entities
{
    public class Travels
    {
        public int Id { get; set; }
        public string TicketCode { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalPrice => CalculateTotalPrice();
        public int ClientId { get; set; }
        public Clients Client { get; set; }
        public Tickets GoingTicket { get; set; }
#nullable enable
        public Tickets? ReturnTicket { get; set; }
        public bool GoingAndReturn { get; set; } //Só ida: false; Ida e Volta: true
        public string TravelBrief { get; set; }

        public Travels()
        {

        }
 
        private string DoTravelBrief()
        {
            if (GoingAndReturn == true)
            {
                return $"Código da passagem: {TicketCode} | Cliente: {Client.CompleteName} | Cpf: {Client.Cpf} | " +
                    $"Seu voo de {GoingTicket.DepartFrom} a {GoingTicket.FlyingTo} será dia {GoingTicket.DepartFromDate.ToShortDateString()}" +
                       $" as {GoingTicket.DepartFromDate.ToShortTimeString()}h";
            }
            return $"Código da passagem: {TicketCode} | Cliente: {Client.CompleteName} | Cpf: {Client.Cpf} | " +
                $"Seu voo de {GoingTicket.DepartFrom} a {GoingTicket.FlyingTo} será dia {GoingTicket.DepartFromDate.ToShortDateString()}" +
                       $" as {GoingTicket.FlyingToDate.ToShortTimeString()}h e seu voo de volta de {ReturnTicket.DepartFrom}" +
                       $" a {ReturnTicket.FlyingTo} será dia {ReturnTicket.DepartFromDate.ToShortDateString()}" +
                       $" as {ReturnTicket.DepartFromDate.ToShortTimeString()}h";
        }

        private decimal CalculateTotalPrice()
        {
            if (GoingAndReturn == true)
                return GoingTicket.Price + ReturnTicket.Price;

            return GoingTicket.Price;
        }

        public bool ValidateGoingAndReturnTicketDate(Tickets goingTicket, Tickets returnTicket)
        {
            if (goingTicket.FlyingToDate < returnTicket.FlyingToDate)
            {
                return true;
            }
            return false;
        }

        public bool VerifyTicketCode(string ticketCode)  
        {
            return ticketCode.Length == 6 ? true : false;
        }
    }
}
