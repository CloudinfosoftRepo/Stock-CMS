﻿using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblDoc
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Pan { get; set; }

    public string? Panurl { get; set; }

    public string? Aadhar { get; set; }

    public string? AadharUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? NameAsPerPan { get; set; }

    public string? NameAsPerAadhar { get; set; }

    public string? AddressAsPerAadhar { get; set; }

    public DateTime? Dob { get; set; }

    public string? Mobile { get; set; }

    public string? Relationship { get; set; }

    public string? RelativesName { get; set; }

    public string? Email { get; set; }

    public string? DeathCertiUrl { get; set; }

    public string? NameAsPerDeathCerti { get; set; }

    public DateTime? DateOfDeath { get; set; }

    public string? PlaceOfDeath { get; set; }

    public string? VoterIdUrl { get; set; }

    public string? NameAsPerVoterId { get; set; }

    public string? AddressAsPerVoterId { get; set; }

    public string? Dpid { get; set; }

    public string? ClientId { get; set; }

    public string? WitnessJson { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? PinCode { get; set; }

    public string? NameAsPerCertificate { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? DpName { get; set; }

    public virtual TblCustomer? Customer { get; set; }

    public virtual ICollection<TblStock> TblStockFirstHolders { get; set; } = new List<TblStock>();

    public virtual ICollection<TblStock> TblStockSecondHolders { get; set; } = new List<TblStock>();

    public virtual ICollection<TblStock> TblStockThirdHolders { get; set; } = new List<TblStock>();
}
