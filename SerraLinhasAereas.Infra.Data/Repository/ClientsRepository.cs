using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data.DAO;
using System.Collections.Generic;

namespace SerraLinhasAereas.Infra.Data.Repository
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly ClientDAO _clientDAO;

        public ClientsRepository()
        {
            _clientDAO = new ClientDAO();
        }

        public void DeleteClient(string cpf)
        {
            var wantedClient = _clientDAO.GetClientByCpf(cpf);

            _clientDAO.DeleteCliente(wantedClient.Cpf);
        }

        public List<Clients> GetAllClients()
        {
            return _clientDAO.GetAllClients();
        }

        public void RegisterClient(Clients newClient)
        {
            _clientDAO.RegisterClient(newClient);
        }

        public Clients GetClientByCpf(string cpf)
        {
            return _clientDAO.GetClientByCpf(cpf);
        }

        public void UpdateClient(Clients editedClient)
        {
            _clientDAO.UpdateClient(editedClient);
        }

        public bool VerifyIfClientAlreadyExist(string cpf)
        {
            var verifyClient = _clientDAO.GetClientByCpf(cpf);

            if (verifyClient == null)
                return false;

            return true;
        }

        public Clients GetClientById(int id)
        {
            return _clientDAO.GetClientById(id);
        }
    }
}
