using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class CompanyDto
    {
        public int Id { get; set; }

        public string? CompanyName { get; set; }

        public string? CompanyAddress { get; set; }

        public string? CompanyPinCode { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public int Rtaid { get; set; }

        [NotMapped]
        public string? RtaName { get; set; }

        [NotMapped]
        public string? RtaAddress { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
    }
}
