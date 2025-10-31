using System.ComponentModel.DataAnnotations;

namespace QuanLyKhamBenh.Core.DTOs
{
    public class DoctorCreateRequest
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public int? SpecialtyId { get; set; }

        public string? Qualifications { get; set; }
    }
}