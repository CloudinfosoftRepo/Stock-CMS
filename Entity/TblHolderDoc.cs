using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblHolderDoc
{
    public long Id { get; set; }

    public string? DocumentName { get; set; }

    public string? DocUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public long? HolderId { get; set; }

    public long? LegalHeirId { get; set; }
}
