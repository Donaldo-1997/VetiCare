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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerController> _logger;

        public OwnerController(IOwnerService ownerService, IMapper mapper, ILogger<OwnerController> logger)
        {
            _ownerService = ownerService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerResponseDTO>>> GetAll()
        {
            var owners = await _ownerService.GetAllWithPetsAsync();
            return Ok(_mapper.Map<IEnumerable<OwnerResponseDTO>>(owners));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerResponseDTO>> GetById(int id)
        {
            var owner = await _ownerService.GetByIdWithPetsAsync(id);
            if (owner == null)
                return NotFound(new { message = $"Propietario con ID {id} no encontrado" });
            return Ok(_mapper.Map<OwnerResponseDTO>(owner));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<OwnerResponseDTO>> GetByEmail(string email)
        {
            var owner = await _ownerService.GetByEmailAsync(email);
            if (owner == null)
                return NotFound(new { message = $"Propietario con email {email} no encontrado" });
            return Ok(_mapper.Map<OwnerResponseDTO>(owner));
        }

        [HttpGet("phone/{phone}")]
        public async Task<ActionResult<OwnerResponseDTO>> GetByPhone(string phone)
        {
            var owner = await _ownerService.GetByPhoneAsync(phone);
            if (owner == null)
                return NotFound(new { message = $"Propietario con teléfono {phone} no encontrado" });
            return Ok(_mapper.Map<OwnerResponseDTO>(owner));
        }

        [HttpPost]
        public async Task<ActionResult<OwnerResponseDTO>> Create([FromBody] OwnerRequestDTO dto)
        {
            try
            {
                var owner = _mapper.Map<Owner>(dto);
                var created = await _ownerService.CreateAsync(owner);
                var responseDto = _mapper.Map<OwnerResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] OwnerRequestDTO dto)
        {
            try
            {
                var owner = _mapper.Map<Owner>(dto);
                await _ownerService.UpdateAsync(id, owner);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _ownerService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
