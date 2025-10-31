using System.ComponentModel.DataAnnotations;

namespace QuanLyKhamBenh.Core.DTOs
{
    public class PatientUpdateRequest
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
        public DateOnly BirthDate { get; set; }

        public string? Address { get; set; }
    }
}