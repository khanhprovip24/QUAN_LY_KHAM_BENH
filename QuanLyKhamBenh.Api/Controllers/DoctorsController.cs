using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhamBenh.Core.Data;
using QuanLyKhamBenh.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhamBenh.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorsController : ControllerBase
	{
		private readonly QuanLyKhamBenhDBContext _context;

		public DoctorsController(QuanLyKhamBenhDBContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Lấy danh sách tất cả bác sĩ.
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAllDoctors()
		{
			var doctors = await _context.Doctors
				.Include(d => d.Specialty)
				.Select(d => new DoctorDto
				{
					DoctorId = d.DoctorId,
					FullName = d.FullName,
					Email = d.Email,
					Phone = d.Phone,
					SpecialtyId = d.SpecialtyId,
					SpecialtyName = d.Specialty != null ? d.Specialty.Name : null,
					Qualifications = d.Qualifications
				})
				.ToListAsync();

			return Ok(doctors);
		}

		/// <summary>
		/// Lấy thông tin chi tiết một bác sĩ theo ID.
		/// </summary>
		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<ActionResult<DoctorDto>> GetDoctorById(int id)
		{
			var doctor = await _context.Doctors
				.Include(d => d.Specialty)
				.Where(d => d.DoctorId == id)
				.Select(d => new DoctorDto
				{
					DoctorId = d.DoctorId,
					FullName = d.FullName,
					Email = d.Email,
					Phone = d.Phone,
					SpecialtyId = d.SpecialtyId,
					SpecialtyName = d.Specialty != null ? d.Specialty.Name : null,
					Qualifications = d.Qualifications
				})
				.FirstOrDefaultAsync();

			if (doctor == null)
			{
				return NotFound(new { message = "Không tìm thấy bác sĩ." });
			}

			return Ok(doctor);
		}

		/// <summary>
		/// Tạo mới một bác sĩ.
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<DoctorDto>> CreateNewDoctor([FromBody] DoctorCreateRequest request)
		{
			if (await _context.Doctors.AnyAsync(d => d.Email == request.Email) || await _context.Patients.AnyAsync(p => p.Email == request.Email))
			{
				return BadRequest(new { message = "Email đã được sử dụng." });
			}
			if (await _context.Doctors.AnyAsync(d => d.Phone == request.Phone) || await _context.Patients.AnyAsync(p => p.Phone == request.Phone))
			{
				return BadRequest(new { message = "Số điện thoại đã được sử dụng." });
			}

			var doctor = new Doctor
			{
				FullName = request.FullName,
				Email = request.Email,
				Phone = request.Phone,
				SpecialtyId = request.SpecialtyId,
				Qualifications = request.Qualifications,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
			};

			_context.Doctors.Add(doctor);
			await _context.SaveChangesAsync();

			var doctorDto = new DoctorDto
			{
				DoctorId = doctor.DoctorId,
				FullName = doctor.FullName,
				Email = doctor.Email,
				Phone = doctor.Phone,
				SpecialtyId = doctor.SpecialtyId,
				Qualifications = doctor.Qualifications
			};

			return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorId }, doctorDto);
		}

		/// <summary>
		/// Cập nhật thông tin một bác sĩ.
		/// </summary>
		[HttpPut("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> UpdateDoctorInfo(int id, [FromBody] DoctorUpdateRequest request)
		{
			var doctor = await _context.Doctors.FindAsync(id);

			if (doctor == null)
			{
				return NotFound(new { message = "Không tìm thấy bác sĩ." });
			}

			if (await _context.Doctors.AnyAsync(d => d.DoctorId != id && d.Email == request.Email))
			{
				return BadRequest(new { message = "Email đã được sử dụng bởi một tài khoản khác." });
			}
			if (await _context.Doctors.AnyAsync(d => d.DoctorId != id && d.Phone == request.Phone))
			{
				return BadRequest(new { message = "Số điện thoại đã được sử dụng bởi một tài khoản khác." });
			}

			doctor.FullName = request.FullName;
			doctor.Email = request.Email;
			doctor.Phone = request.Phone;
			doctor.SpecialtyId = request.SpecialtyId;
			doctor.Qualifications = request.Qualifications;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		/// <summary>
		/// Xóa một bác sĩ.
		/// </summary>
		[HttpDelete("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> DeleteDoctorById(int id)
		{
			var doctor = await _context.Doctors.FindAsync(id);
			if (doctor == null)
			{
				return NotFound(new { message = "Không tìm thấy bác sĩ." });
			}

			var hasAppointments = await _context.Appointments.AnyAsync(a => a.DoctorId == id);
			if (hasAppointments)
			{
				return BadRequest(new { message = "Không thể xóa bác sĩ vì đã có lịch hẹn liên quan." });
			}

			_context.Doctors.Remove(doctor);
			await _context.SaveChangesAsync();

			return Ok(new { message = "Xóa bác sĩ thành công." });
		}
	}
}