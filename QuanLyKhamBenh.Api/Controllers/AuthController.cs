using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLyKhamBenh.Core.Data;
using QuanLyKhamBenh.Core.DTOs; // Sử dụng DTO
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyKhamBenh.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly QuanLyKhamBenhDBContext _context;
		private readonly IConfiguration _configuration;

		// "Tiêm" DbContext và Configuration vào
		public AuthController(QuanLyKhamBenhDBContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		// Đây là API đăng nhập của bạn
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
		{
			// === TRƯỜNG HỢP 1: Đăng nhập bằng SĐT (Bác sĩ hoặc Bệnh nhân) ===
			if (!string.IsNullOrEmpty(loginRequest.Phone))
			{
				// 1. Thử tìm trong Bệnh nhân
				var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Phone == loginRequest.Phone);
				if (patient != null)
				{
					// 2. So sánh mật khẩu
					if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, patient.PasswordHash))
					{
						// Mật khẩu đúng! Tạo token cho Bệnh nhân
						var token = GenerateJwtToken(patient.PatientId.ToString(), patient.Email, "Patient");
						return Ok(new { token });
					}
				}

				// 3. Thử tìm trong Bác sĩ
				var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Phone == loginRequest.Phone);
				if (doctor != null)
				{
					// 4. So sánh mật khẩu
					if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, doctor.PasswordHash))
					{
						// Mật khẩu đúng! Tạo token cho Bác sĩ
						var token = GenerateJwtToken(doctor.DoctorId.ToString(), doctor.Email, "Doctor");
						return Ok(new { token });
					}
				}
			}
			// === TRƯỜNG HỢP 2: Đăng nhập bằng Username (Admin) ===
			else if (!string.IsNullOrEmpty(loginRequest.Username))
			{
				// 1. Thử tìm trong Admin
				var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == loginRequest.Username);
				if (admin != null)
				{
					// 2. So sánh mật khẩu
					if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, admin.PasswordHash))
					{
						// Mật khẩu đúng! Tạo token cho Admin
						var token = GenerateJwtToken(admin.AdminId.ToString(), admin.Email, "Admin");
						return Ok(new { token });
					}
				}
			}

			// Nếu không tìm thấy ai hoặc sai mật khẩu
			return Unauthorized(new { message = "Thông tin đăng nhập không đúng." });
		}

		// --- Hàm trợ giúp để tạo JWT Token ---
		private string GenerateJwtToken(string userId, string email, string role)
		{
			var jwtKey = _configuration["Jwt:Key"];
			if (string.IsNullOrEmpty(jwtKey))
			{
				throw new InvalidOperationException("Chưa cài đặt JWT Key trong appsettings.json");
			}

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			;

			// "Claims" là thông tin bạn muốn mã hóa vào trong Token
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId), // ID của người dùng
                new Claim(JwtRegisteredClaimNames.Email, email), // Email (để tiện sử dụng)
                new Claim(ClaimTypes.Role, role), // Vai trò (Patient, Doctor, Admin)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(3), // Token hết hạn sau 3 giờ
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}