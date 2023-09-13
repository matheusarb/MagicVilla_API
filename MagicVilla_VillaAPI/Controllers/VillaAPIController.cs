using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Logging;

namespace MagicVilla_VillaAPI.Controllers 
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        public ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            this._logger = logger;
        }

        [HttpGet] // Read
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas", "info");
            return Ok(VillaStore.villaList);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        [HttpGet("{id:int}", Name ="GetVilla")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            _logger.Log($"Get villa error with id {id}", "error");
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost] // Create
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDto)
        {
            // podemos validar as propriedades do Model que usamos neste contexto de duas formas:
            // 1. Passando a validação através de atributos no VillaDTO E usando o atributo [ApiController], que é quem importará funcionalidades básicas e habilitará a leitura do código dos atributos lá anotados
            // 2. Validação explícita através do ModelState
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.villaList.FirstOrDefault(x => x.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This name already exists.");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest();
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(x=>x.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDto);

            return CreatedAtRoute("GetVilla", new { Id = villaDto.Id }, villaDto);
            ;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        // quando usamos "ActionResult", precisamos estabelecer o tipo de retorno do método/ação. Ao usar "IActionResult" não precisamos definir e, por se tratar de um HttpDelete, o retorno que pretendemos é um "no content" e, por isso, IActionResult é apropriado
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x=>x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x=>x.Id == id);
            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.Sqft = villaDto.Sqft;

            return NoContent();
        }
        
        [HttpPatch("{id:int}", Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x=>x.Id==id);
            if(villa == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}

