using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblDoc
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Pan { get; set; }

    public string? PanUrl { get; set; }

    public string? Aadhar { get; set; }

    public string? AadharUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblCustomer? Customer { get; set; }
}
