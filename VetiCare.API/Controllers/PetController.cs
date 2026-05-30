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
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IMapper _mapper;
        private readonly ILogger<PetController> _logger;

        public PetController(IPetService petService, IMapper mapper, ILogger<PetController> logger)
        {
            _petService = petService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetResponseDTO>>> GetAll()
        {
            var pets = await _petService.GetAllWithDetailsAsync();
            return Ok(_mapper.Map<IEnumerable<PetResponseDTO>>(pets));
        }
        [HttpPost]
        public async Task<ActionResult<PetResponseDTO>> Create([FromBody] PetRequestDTO dto)
        {
            try
            {
                var pet = _mapper.Map<Pet>(dto);
                var created = await _petService.CreateAsync(pet);
                var responseDto = _mapper.Map<PetResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponseDTO>> GetById(int id)
        {
            var pet = await _petService.GetByIdWithDetailsAsync(id);
            if (pet == null)
                return NotFound(new { message = $"Mascota con ID {id} no encontrada" });
            return Ok(_mapper.Map<PetResponseDTO>(pet));
        }
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<PetResponseDTO>>> GetByOwnerId(int ownerId)
        {
            var pets = await _petService.GetByOwnerIdAsync(ownerId);
            return Ok(_mapper.Map<IEnumerable<PetResponseDTO>>(pets));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, PetRequestDTO dto)
        {
            try
            {
                var pet = _mapper.Map<Pet>(dto);
                await _petService.UpdateAsync(id, pet);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _petService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
