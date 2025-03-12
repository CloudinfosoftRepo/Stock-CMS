using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class BankDto
    {
        public long Id { get; set; }

        public long? ClientId { get; set; }

        public string? BankName { get; set; }

        public string? Branch { get; set; }

        public string? PostalAddress { get; set; }

        public string? PhoneNo { get; set; }

        public string? BankEmail { get; set; }

        public string? AccountNo { get; set; }

        public DateTime? AccountOpeningDate { get; set; }

        public string? BankManagerName { get; set; }

        public string? EmployeeCode { get; set; }

        public string? BankMangerEmail { get; set; }

        public string? Ifsccode { get; set; }

        public string? Micrcode { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public string? AccountType { get; set; }

        public long? LegalHeirId { get; set; }

        public string? BankCity { get; set; }

        public string? PinCode { get; set; }

        public string? NameAsPerBankAccount { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
    }
}
