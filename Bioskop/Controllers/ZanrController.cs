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
    [Route("api/zanr")]
    [Produces("application/json")]
    public class ZanrController : ControllerBase
    {
        private readonly IZanrRepository zanrRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public ZanrController(IZanrRepository zanrRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.zanrRepository = zanrRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Zanr>> GetZanrList([FromQuery] PaginationFilter filter, string naziv)
        {
            List<Zanr> zanrs = zanrRepository.GetZanrList(naziv);
            if (zanrs == null || zanrs.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = zanrs
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = zanrs.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Zanr>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [AllowAnonymous]
        [HttpGet("{zanrId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Zanr> GetZanrById(Guid zanrId)
        {
            Zanr zanr = zanrRepository.GetZanrById(zanrId);
            if (zanr == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Zanr>(zanr));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Zanr> CreateZanr([FromBody] Zanr zanr)
        {
            try
            {

                var zanrEntity = mapper.Map<Zanr>(zanr);
                var confirmation = zanrRepository.CreateZanr(zanrEntity);

                zanrRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetZanrList", "Zanr", new { zanrId = confirmation.ZanrID });
                return Created(location, mapper.Map<Zanr>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
            }

        }

        [HttpDelete("{zanrId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteZanr(Guid zanrId)
        {
            try
            {
                var zanr = zanrRepository.GetZanrById(zanrId);

                zanrRepository.DeleteZanr(zanrId);
                zanrRepository.SaveChanges();
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
        public ActionResult<Zanr> UpdateZanr(Zanr zanr)
        {
            try
            {
                var oldZanr = zanrRepository.GetZanrById(zanr.ZanrID);
                if (oldZanr == null)
                {
                    return NotFound();
                }

                Zanr zanrEntity = mapper.Map<Zanr>(zanr);

                mapper.Map(zanrEntity, oldZanr);

                zanrRepository.SaveChanges();
                return Ok(mapper.Map<Zanr>(oldZanr));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
            }
        }



        [HttpOptions]
        public IActionResult GetZanrOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}

