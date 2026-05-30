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
    public class BreedController : ControllerBase
    {
        private readonly IBreedService _breedService;
        private readonly IMapper _mapper;
        private readonly ILogger<BreedController> _logger;

        public BreedController(IBreedService breedService, IMapper mapper, ILogger<BreedController> logger)
        {
            _breedService = breedService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreedResponseDTO>>> GetAll()
        {
            var breeds = await _breedService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BreedResponseDTO>>(breeds));
        }
        [HttpPost]
        public async Task<ActionResult<BreedResponseDTO>> Create([FromBody] BreedRequestDTO dto)
        {
            try
            {
                var breed = _mapper.Map<Breed>(dto);
                var created = await _breedService.CreateAsync(breed);
                var responseDto = _mapper.Map<BreedResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BreedResponseDTO>> GetById(int id)
        {
            var breed = await _breedService.GetByIdAsync(id);
            if (breed == null)
                return NotFound(new { message = $"Raza con ID {id} no encontrada" });
            return Ok(_mapper.Map<BreedResponseDTO>(breed));
        }
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<BreedResponseDTO>> GetByName(string name)
        {
            var breed = await _breedService.GetByNameAsync(name);
            if (breed == null)
                return NotFound(new { message = $"Raza con nombre {name} no encontrada" });
            return Ok(_mapper.Map<BreedResponseDTO>(breed));
        }
        [HttpGet("byspecies/{species}")]
        public async Task<ActionResult<IEnumerable<BreedResponseDTO>>> GetBySpecies(string species)
        {
            var breeds = await _breedService.GetBySpeciesAsync(species);
            if (breeds == null || !breeds.Any())
                return NotFound(new { message = $"No se encontraron razas para la especie {species}" });
            return Ok(_mapper.Map<IEnumerable<BreedResponseDTO>>(breeds));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, BreedRequestDTO dto)
        {
            try
            {
                var breed = _mapper.Map<Breed>(dto);
                await _breedService.UpdateAsync(id, breed);
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
                await _breedService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
