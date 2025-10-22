using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int PatientId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
