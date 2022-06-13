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
    //[Authorize(Roles = "Admin, Zaposleni, Registrovani korisnik")]
    [ApiController]
    [Route("api/sediste-projekcije")]
    [Produces("application/json")]
    public class SedisteProjekcijeController : ControllerBase
    {
        private readonly ISedisteProjekcijeRepository sedisteProjekcijeRepository;
        private readonly ISedisteRepository sedisteRepository;
        private readonly IProjekcijaRepository projekcijaRepository;
        private readonly IKupovinaRepository kupovinaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;


        public SedisteProjekcijeController(ISedisteProjekcijeRepository sedisteProjekcijeRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService, ISedisteRepository sedisteRepository, IProjekcijaRepository projekcijaRepository, IKupovinaRepository kupovinaRepository)
        {
            this.sedisteProjekcijeRepository = sedisteProjekcijeRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;
            this.sedisteRepository = sedisteRepository;
            this.projekcijaRepository = projekcijaRepository;
            this.kupovinaRepository = kupovinaRepository;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<List<SedisteProjekcijeDto>> GetSedisteProjekcijeList([FromQuery] PaginationFilter filter, Guid kupovinaID, Guid projekcijaID)
        {
            List<SedisteProjekcije> sedisteProjekcijes = sedisteProjekcijeRepository.GetSedisteProjekcijeList(kupovinaID, projekcijaID);
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
            var pagedReponse = PaginationHelper.CreatePagedReponse<SedisteProjekcijeDto>(mapper.Map<List<SedisteProjekcijeDto>>(pagedData), validFilter, totalRecords, uriService, route);
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
            return Ok(mapper.Map<SedisteProjekcijeDto>(sedisteProjekcije));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SedisteProjekcije> CreateSedisteProjekcije([FromBody] SedisteProjekcije sedisteProjekcije)
        {
            try
            {
                if (sedisteProjekcije.KupovinaID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    sedisteProjekcije.KupovinaID = new Guid("2cc310d7-0fc9-49af-bf7d-a60954c7f69b");
                }

                var sedisteProjekcijeEntity = mapper.Map<SedisteProjekcije>(sedisteProjekcije);
                var confirmation = sedisteProjekcijeRepository.CreateSedisteProjekcije(sedisteProjekcijeEntity);

                confirmation.Sediste = sedisteProjekcijeRepository.GetSedisteProjekcijeById(confirmation.SedisteID, confirmation.ProjekcijaID).Sediste;
                confirmation.Projekcija = sedisteProjekcijeRepository.GetSedisteProjekcijeById(confirmation.SedisteID, confirmation.ProjekcijaID).Projekcija;
                confirmation.Kupovina = sedisteProjekcijeRepository.GetSedisteProjekcijeById(confirmation.SedisteID, confirmation.ProjekcijaID).Kupovina;

                sedisteProjekcijeRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetSedisteProjekcijeList", "SedisteProjekcije", new { sedisteId = confirmation.SedisteID, projekcijaId = confirmation.ProjekcijaID });
                return Created(location, mapper.Map<SedisteProjekcijeDto>(confirmation));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
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
        public ActionResult<SedisteProjekcije> UpdateSedisteProjekcije(SedisteProjekcije sedisteProjekcije)
        {
            try
            {
                sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteProjekcije.SedisteID, sedisteProjekcije.ProjekcijaID).Sediste =
                       sedisteRepository.GetSedisteById(sedisteProjekcije.SedisteID);
                sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteProjekcije.SedisteID, sedisteProjekcije.ProjekcijaID).Projekcija =
                       projekcijaRepository.GetProjekcijaById(sedisteProjekcije.ProjekcijaID);
                sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteProjekcije.SedisteID, sedisteProjekcije.ProjekcijaID).Kupovina =
                       kupovinaRepository.GetKupovinaById(sedisteProjekcije.KupovinaID);
                var oldSedisteProjekcije = sedisteProjekcijeRepository.GetSedisteProjekcijeById(sedisteProjekcije.SedisteID, sedisteProjekcije.ProjekcijaID);
                
                if (oldSedisteProjekcije == null)
                {
                    return NotFound();
                }

                Sediste s = oldSedisteProjekcije.Sediste;
                Projekcija p = oldSedisteProjekcije.Projekcija;
                Kupovina k = oldSedisteProjekcije.Kupovina;

                SedisteProjekcije sedisteProjekcijeEntity = mapper.Map<SedisteProjekcije>(sedisteProjekcije);

                mapper.Map(sedisteProjekcijeEntity, oldSedisteProjekcije);

                oldSedisteProjekcije.Sediste = s;
                oldSedisteProjekcije.Projekcija = p;
                oldSedisteProjekcije.Kupovina = k;

                sedisteProjekcijeRepository.SaveChanges();
                return Ok(mapper.Map<SedisteProjekcijeDto>(oldSedisteProjekcije));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.GetBaseException().Message);
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
