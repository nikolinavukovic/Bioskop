using AutoMapper;
using Bioskop.Data;
using Bioskop.Filter;
using Bioskop.Helpers;
using Bioskop.Models;
using Bioskop.Services;
using Bioskop.Wrappers;
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
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public ZanrFilmaController(IZanrFilmaRepository zanrFilmaRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.zanrFilmaRepository = zanrFilmaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;
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
            var pagedReponse = PaginationHelper.CreatePagedReponse<ZanrFilma>(pagedData, validFilter, totalRecords, uriService, route);
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

            return Ok(mapper.Map<ZanrFilma>(zanrFilma));
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

                zanrFilmaRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetZanrFilmaList", "ZanrFilma", new { zanrId = confirmation.ZanrID, filmId = confirmation.FilmID} ) ;
                return Created(location, mapper.Map<ZanrFilma>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
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
        public ActionResult<ZanrFilma> UpdateZanrFilma(ZanrFilma zanrFilma)
        {
            try
            {
                var oldZanrFilma = zanrFilmaRepository.GetZanrFilmaById(zanrFilma.ZanrID, zanrFilma.FilmID);
                if (oldZanrFilma == null)
                {
                    return NotFound();
                }

                ZanrFilma zanrFilmaEntity = mapper.Map<ZanrFilma>(zanrFilma);

                mapper.Map(zanrFilmaEntity, oldZanrFilma);

                zanrFilmaRepository.SaveChanges();
                return Ok(mapper.Map<ZanrFilma>(oldZanrFilma));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
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
