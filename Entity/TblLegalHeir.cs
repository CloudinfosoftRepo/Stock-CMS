using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblLegalHeir
{
    public long Id { get; set; }

    public long? DocId { get; set; }

    public string? Name { get; set; }

    public string? Pan { get; set; }

    public string? Panurl { get; set; }

    public string? Aadhar { get; set; }

    public string? AadharUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? NameAsPerPan { get; set; }

    public string? NameAsPerAadhar { get; set; }

    public string? AddressAsPerAadhar { get; set; }

    public DateTime? Dob { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public bool? IsClaiment { get; set; }

    public bool? IsNoc { get; set; }
}
