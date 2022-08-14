using AutoMapper;
using Bioskop.Data;
using Bioskop.Filter;
using Bioskop.Helpers;
using Bioskop.Models;
using Bioskop.Models.Dtos;
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
    
    [Authorize(Roles = "Admin, Zaposleni, Registrovani korisnik")]
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

        [AllowAnonymous] //obrisati kasnije, dodato radi lakseg testiranja
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Korisnik>> GetKorisnikList([FromQuery] PaginationFilter filter, string korisnickoIme)
        {
            List<Korisnik> korisniks = korisnikRepository.GetKorisnikList(korisnickoIme);
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
            var pagedReponse = PaginationHelper.CreatePagedReponse<KorisnikDto>(mapper.Map<List<KorisnikDto>>(pagedData), validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [AllowAnonymous] //obrisati kasnije, dodato radi lakseg testiranja
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

            return Ok(mapper.Map<KorisnikDto>(korisnik));
        }

        [AllowAnonymous]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Korisnik> CreateKorisnik([FromBody] Korisnik korisnik)
        {
            try
            {

                var korisnikEntity = mapper.Map<Korisnik>(korisnik);


                bool validMail = EmailHelper.IsValidEmail(korisnik.Email);

                if (!validMail || korisnikRepository.UserWithEmailExists(korisnik.Email))
                {
                    throw new ArgumentException("Email nije validan ili se već koristi.");
                }

                if (korisnikRepository.UserWithUsernameExistst(korisnik.KorisnickoIme))
                {
                    throw new ArgumentException("Korisničko ime nije validno ili se već koristi.");
                }


                var confirmation = korisnikRepository.CreateKorisnik(korisnikEntity);


                korisnikRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetKorisnikList", "Korisnik", new { korisnikId = confirmation.KorisnikID });
                return Created(location, mapper.Map<KorisnikDto>(confirmation));

            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
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

                TipKorisnika tk = oldKorisnik.TipKorisnika;

                Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);
                mapper.Map(korisnikEntity, oldKorisnik);

                oldKorisnik.TipKorisnika = tk;

                korisnikRepository.SaveChanges();
                return Ok(mapper.Map<KorisnikDto>(oldKorisnik));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
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
