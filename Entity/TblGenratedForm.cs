using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblGenratedForm
{
    public long Id { get; set; }

    public string? FormName { get; set; }

    public long? StockId { get; set; }

    public string? Url { get; set; }

    public string? JsonString { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? ClientName { get; set; }

    public long? ClientId { get; set; }

    public virtual TblStock? Stock { get; set; }
}
