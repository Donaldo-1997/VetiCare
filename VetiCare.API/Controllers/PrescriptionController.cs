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
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<PrescriptionController> _logger;

        public PrescriptionController(
            IPrescriptionService prescriptionService,
            IMapper mapper,
            ILogger<PrescriptionController> logger)
        {
            _prescriptionService = prescriptionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionResponseDTO>>> GetAll()
        {
            var prescriptions = await _prescriptionService.GetAllAsync();
            var prescriptionsDto = _mapper.Map<IEnumerable<PrescriptionResponseDTO>>(prescriptions);
            return Ok(prescriptionsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionResponseDTO>> GetById(int id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);

            if (prescription == null)
                return NotFound(new { message = $"Prescripción con ID {id} no encontrada" });

            var prescriptionDto = _mapper.Map<PrescriptionResponseDTO>(prescription);
            return Ok(prescriptionDto);
        }

        [HttpGet("MedicalRecord/{MedicalRecordId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionResponseDTO>>> GetByMedicalRecord(int MedicalRecordId)
        {
            try
            {
                var prescriptions = await _prescriptionService.GetByMedicalRecordAsync(MedicalRecordId);
                var prescriptionsDto = _mapper.Map<IEnumerable<PrescriptionResponseDTO>>(prescriptions);
                return Ok(prescriptionsDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("Medicine/{MedicineId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionResponseDTO>>> GetByMedicine(int MedicineId)
        {
            try
            {
                var prescriptions = await _prescriptionService.GetByMedicineAsync(MedicineId);
                var prescriptionsDto = _mapper.Map<IEnumerable<PrescriptionResponseDTO>>(prescriptions);
                return Ok(prescriptionsDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionResponseDTO>> Create(PrescriptionRequestDTO dto)
        {
            try
            {
                var prescription = _mapper.Map<Prescription>(dto);
                var createdPrescription = await _prescriptionService.CreateAsync(prescription);

                // Recargar Details para Medicine y MedicalRecord
                var prescriptionWithDetails = await _prescriptionService.GetByIdWithDetailsAsync(createdPrescription.Id);
                var responseDto = _mapper.Map<PrescriptionResponseDTO>(prescriptionWithDetails);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = responseDto.Id },
                    responseDto);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, PrescriptionRequestDTO dto)
        {
            try
            {
                var prescription = _mapper.Map<Prescription>(dto);
                await _prescriptionService.UpdateAsync(id, prescription);
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
                await _prescriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
