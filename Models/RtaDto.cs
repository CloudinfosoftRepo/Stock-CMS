using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class RtaDto
    {
        public int Id { get; set; }

        public string? RtaName { get; set; }

        public string? RtaAddress { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
    }
}
