using System;
using System.Collections.Generic;

namespace QuanLyKhamBenh.Core.Data;

public partial class AiRecommendation
{
    public int RecommendationId { get; set; }

    public int? PatientId { get; set; }

    public string SymptomsInput { get; set; } = null!;

    public int? DoctorId { get; set; }

    public int? SpecialtyId { get; set; }

    public DateTime? RecommendationDate { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Specialty? Specialty { get; set; }
}
