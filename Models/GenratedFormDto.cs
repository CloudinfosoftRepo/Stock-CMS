using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models;

public partial class GenratedFormDto
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

    [NotMapped]
    public string? CreatedByName { get; set; }
    [NotMapped]
    public string? UpdatedByName { get; set; }
    [NotMapped]
    public string? CompanyName { get; set; }
    [NotMapped]
    public string? CustomerName { get; set; }
    [NotMapped]
    public string? FolioNo { get; set; }
}
