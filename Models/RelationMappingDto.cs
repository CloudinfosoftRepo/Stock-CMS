using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class RelationMappingDto
    {
        public long Id { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public long? HolderId { get; set; }

        public long? LegalHeirId { get; set; }

        public string? RelationWithDead { get; set; }

        public long? LegalHeirParentId { get; set; }

        public long? NomineeId { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
        [NotMapped]
        public string? HolderName { get; set; }
        [NotMapped]
        public string? LegalHeirName { get; set; }
        [NotMapped]
        public string? LegalHeirParentName { get; set; }
        [NotMapped]
        public string? NomineeName { get; set; }
    }
}
