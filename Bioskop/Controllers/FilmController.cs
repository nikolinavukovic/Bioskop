using AutoMapper;
using Bioskop.Data;
using Bioskop.Filter;
using Bioskop.Helpers;
using Bioskop.Models;
using Bioskop.Services;
using Bioskop.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Controllers
{
    [ApiController]
    [Route("api/film")]
    [Produces("application/json")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository filmRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public FilmController(IFilmRepository filmRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.filmRepository = filmRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Film>> GetFilmList([FromQuery] PaginationFilter filter, int godina)
        {
            List<Film> films = filmRepository.GetFilmList(godina);

            if (films == null || films.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = films
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = films.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Film>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{filmId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Film> GetFilmById(Guid filmId)
        {
            Film film = filmRepository.GetFilmById(filmId);
            if (film == null)
            {
                return NotFound();
            }


            return Ok(mapper.Map<Film>(film));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Film> CreateFilm([FromBody] Film film)
        {
            try
            {

                var filmEntity = mapper.Map<Film>(film);
                var confirmation = filmRepository.CreateFilm(filmEntity);

                filmRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetFilmList", "Film", new { filmId = confirmation.FilmID });
                return Created(location, mapper.Map<Film>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
            }

        }

        [HttpDelete("{filmId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFilm(Guid filmId)
        {
            try
            {
                var film = filmRepository.GetFilmById(filmId);

                filmRepository.DeleteFilm(filmId);
                filmRepository.SaveChanges();
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
        public ActionResult<Film> UpdateFilm(Film film)
        {
            try
            {
                var oldFilm = filmRepository.GetFilmById(film.FilmID);
                if (oldFilm == null)
                {
                    return NotFound();
                }

                Film filmEntity = mapper.Map<Film>(film);

                mapper.Map(filmEntity, oldFilm);

                filmRepository.SaveChanges();
                return Ok(mapper.Map<Film>(oldFilm));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
            }
        }



        [HttpOptions]
        public IActionResult GetFilmOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
