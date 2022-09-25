using SerraLinhasAereas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereas.Domain.Interfaces
{
    public interface ITravelsRepository
    {
        void RegisterTravelJustGoing(Travels newTravel);
        void RegisterTravelGoingAndReturn(Travels newTravel);
        List<Travels> GetAllTravelsFromExistentClient(string cpf);
        List<Travels> GetAllTravels();
        void UpdateTravelGoingAndReturn(int travelId, DateTime departFromGoingDate, DateTime flyingToGoingDate, DateTime departFromReturnDate, DateTime flyingToReturnDate);
        void UpdateTravelJustGoing(int travelId, DateTime departFromDate, DateTime flyingToDate);
    }
}
