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
    [ApiController]
    [Route("api/kupovina")]
    [Produces("application/json")]
    [Authorize]
    public class KupovinaController : ControllerBase
    {
        private readonly IKupovinaRepository kupovinaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public KupovinaController(IKupovinaRepository kupovinaRepository, LinkGenerator linkGenerator, IMapper mapper, IUriService uriService)
        {
            this.kupovinaRepository = kupovinaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.uriService = uriService;

        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Kupovina>> GetKupovinaList([FromQuery] PaginationFilter filter, bool placeno)
        {
            List<Kupovina> kupovinas = kupovinaRepository.GetKupovinaList(placeno);
            if (kupovinas == null || kupovinas.Count == 0)
            {

                return NoContent();
            }

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = kupovinas
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = kupovinas.Count;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Kupovina>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{kupovinaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Kupovina> GetKupovinaById(Guid kupovinaId)
        {
            Kupovina kupovina = kupovinaRepository.GetKupovinaById(kupovinaId);
            if (kupovina == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Kupovina>(kupovina));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Kupovina> CreateKupovina([FromBody] Kupovina kupovina)
        {
            try
            {

                var kupovinaEntity = mapper.Map<Kupovina>(kupovina);
                var confirmation = kupovinaRepository.CreateKupovina(kupovinaEntity);

                kupovinaRepository.SaveChanges();
                string location = linkGenerator.GetPathByAction("GetKupovinaList", "Kupovina", new { kupovinaId = confirmation.KupovinaID });
                return Created(location, mapper.Map<Kupovina>(confirmation));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when creating an object");
            }

        }

        [HttpDelete("{kupovinaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteKupovina(Guid kupovinaId)
        {
            try
            {
                var kupovina = kupovinaRepository.GetKupovinaById(kupovinaId);

                kupovinaRepository.DeleteKupovina(kupovinaId);
                kupovinaRepository.SaveChanges();
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
        public ActionResult<Kupovina> UpdateKupovina(Kupovina kupovina)
        {
            try
            {
                var oldKupovina = kupovinaRepository.GetKupovinaById(kupovina.KupovinaID);
                if (oldKupovina == null)
                {
                    return NotFound();
                }

                Kupovina kupovinaEntity = mapper.Map<Kupovina>(kupovina);


                //Podesavanje vremena rezervacije i placanja prilikom update
                kupovinaEntity.VremeRezervacije = oldKupovina.VremeRezervacije;

                if (kupovinaEntity.Placeno == true && oldKupovina.Placeno == false)
                {
                    kupovinaEntity.VremePlacanja = DateTime.Now;
                }
                else
                {
                    kupovinaEntity.VremePlacanja = oldKupovina.VremePlacanja;
                }

                mapper.Map(kupovinaEntity, oldKupovina);

                kupovinaRepository.SaveChanges();
                return Ok(mapper.Map<Kupovina>(oldKupovina));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred when updating an object");
            }
        }



        [HttpOptions]
        public IActionResult GetKupovinaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");

            return Ok();
        }
    }
}