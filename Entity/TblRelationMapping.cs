using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblRelationMapping
{
    public long Id { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public long? HolderId { get; set; }

    public long? LegalHeirId { get; set; }

    public string? RelationWithDead { get; set; }

    public long? LegalHeirParentId { get; set; }

    public long? NomineeId { get; set; }
}
