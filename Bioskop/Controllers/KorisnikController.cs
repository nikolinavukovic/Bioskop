using AutoMapper;
using Bioskop.Data;
using Bioskop.Filter;
using Bioskop.Helpers;
using Bioskop.Models;
using Bioskop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Controllers
{
    [ApiController]
    [Route("api/korisnik")]
    [Produces("application/json")]
    public class KorisnikController : ControllerBase
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public KorisnikController(IKorisnikRepository korisnikRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.korisnikRepository = korisnikRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Korisnik>> GetKorisnikList([FromQuery] PaginationFilter filter)
        {
            List<Korisnik> korisniks = korisnikRepository.GetKorisnikList();
            if (korisniks == null || korisniks.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = korisniks
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = korisniks.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Korisnik>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{korisnikId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Korisnik> GetKorisnikById(Guid korisnikId)
        {
            Korisnik korisnik = korisnikRepository.GetKorisnikById(korisnikId);
            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Korisnik>(korisnik));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Korisnik> CreateKorisnik([FromBody] Korisnik korisnik)
        {
            try
            {

                var korisnikEntity = mapper.Map<Korisnik>(korisnik);
                var confirmation = korisnikRepository.CreateKorisnik(korisnikEntity);

                korisnikRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetKorisnikList", "Korisnik", new { korisnikId = confirmation.KorisnikID });
                return Created(location, mapper.Map<Korisnik>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
            }

        }

        [HttpDelete("{korisnikId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteKorisnik(Guid korisnikId)
        {
            try
            {
                var korisnik = korisnikRepository.GetKorisnikById(korisnikId);

                korisnikRepository.DeleteKorisnik(korisnikId);
                korisnikRepository.SaveChanges();
                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when deleting an object");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public ActionResult<Korisnik> UpdateKorisnik(Korisnik korisnik)
        {
            try
            {
                var oldKorisnik = korisnikRepository.GetKorisnikById(korisnik.KorisnikID);
                if (oldKorisnik == null)
                {
                    return NotFound();
                }

                Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);

                mapper.Map(korisnikEntity, oldKorisnik);

                korisnikRepository.SaveChanges();
                return Ok(mapper.Map<Korisnik>(oldKorisnik));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
            }
        }



        [HttpOptions]
        public IActionResult GetKorisnikOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
