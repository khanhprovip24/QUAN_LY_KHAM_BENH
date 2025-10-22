using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? SpecialtyId { get; set; }

    public string? Qualifications { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Specialty? Specialty { get; set; }
}
