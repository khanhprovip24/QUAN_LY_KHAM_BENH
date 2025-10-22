using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int AppointmentId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public string? Instructions { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Medicine Medicine { get; set; } = null!;
}
