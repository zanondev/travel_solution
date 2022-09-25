using Microsoft.AspNetCore.Mvc;
using SerraLinhasAereas.Domain.Entities;
using SerraLinhasAereas.Domain.Interfaces;
using SerraLinhasAereas.Infra.Data.Repository;
using System;
using System.Collections.Generic;

namespace SerraLinhasAereas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/viagens")]
    public class TravelsController : Controller
    {
        private readonly ITravelsRepository _travelsRepository;
        public TravelsController()
        {
            _travelsRepository = new TravelsRepository();
        }

        [HttpGet("{cpf}")]
        public IActionResult GetTravelByClientCpf(string cpf)
        {

            var wantedTravels = _travelsRepository.GetAllTravelsFromExistentClient(cpf);
            if (wantedTravels == null)
                return BadRequest(new Response(400, "Nenhum cliente localizado!"));

            return Ok(wantedTravels);
        }

        [HttpPost("ida_e_volta")]
        public IActionResult PostTravelGoingAndReturn([FromBody] Travels newTravel)
        {
            //IMPORTANTE!
            //PARA CADASTRAR VIAGEM BASTA INFORMAR O TICKETCODE (CODIGO), ID DO CLIENTE, E ID DAS PASSAGENS NO JSON DO POST
            _travelsRepository.RegisterTravelGoingAndReturn(newTravel);
            try
            {

                return Ok("Viagem registrada com sucesso!");
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response(500, e.Message));
            }
        }
        [HttpPost("so_ida")]
        public IActionResult PostTravelJustGoing([FromBody] Travels newTravel)
        {
            //IMPORTANTE!
            //PARA CADASTRAR VIAGEM BASTA INFORMAR O TICKETCODE (CODIGO), ID DO CLIENTE, E ID DAS PASSAGENS NO JSON DO POST
            _travelsRepository.RegisterTravelJustGoing(newTravel);
            try
            {

                return Ok("Viagem registrada com sucesso!");
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response(500, e.Message));
            }
        }
        [HttpPatch("so_ida")]
        public IActionResult PatchJustGoing([FromQuery] int travelId, [FromQuery] DateTime departFromDate, DateTime flyingToDate)
        {
            try
            {
                _travelsRepository.UpdateTravelJustGoing(travelId, departFromDate, flyingToDate);
                return Ok($"Viagem remarcada com sucesso! Seu novo cronograma é: Saída: {departFromDate} Chegada: {flyingToDate}");
            }
            catch (Exception e)
            {
                return BadRequest("Falha ao remarcar passagem.");
            }
        }
        [HttpPatch("ida_e_volta")]
        public IActionResult PatchGoingAndReturn([FromQuery] int travelId, [FromQuery] DateTime departFromGoingDate, DateTime flyingToGoingDate, DateTime departFromReturnDate, DateTime flyingToReturnDate)
        {
            try
            {
                _travelsRepository.UpdateTravelGoingAndReturn(travelId, departFromGoingDate, flyingToGoingDate, departFromReturnDate, flyingToReturnDate);
                return Ok($"Viagem remarcada com sucesso! Seu novo cronograma é: Origem => Saída: {departFromGoingDate} Chegada: {flyingToGoingDate} Retorno => Saída: {departFromReturnDate} Chegada: {flyingToReturnDate}");
            }
            catch (Exception e)
            {
                return BadRequest("Falha ao remarcar passagem.");
            }
        }
    }
}
