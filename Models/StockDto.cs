﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Entity;

public partial class StockDto
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? CompanyName { get; set; }

    public string? FirstHolder { get; set; }

    public string? SecondHolder { get; set; }

    public string? ThirdHolder { get; set; }

    public string? FolioNo { get; set; }

    public string? ClamStatus { get; set; }

    public double? Qty { get; set; }

    public double? Rate { get; set; }

    public double? Value { get; set; }

    public double? Brokerage { get; set; }

    public string? Ptbf { get; set; }

    public string? Remarks { get; set; }

    public bool? IsProcessed { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    [NotMapped]
	public string? CustomerName { get; set; }
	[NotMapped]
	public string? CreatedByName { get; set; }
	[NotMapped]
	public string? UpdatedByName { get; set; }
	
	


}