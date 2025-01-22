using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models;

public partial class DocDto
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Pan { get; set; }

    public string? PanUrl { get; set; }

    public string? Aadhar { get; set; }

    public string? AadharUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
	[NotMapped]
	public IFormFile? PanFile { get; set; }
	[NotMapped]
	public IFormFile? AadharFile { get; set; }
	[NotMapped]
	public string? CreatedByName { get; set; }
	[NotMapped]
	public string? UpdatedByName { get; set; }
}

