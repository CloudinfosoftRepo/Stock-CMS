﻿using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class CustomerDto
{
    public long Id { get; set; } = 0;

    public string? CustomerName { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}