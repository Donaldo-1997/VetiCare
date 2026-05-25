using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VetiCare.API.DTOs.Request;
using VetiCare.API.DTOs.Response;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Services;
using VetiCare.Domain.Services;

namespace VetiCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IMapper _mapper;
        private ILogger<MedicineController> _logger;

        public MedicineController(
            IMedicineService medicineService, 
            IMapper mapper, 
            ILogger<MedicineController> logger)
        {
            _medicineService = medicineService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineResponseDTO>>> GetAll()
        {
            var medicines = await _medicineService.GetAllAsync();
            var medicinesDto = _mapper.Map<IEnumerable<MedicineResponseDTO>>(medicines);
            return Ok(medicinesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineResponseDTO>> GetById(int id)
        {
            var medicine = await _medicineService.GetByIdAsync(id);

            if (medicine == null)
                return NotFound(new { message = $"Medicina con ID {id} no encontrada" });

            var medicineDto = _mapper.Map<MedicineResponseDTO>(medicine);
            return Ok(medicineDto);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<MedicineResponseDTO>> GetByName(string name)
        {
            try
            {
                var medicine = await _medicineService.GetByNameAsync(name);
                if (medicine == null)
                    return NotFound(new { message = $"Medicamento con nombre {name} no encontrado" });
                return Ok(_mapper.Map<MedicineResponseDTO>(medicine));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicineResponseDTO>> Create(MedicineRequestDTO dto)
        {
            try
            {
                var medicine = _mapper.Map<Medicine>(dto);
                var createdMedicine = await _medicineService.CreateAsync(medicine);
                var responseDto = _mapper.Map<MedicineResponseDTO>(createdMedicine);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = responseDto.Id },
                    responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MedicineRequestDTO dto)
        {
            try
            {
                var medicine = _mapper.Map<Medicine>(dto);
                await _medicineService.UpdateAsync(id, medicine);
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
                await _medicineService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
