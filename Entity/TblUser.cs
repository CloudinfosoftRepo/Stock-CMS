using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblUser
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public long? ContactNo { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblRole Role { get; set; } = null!;
}
