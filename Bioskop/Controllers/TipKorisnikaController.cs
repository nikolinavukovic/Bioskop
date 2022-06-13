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
    [Route("api/tip-korisnika")]
    [Produces("application/json")]
    public class TipKorisnikaController : ControllerBase
    {
        private readonly ITipKorisnikaRepository tipKorisnikaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public int FromQuery { get; private set; }

        public TipKorisnikaController(ITipKorisnikaRepository tipKorisnikaRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.tipKorisnikaRepository = tipKorisnikaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<TipKorisnika>> GetTipKorisnikaList([FromQuery] PaginationFilter filter)
        {
            List<TipKorisnika> tipKorisnikas = tipKorisnikaRepository.GetTipKorisnikaList();
            if (tipKorisnikas == null || tipKorisnikas.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = tipKorisnikas
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = tipKorisnikas.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<TipKorisnika>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{tipKorisnikaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TipKorisnika> GetTipKorisnikaById(Guid tipKorisnikaId)
        {
            TipKorisnika tipKorisnika = tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnikaId);
            if (tipKorisnika == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TipKorisnika>(tipKorisnika));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TipKorisnika> CreateTipKorisnika([FromBody] TipKorisnika tipKorisnika)
        {
            try
            {

                var tipKorisnikaEntity = mapper.Map<TipKorisnika>(tipKorisnika);
                var confirmation = tipKorisnikaRepository.CreateTipKorisnika(tipKorisnikaEntity);


                tipKorisnikaRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetTipKorisnikaList", "TipKorisnika", new { tipKorisnikaId = confirmation.TipKorisnikaID });
                return Created(location, mapper.Map<TipKorisnika>(confirmation));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }

        }

        [HttpDelete("{tipKorisnikaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTipKorisnika(Guid tipKorisnikaId)
        {
            try
            {
                var tipKorisnika = tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnikaId);

                tipKorisnikaRepository.DeleteTipKorisnika(tipKorisnikaId);
                tipKorisnikaRepository.SaveChanges();
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
        public ActionResult<TipKorisnika> UpdateTipKorisnika(TipKorisnika tipKorisnika)
        {
            try
            {
                var oldTip = tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnika.TipKorisnikaID);
                if (oldTip == null)
                {
                    return NotFound();
                }
                TipKorisnika tipKorisnikaEntity = mapper.Map<TipKorisnika>(tipKorisnika);

                mapper.Map(tipKorisnikaEntity, oldTip);

                tipKorisnikaRepository.SaveChanges();
                return Ok(mapper.Map<TipKorisnika>(oldTip));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
            }
        }



        [HttpOptions]
        public IActionResult GetTipKorisnikaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}
