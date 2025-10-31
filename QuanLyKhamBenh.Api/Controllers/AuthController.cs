using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhamBenh.Core.Data;
using QuanLyKhamBenh.Core.DTOs;

namespace QuanLyKhamBenh.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly QuanLyKhamBenhDBContext _context;
		private readonly IConfiguration _configuration;

		public AuthController(QuanLyKhamBenhDBContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
		{
			if (!string.IsNullOrEmpty(loginRequest.Phone))
			{
				var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Phone == loginRequest.Phone);
				if (patient != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, patient.PasswordHash))
				{
					return Ok(new { role = "Patient", userId = patient.PatientId, name = patient.FullName, email = patient.Email });
				}

				var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Phone == loginRequest.Phone);
				if (doctor != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, doctor.PasswordHash))
				{
					return Ok(new { role = "Doctor", userId = doctor.DoctorId, name = doctor.FullName, email = doctor.Email });
				}
			}
			else if (!string.IsNullOrEmpty(loginRequest.Username))
			{
				var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == loginRequest.Username);
				if (admin != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, admin.PasswordHash))
				{
					return Ok(new { role = "Admin", userId = admin.AdminId, name = admin.Username, email = admin.Email });
				}
			}

			return Unauthorized(new { message = "Thông tin đăng nhập không đúng." });
		}

		

		
	}
}