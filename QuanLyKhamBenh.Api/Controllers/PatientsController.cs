using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhamBenh.Core.Data;
using QuanLyKhamBenh.Core.DTOs;
using System.Security.Claims;

namespace QuanLyKhamBenh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
            // Chỉ Bệnh nhân mới có quyền truy cập
    public class PatientsController : ControllerBase
    {
        private readonly QuanLyKhamBenhDBContext _context;

        public PatientsController(QuanLyKhamBenhDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Đặt lịch hẹn mới cho một bệnh nhân cụ thể.
        /// </summary>
        [HttpPost("{patientId}/appointments")]
        [AllowAnonymous]
        public async Task<IActionResult> BookAppointment(int patientId, [FromBody] BookAppointmentDto bookingDto)
        {
            // Lấy ID của bệnh nhân đang đăng nhập từ token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            int userId = int.Parse(userIdString);

            // You may want to add logic here to book the appointment for the specified patientId
            // For now, just return the userId for demonstration
            return Ok(new { userId });
        }

        // Đặt lịch hẹn mới
        [HttpPost("me/appointments")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientId =     GetCurrentPatientId();

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = bookingDto.DoctorId,
                SpecialtyId = bookingDto.SpecialtyId,
                AppointmentDatetime = bookingDto.AppointmentDatetime,
                Symptoms = bookingDto.Symptoms,
                Status = "pending" // Trạng thái mặc định
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt lịch hẹn thành công!", appointmentId = appointment.AppointmentId });
        }

        // Xem lịch sử khám bệnh
        [HttpGet("me/history")]
        public async Task<IActionResult> GetMyHistory()
        {
            var patientId = GetCurrentPatientId();
            var history = await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .Include(a => a.Specialty)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentDatetime,
                    a.Status,
                    DoctorName = a.Doctor.FullName,
                    SpecialtyName = a.Specialty.Name,
                    a.Symptoms
                })
                .OrderByDescending(a => a.AppointmentDatetime)
                .ToListAsync();

            return Ok(history);
        }

        // Helper method to get current patient ID from claims
        private int GetCurrentPatientId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            return int.Parse(userIdString);
        }
    }
}