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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetAll()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // GET: api/appointments/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppointmentResponseDTO>> GetById(int id)
        {
            var appointment = await _appointmentService.GetWithDetailsAsync(id);

            if (appointment == null)
                return NotFound(new { message = $"Cita con ID {id} no encontrada." });

            return Ok(_mapper.Map<AppointmentResponseDTO>(appointment));
        }

        // GET: api/appointments/pet/3
        [HttpGet("pet/{petId:int}")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetByPet(int petId)
        {
            var appointments = await _appointmentService.GetByPetIdAsync(petId);
            return Ok(_mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // GET: api/appointments/vet/2
        [HttpGet("vet/{vetId:int}")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetByVet(int vetId)
        {
            var appointments = await _appointmentService.GetByVetIdAsync(vetId);
            return Ok(_mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // GET: api/appointments/status/0
        [HttpGet("status/{status:int}")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetByStatus(int status)
        {
            if (!Enum.IsDefined(typeof(Domain.Enums.AppointmentStatus), status))
                return BadRequest(new { message = "Estado de cita inválido." });

            var appointments = await _appointmentService.GetByStatusAsync(
                (Domain.Enums.AppointmentStatus)status);

            return Ok(_mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<AppointmentResponseDTO>> Create(AppointmentRequestDTO dto)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(dto);
                var created = await _appointmentService.CreateAsync(appointment);
                var responseDto = _mapper.Map<AppointmentResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/appointments/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, AppointmentRequestDTO dto)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(dto);
                await _appointmentService.UpdateAsync(id, appointment);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH: api/appointments/5/status
        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult> UpdateStatus(int id, AppointmentStatusRequestDTO dto)
        {
            try
            {
                await _appointmentService.UpdateStatusAsync(id, dto.Status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _appointmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
