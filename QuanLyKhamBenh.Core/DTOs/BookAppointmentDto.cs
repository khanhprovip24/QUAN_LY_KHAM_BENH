using System.ComponentModel.DataAnnotations;

namespace QuanLyKhamBenh.Core.DTOs
{
    public class BookAppointmentDto
    {
        public int? DoctorId { get; set; } // Có thể null nếu chỉ chọn chuyên khoa
        [Required]
        public int SpecialtyId { get; set; }
        [Required]
        public DateTime AppointmentDatetime { get; set; }
        public string? Symptoms { get; set; }
    }
}