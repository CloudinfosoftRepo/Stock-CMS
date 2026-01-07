using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblNominee
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public string? Pan { get; set; }

    public string? Mobile { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? Pincode { get; set; }

    public string? BankName { get; set; }

    public string? Branch { get; set; }

    public string? AccountNo { get; set; }

    public string? AccountType { get; set; }

    public string? Ifsccode { get; set; }

    public string? Micrcode { get; set; }

    public string? BankCity { get; set; }

    public string? BankPinCode { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public long? CustomerId { get; set; }
}
