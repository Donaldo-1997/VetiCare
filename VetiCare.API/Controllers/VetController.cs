using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VetiCare.API.DTOs.Request;
using VetiCare.API.DTOs.Response;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VetsController : ControllerBase
    {
        private readonly IVetService _vetService;
        private readonly IMapper _mapper;

        public VetsController(
            IVetService vetService,
            IMapper mapper)
        {
            _vetService = vetService;
            _mapper = mapper;
        }

        // GET: api/vets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VetResponseDTO>>> GetAll()
        {
            var vets = await _vetService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VetResponseDTO>>(vets));
        }

        // GET: api/vets/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VetResponseDTO>> GetById(int id)
        {
            var vet = await _vetService.GetByIdAsync(id);

            if (vet == null)
                return NotFound(new { message = $"Veterinario con ID {id} no encontrado" });

            return Ok(_mapper.Map<VetResponseDTO>(vet));
        }

        // GET: api/vets/with-appointments
        [HttpGet("with-appointments")]
        public async Task<ActionResult<IEnumerable<VetResponseDTO>>> GetWithAppointments()
        {
            var vets = await _vetService.GetAllWithAppointmentsAsync();
            return Ok(_mapper.Map<IEnumerable<VetResponseDTO>>(vets));
        }

        // POST: api/vets
        [HttpPost]
        public async Task<ActionResult<VetResponseDTO>> Create(VetRequestDTO dto)
        {
            var vet = _mapper.Map<Vet>(dto);
            var created = await _vetService.CreateAsync(vet);
            var responseDto = _mapper.Map<VetResponseDTO>(created);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        // PUT: api/vets/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, VetRequestDTO dto)
        {
            try
            {
                var vet = _mapper.Map<Vet>(dto);
                await _vetService.UpdateAsync(id, vet);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/vets/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _vetService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
