namespace QuanLyKhamBenh.Core.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Address { get; set; }
    }
}