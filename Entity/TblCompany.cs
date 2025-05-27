using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblCompany
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? CompanyPinCode { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public int Rtaid { get; set; }

    public int? FaceValue { get; set; }

    public virtual TblRtaCompany Rta { get; set; } = null!;

    public virtual ICollection<TblStock> TblStocks { get; set; } = new List<TblStock>();
}
