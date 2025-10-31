using System.ComponentModel.DataAnnotations;

namespace QuanLyKhamBenh.Core.DTOs
{
    public class DoctorUpdateRequest
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

        public int? SpecialtyId { get; set; }

        public string? Qualifications { get; set; }
    }
}