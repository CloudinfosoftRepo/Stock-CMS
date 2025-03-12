using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models;

public partial class StockDto
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? FolioNo { get; set; }

    public string? ClaimStatus { get; set; }

    public double? ActualQty { get; set; }

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

    public string? StockJson { get; set; }

    public int? CompanyId { get; set; }

    public long? FirstHolderId { get; set; }

    public long? SecondHolderId { get; set; }

    public long? ThirdHolderId { get; set; }

    public string? NomineeJson { get; set; }

    [NotMapped]
    public string? CompanyName { get; set; }

    [NotMapped]
    public string? FirstHolderName { get; set; }

    [NotMapped]
    public string? SecondHolderName { get; set; }

    [NotMapped]
    public string? ThirdHolderName { get; set; }


    [NotMapped]
	public string? CustomerName { get; set; }
	[NotMapped]
	public string? CreatedByName { get; set; }
	[NotMapped]
	public string? UpdatedByName { get; set; }	
    
    [NotMapped]
	public bool IsClient { get; set; }

	public virtual CustomerDto? Customer { get; set; }
    public virtual CompanyDto? Company { get; set; }
    public virtual DocDto? FirstHolderData { get; set; }
    public virtual DocDto? SecondHolderData { get; set; }
    public virtual DocDto? ThirdHolderData { get; set; }


}

public class UploadStockDto
{
    public long? CustomerId { get; set; }

    [NotMapped]
    public IFormFile? DocFile { get; set; }
}
