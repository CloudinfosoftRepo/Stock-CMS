﻿using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblStock
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? CompanyName { get; set; }

    public string? FirstHolder { get; set; }

    public string? SecondHolder { get; set; }

    public string? ThirdHolder { get; set; }

    public string? FolioNo { get; set; }

    public string? ClamStatus { get; set; }

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

    public virtual TblCompany? Company { get; set; }

    public virtual TblCustomer? Customer { get; set; }

    public virtual TblDoc? FirstHolderNavigation { get; set; }

    public virtual TblDoc? SecondHolderNavigation { get; set; }

    public virtual ICollection<TblGenratedForm> TblGenratedForms { get; set; } = new List<TblGenratedForm>();

    public virtual TblDoc? ThirdHolderNavigation { get; set; }
}
