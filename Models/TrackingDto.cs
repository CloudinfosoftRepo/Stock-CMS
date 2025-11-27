using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class TrackingDto
    {
        public long Id { get; set; }

        public DateTime? DateofSubmission { get; set; }

        public string? Process { get; set; }

        public string? SendTo { get; set; }

        public string? TrackingId { get; set; }

        public string? Status { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public long StockId { get; set; }

        public string? SendUrl { get; set; }

        public string? ResponseUrl { get; set; }

        public DateTime? DateofFollowUp { get; set; }

        public string? Remark { get; set; }

        public string? Srnno { get; set; }

        public DateTime? Srndate { get; set; }

        public string? DpName { get; set; }

        public string? DpIdClientId { get; set; }

        public string? Iepfstatus { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }

        [NotMapped]
        public string? CompanyName { get; set; }

        [NotMapped]
        public string? CustomerName { get; set; }
        [NotMapped]
        public string? FolioNo { get; set; }

        [NotMapped]
        public double? Qty { get; set; }
        public IFormFile? SendFile { get; set; }
        [NotMapped]
        public IFormFile? ResponseFile { get; set; }
    }
}
