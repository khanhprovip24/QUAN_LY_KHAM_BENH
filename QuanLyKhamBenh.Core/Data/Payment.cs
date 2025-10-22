using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
