using Stock_CMS.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class HolderDocsDto
    {
        public long Id { get; set; }

        public string? DocumentName { get; set; }

        public string? DocUrl { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public long? HolderId { get; set; }

        public long? LegalHeirId { get; set; }

        [NotMapped]
        public IFormFile? DocFile { get; set; }
        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
    }
}
