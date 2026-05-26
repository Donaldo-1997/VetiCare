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
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicalRecordController> _logger;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService, 
            IMapper mapper, 
            ILogger<MedicalRecordController> logger)
        {
            _medicalRecordService = medicalRecordService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordResponseDTO>>> GetAll()
        {
            var medicalRecords = await _medicalRecordService.GetAllWithDetailsAsync();
            var medicalRecordsDto = _mapper.Map<IEnumerable<MedicalRecordResponseDTO>>(medicalRecords);
            return Ok(medicalRecordsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordResponseDTO>> GetById(int id)
        {
            var medicalRecord = await _medicalRecordService.GetByIdWithDetailsAsync(id);

            if (medicalRecord == null)
                return NotFound(new { message = $"Registro médico con ID {id} no encontrado" });

            var medicalRecordDto = _mapper.Map<MedicalRecordResponseDTO>(medicalRecord);
            return Ok(medicalRecordDto);
        }

        [HttpPost]
        public async Task<ActionResult<MedicalRecordResponseDTO>> Create(MedicalRecordRequestDTO dto)
        {
            try
            {
                var medicalRecord = _mapper.Map<MedicalRecord>(dto);
                var createdMedicalRecord = await _medicalRecordService.CreateAsync(medicalRecord);
               

                var medicalRecordWithDetails = await _medicalRecordService.GetByIdWithDetailsAsync(createdMedicalRecord.Id);
                var responseDto = _mapper.Map<MedicalRecordResponseDTO>(medicalRecordWithDetails);

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
        public async Task<ActionResult> Update(int id, MedicalRecordRequestDTO dto)
        {
            try
            {
                var medicalRecord = _mapper.Map<MedicalRecord>(dto);
                await _medicalRecordService.UpdateAsync(id, medicalRecord);
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
                await _medicalRecordService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
