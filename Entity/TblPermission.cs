using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblPermission
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public string? Actions { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblMenu Menu { get; set; } = null!;

    public virtual TblRole Role { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
