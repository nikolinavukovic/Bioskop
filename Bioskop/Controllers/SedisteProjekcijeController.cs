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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Controllers
{
    [Authorize(Roles = "Admin, Zaposleni, Registrovani korisnik")]
    [ApiController]
    [Route("api/sediste-projekcije")]
    [Produces("application/json")]
    public class SedisteProjekcijeController : ControllerBase
    {
        private readonly ISedisteProjekcijeRepository sedisteProjekcijeRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public SedisteProjekcijeController(ISedisteProjekcijeRepository sedisteProjekcijeRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.sedisteProjekcijeRepository = sedisteProjekcijeRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<List<SedisteProjekcije>> GetSedisteProjekcijeList([FromQuery] PaginationFilter filter)
        {
            List<SedisteProjekcije> sedisteProjekcijes = sedisteProjekcijeRepository.GetSedisteProjekcijeList();
            if (sedisteProjekcijes == null || sedisteProjekcijes.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = sedisteProjekcijes
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = sedisteProjekcijes.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<SedisteProjekcije>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet("{sedisteId}/{projekcijaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SedisteProjekcije> GetSedisteProjekcijeById(Guid sedisteId, Guid projekcijaId)
        {
            SedisteProjekcije sedisteProjekcije = sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteId, projekcijaId);
            if (sedisteProjekcije == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<SedisteProjekcije>(sedisteProjekcije));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SedisteProjekcije> CreateSedisteProjekcije([FromBody] SedisteProjekcije sedisteProjekcije)
        {
            try
            {

                var sedisteProjekcijeEntity = mapper.Map<SedisteProjekcije>(sedisteProjekcije);
                var confirmation = sedisteProjekcijeRepository.CreateSedisteProjekcije(sedisteProjekcijeEntity);

                sedisteProjekcijeRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetSedisteProjekcijeList", "SedisteProjekcije", new { sedisteId = confirmation.SedisteID, projekcijaId = confirmation.ProjekcijaID });
                return Created(location, mapper.Map<SedisteProjekcije>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
            }

        }

        [HttpDelete("{sedisteId}/{projekcijaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSedisteProjekcije(Guid sedisteId, Guid projekcijaId)
        {
            try
            {
                var sedisteProjekcije = sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteId, projekcijaId);

                sedisteProjekcijeRepository.DeleteSedisteProjekcije(sedisteId, projekcijaId);
                sedisteProjekcijeRepository.SaveChanges();
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
        public ActionResult<SedisteProjekcije> UpdateSedisteProjekcije(SedisteProjekcije sedisteProjekcije)
        {
            try
            {
                var oldSedisteProjekcije = sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteProjekcije.SedisteID, sedisteProjekcije.ProjekcijaID);
                if (oldSedisteProjekcije == null)
                {
                    return NotFound();
                }

                SedisteProjekcije sedisteProjekcijeEntity = mapper.Map<SedisteProjekcije>(sedisteProjekcije);

                mapper.Map(sedisteProjekcijeEntity, oldSedisteProjekcije);

                sedisteProjekcijeRepository.SaveChanges();
                return Ok(mapper.Map<SedisteProjekcije>(oldSedisteProjekcije));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
            }
        }



        [HttpOptions]
        public IActionResult GetSedisteProjekcijeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
