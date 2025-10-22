using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int? DoctorId { get; set; }

    public int SpecialtyId { get; set; }

    public DateTime AppointmentDatetime { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public string? BookingCode { get; set; }

    public string? Symptoms { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual Specialty Specialty { get; set; } = null!;
}
