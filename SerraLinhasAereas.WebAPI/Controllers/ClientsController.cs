using Microsoft.AspNetCore.Mvc;
using SerraLinhasAereas.Domain;
using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data;
using SerraLinhasAereas.Infra.Data.Repository;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace SerraLinhasAereas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]

    public class ClientsController : Controller
    {


        private readonly IClientsRepository _clientsRepository;

        public ClientsController()
        {
            _clientsRepository = new ClientsRepository();
        }
        [HttpPost]
        public IActionResult PostClient(Clients newClient)
        {
            if (newClient == null)
                return NoContent();

            var clientList = _clientsRepository.GetAllClients();
            foreach (var client in clientList)
            {
                if (client.Cpf == newClient.Cpf)
                    return BadRequest("Cliente já cadastrado"); ;
            }

            _clientsRepository.RegisterClient(newClient);
            return Ok(newClient);
        }
        [HttpGet("{cpf}")]
        public IActionResult GetClientByCpf(string cpf)
        {
            var wantedClient = _clientsRepository.GetClientByCpf(cpf);
            if (wantedClient.Cpf == null)
                return BadRequest(new Response(400, "Nenhum cliente localizado!"));

            return Ok(wantedClient);
        }
        [HttpGet]
        public IActionResult GetAllClientes()
        {
            var wantedClients = _clientsRepository.GetAllClients();
            if (wantedClients.Count == 0)
                return BadRequest("Nenhum cliente localizado!");
            return Ok(wantedClients);
        }
        [HttpDelete("{cpf}")]
        public IActionResult DeleteClient(string cpf)
        {
            var wantedClient = _clientsRepository.GetClientByCpf(cpf);

            if (wantedClient.Cpf == null)
                return BadRequest(new Response(400, "Nenhum cliente localizado!"));


            _clientsRepository.DeleteClient(cpf);

            return Ok("Cliente deletado com sucesso!");
        }
        [HttpPut]
        public IActionResult PutClient(Clients editedClient)
        {
            var wantedClient = _clientsRepository.GetClientById(editedClient.Id);
            if (wantedClient.Id != editedClient.Id)
                return BadRequest(new Response(400, "Nenhum cliente localizado!"));

            wantedClient.Cpf = editedClient.Cpf;
            wantedClient.Name = editedClient.Name;
            wantedClient.Nickname = editedClient.Nickname;
            wantedClient.Address.Cep = editedClient.Address.Cep;
            wantedClient.Address.Street = editedClient.Address.Street;
            wantedClient.Address.District = editedClient.Address.District;
            wantedClient.Address.Complement = editedClient.Address.Complement;

            _clientsRepository.UpdateClient(wantedClient);

            return Ok(_clientsRepository.GetAllClients());
        }


    }
}
