using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblRtaCompany
{
    public int Id { get; set; }

    public string? RtaName { get; set; }

    public string? RtaAddress { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<TblCompany> TblCompanies { get; set; } = new List<TblCompany>();
}
