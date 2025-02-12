using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class LegalHeirDto
    {
        public long Id { get; set; }

        public long? DocId { get; set; }

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

        public string? Email { get; set; }

        public bool? IsClaiment { get; set; }

        public bool? IsNoc { get; set; }

        public string? RelationWithDead { get; set; }

        public bool? IsDead { get; set; }

        public string? DeathCertiUrl { get; set; }

        public string? NameAsPerDeathCerti { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public string? PlaceOfDeath { get; set; }

        [NotMapped]
        public IFormFile? PanFile { get; set; }
        [NotMapped]
        public IFormFile? AadharFile { get; set; }
        [NotMapped]
        public IFormFile? DeathcertiFile { get; set; }
        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }

    }
}
