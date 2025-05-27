using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblMenu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public int? Sequence { get; set; }

    public int? ParentId { get; set; }

    public string? Actions { get; set; }

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<TblPermission> TblPermissions { get; set; } = new List<TblPermission>();
}
