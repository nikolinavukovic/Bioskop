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
    [Authorize(Roles = "Admin, Zaposleni")]
    [ApiController]
    [Route("api/zanr-filma")]
    [Produces("application/json")]
    public class ZanrFilmaController : ControllerBase
    {
        private readonly IZanrFilmaRepository zanrFilmaRepository;
        private readonly IZanrRepository zanrRepository;
        private readonly IFilmRepository filmRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public ZanrFilmaController(IZanrFilmaRepository zanrFilmaRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService, IZanrRepository zanrRepository, IFilmRepository filmRepository)
        {
            this.zanrFilmaRepository = zanrFilmaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;
            this.zanrRepository = zanrRepository;
            this.filmRepository = filmRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<ZanrFilma>> GetZanrFilmaList([FromQuery] PaginationFilter filter)
        {
            List<ZanrFilma> zanrFilmas = zanrFilmaRepository.GetZanrFilmaList();
            if (zanrFilmas == null || zanrFilmas.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = zanrFilmas
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = zanrFilmas.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<ZanrFilmaDto>(mapper.Map<List<ZanrFilmaDto>>(pagedData), validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [AllowAnonymous]
        [HttpGet("{zanrId}/{filmId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ZanrFilma> GetZanrFilmaById(Guid zanrId, Guid filmId)
        {
            ZanrFilma zanrFilma = zanrFilmaRepository.GetZanrFilmaById(zanrId, filmId);
            if (zanrFilma == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ZanrFilmaDto>(zanrFilma));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ZanrFilma> CreateZanrFilma([FromBody] ZanrFilma zanrFilma)
        {
            try
            {

                var zanrFilmaEntity = mapper.Map<ZanrFilma>(zanrFilma);
                var confirmation = zanrFilmaRepository.CreateZanrFilma(zanrFilmaEntity);

                confirmation.Film = zanrFilmaRepository.GetZanrFilmaById(confirmation.ZanrID, confirmation.FilmID).Film;
                confirmation.Zanr = zanrFilmaRepository.GetZanrFilmaById(confirmation.ZanrID, confirmation.FilmID).Zanr;

                zanrFilmaRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetZanrFilmaList", "ZanrFilma", new { zanrId = confirmation.ZanrID, filmId = confirmation.FilmID });
                return Created(location, mapper.Map<ZanrFilmaDto>(confirmation));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }

        }

        [HttpDelete("{zanrId}/{filmId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteZanrFilma(Guid zanrId, Guid filmId)
        {
            try
            {
                var zanrFilma = zanrFilmaRepository.GetZanrFilmaById(zanrId, filmId);

                zanrFilmaRepository.DeleteZanrFilma(zanrId, filmId);
                zanrFilmaRepository.SaveChanges();
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
        public ActionResult<ZanrFilma> UpdateZanrFilma(ZanrFilma zanrFilma)
        {
            try
            {
                zanrFilmaRepository.GetZanrFilmaById(zanrFilma.ZanrID, zanrFilma.FilmID).Film = filmRepository.GetFilmById(zanrFilma.FilmID);
                zanrFilmaRepository.GetZanrFilmaById(zanrFilma.ZanrID, zanrFilma.FilmID).Zanr = zanrRepository.GetZanrById(zanrFilma.ZanrID);
                var oldZanrFilma = zanrFilmaRepository.GetZanrFilmaById(zanrFilma.ZanrID, zanrFilma.FilmID);
                if (oldZanrFilma == null)
                {
                    return NotFound();
                }

                Film f = oldZanrFilma.Film;
                Zanr z = oldZanrFilma.Zanr;


                ZanrFilma zanrFilmaEntity = mapper.Map<ZanrFilma>(zanrFilma);

                mapper.Map(zanrFilmaEntity, oldZanrFilma);

                oldZanrFilma.Film = f;
                oldZanrFilma.Zanr = z;

                zanrFilmaRepository.SaveChanges();
                return Ok(mapper.Map<ZanrFilmaDto>(oldZanrFilma));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }
        }



        [HttpOptions]
        public IActionResult GetZanrFilmaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
