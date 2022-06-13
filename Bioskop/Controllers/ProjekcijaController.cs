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
    //[Authorize(Roles = "Admin, Zaposleni")]
    [ApiController]
    [Route("api/projekcija")]
    [Produces("application/json")]
    public class ProjekcijaController : ControllerBase
    {
        private readonly IProjekcijaRepository projekcijaRepository;
        private readonly IFilmRepository filmRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public ProjekcijaController(IProjekcijaRepository projekcijaRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService, IFilmRepository filmRepository)
        {
            this.projekcijaRepository = projekcijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;
            this.filmRepository = filmRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Projekcija>> GetProjekcijaList([FromQuery] PaginationFilter filter)
        {
            List<Projekcija> projekcijas = projekcijaRepository.GetProjekcijaList();
            if (projekcijas == null || projekcijas.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = projekcijas
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = projekcijas.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<ProjekcijaDto>(mapper.Map<List<ProjekcijaDto>>(pagedData), validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [AllowAnonymous]
        [HttpGet("{projekcijaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Projekcija> GetProjekcijaById(Guid projekcijaId)
        {
            Projekcija projekcija = projekcijaRepository.GetProjekcijaById(projekcijaId);
            if (projekcija == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ProjekcijaDto>(projekcija));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Projekcija> CreateProjekcija([FromBody] Projekcija projekcija)
        {
            try
            {

                var projekcijaEntity = mapper.Map<Projekcija>(projekcija);
                var confirmation = projekcijaRepository.CreateProjekcija(projekcijaEntity);

                confirmation.Film = projekcijaRepository.GetProjekcijaById(confirmation.ProjekcijaID).Film;

                projekcijaRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetProjekcijaList", "Projekcija", new { projekcijaId = confirmation.ProjekcijaID });
                return Created(location, mapper.Map<ProjekcijaDto>(confirmation));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }

        }

        [HttpDelete("{projekcijaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProjekcija(Guid projekcijaId)
        {
            try
            {
                var projekcija = projekcijaRepository.GetProjekcijaById(projekcijaId);

                projekcijaRepository.DeleteProjekcija(projekcijaId);
                projekcijaRepository.SaveChanges();
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
        public ActionResult<Projekcija> UpdateProjekcija(Projekcija projekcija)
        {
            try
            {
                projekcijaRepository.GetProjekcijaById(projekcija.ProjekcijaID).Film = filmRepository.GetFilmById(projekcija.FilmID);
                var oldProjekcija = projekcijaRepository.GetProjekcijaById(projekcija.ProjekcijaID);
                if (oldProjekcija == null)
                {
                    return NotFound();
                }

                Film f = oldProjekcija.Film;

                Projekcija projekcijaEntity = mapper.Map<Projekcija>(projekcija);

                mapper.Map(projekcijaEntity, oldProjekcija);

                oldProjekcija.Film = f;

                projekcijaRepository.SaveChanges();
                return Ok(mapper.Map<ProjekcijaDto>(oldProjekcija));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }
        }



        [HttpOptions]
        public IActionResult GetProjekcijaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
