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
    [Authorize(Roles = "Admin, Zaposleni")]
    [ApiController]
    [Route("api/sediste")]
    [Produces("application/json")]
    public class SedisteController : ControllerBase
    {
        private readonly ISedisteRepository sedisteRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public SedisteController(ISedisteRepository sedisteRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.sedisteRepository = sedisteRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Sediste>> GetSedisteList([FromQuery] PaginationFilter filter)
        {
            List<Sediste> sedistes = sedisteRepository.GetSedisteList();
            if (sedistes == null || sedistes.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = sedistes
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = sedistes.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Sediste>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [AllowAnonymous]
        [HttpGet("{sedisteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Sediste> GetSedisteById(Guid sedisteId)
        {
            Sediste sediste = sedisteRepository.GetSedisteById(sedisteId);
            if (sediste == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Sediste>(sediste));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Sediste> CreateSediste([FromBody] Sediste sediste)
        {
            try
            {

                var sedisteEntity = mapper.Map<Sediste>(sediste);
                var confirmation = sedisteRepository.CreateSediste(sedisteEntity);

                sedisteRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetSedisteList", "Sediste", new { sedisteId = confirmation.SedisteID });
                return Created(location, mapper.Map<Sediste>(confirmation));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }

        }

        [HttpDelete("{sedisteId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSediste(Guid sedisteId)
        {
            try
            {
                var sediste = sedisteRepository.GetSedisteById(sedisteId);

                sedisteRepository.DeleteSediste(sedisteId);
                sedisteRepository.SaveChanges();
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
        public ActionResult<Sediste> UpdateSediste(Sediste sediste)
        {
            try
            {
                var oldSediste = sedisteRepository.GetSedisteById(sediste.SedisteID);
                if (oldSediste == null)
                {
                    return NotFound();
                }

                Sediste sedisteEntity = mapper.Map<Sediste>(sediste);

                mapper.Map(sedisteEntity, oldSediste);

                sedisteRepository.SaveChanges();
                return Ok(mapper.Map<Sediste>(oldSediste));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }
        }



        [HttpOptions]
        public IActionResult GetSedisteOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}