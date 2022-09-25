using Microsoft.AspNetCore.Mvc;
using SerraLinhasAereas.Domain;
using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data;
using SerraLinhasAereas.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace SerraLinhasAereas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/passagens")]
    public class TicketsController : Controller
    {
        private readonly ITicketsRepository _ticketsRepository;

        public TicketsController()
        {
            _ticketsRepository = new TicketsRepository();
        }

        [HttpPost]
        public IActionResult PostTicket(Tickets newTicket)
        {
            if (newTicket == null)
                return NoContent();

            var ticketList = _ticketsRepository.GetAllTickets();
            foreach (var ticket in ticketList)
            {
                if (ticket.Id == newTicket.Id)
                    return BadRequest("Ticket já cadastrado"); ;
            }

            _ticketsRepository.RegisterTicket(newTicket);
            return Ok(newTicket);
        }
        [HttpGet]
        public IActionResult GetAllTickets()
        {
            var wantedTickets = _ticketsRepository.GetAllTickets();
            if (wantedTickets.Count == 0)
                return BadRequest("Nenhum cliente localizado!");
            return Ok(wantedTickets);
        }
        [HttpGet("depart_from/{departFrom}")]
        public IActionResult GetTicketByDepartFrom(string departFrom)
        {
            var wantedTicket = _ticketsRepository.GetTicketByDepartFrom(departFrom);
            if (wantedTicket.Count == 0)
                return BadRequest("Nenhum ticket localizado!");

            return Ok(wantedTicket);
        }
        [HttpGet("flying_to/{flyingTo}")]
        public IActionResult GetTicketByFlyingTo(string flyingTo)
        {
            var wantedTicket = _ticketsRepository.GetTicketByFlyingTo(flyingTo);
            if (wantedTicket.Count == 0)
                return BadRequest("Nenhum ticket localizado!");

            return Ok(wantedTicket);
        }
        [HttpPut]
        public IActionResult PutTicket(Tickets editedTicket)
        {
            var wantedTicket = _ticketsRepository.GetTicketById(editedTicket.Id);
            if (wantedTicket.Id != editedTicket.Id)
                return BadRequest("Nenhum cliente localizado!");

            wantedTicket.DepartFrom = editedTicket.DepartFrom;
            wantedTicket.FlyingTo = editedTicket.FlyingTo;
            wantedTicket.Price = editedTicket.Price;
            wantedTicket.DepartFromDate = editedTicket.DepartFromDate;
            wantedTicket.FlyingToDate = editedTicket.FlyingToDate;

            _ticketsRepository.UpdateTicket(wantedTicket);

            return Ok(_ticketsRepository.GetAllTickets());
        }
        [HttpGet("ticket_date/{year}/{month}/{day}")]
        public IActionResult GetTicketByDate(int year, int month, int day)
        {
            var wantedData = new DateTime(year, month, day);
            var wantedTicket = _ticketsRepository.GetTicketByDate(year, month, day);
            if (wantedTicket.Count == 0)
                return BadRequest("Nenhum ticket localizado!");

            return Ok(wantedTicket);
        }

    }
}
