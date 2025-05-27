using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblCustomer
{
    public long Id { get; set; }

    public string? CustomerName { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsClient { get; set; }

    public string? Reference { get; set; }

    public string? ContactPersonName { get; set; }

    public string? ContactPersonMobile { get; set; }

    public string? FileNo { get; set; }

    public string? Location { get; set; }

    public string? FinancialYear { get; set; }

    public string? DocumentJson { get; set; }

    public virtual ICollection<TblDoc> TblDocs { get; set; } = new List<TblDoc>();

    public virtual ICollection<TblStock> TblStocks { get; set; } = new List<TblStock>();
}
