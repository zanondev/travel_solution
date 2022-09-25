using SerraLinhasAereas.Domain.Entities;
using System.Collections.Generic;

namespace SerraLinhasAereas.Domain.Interfaces
{
    public interface IClientsRepository
    {
        void RegisterClient(Clients newClient);
        Clients GetClientByCpf(string cpf);
        List<Clients> GetAllClients();
        public void UpdateClient(Clients editedClient);
        void DeleteClient(string cpf);
        bool VerifyIfClientAlreadyExist(string cpf);
        Clients GetClientById(int Id);




    }
}
