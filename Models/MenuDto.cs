using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class MenuDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public int? Sequence { get; set; }

        public int? ParentId { get; set; }

        public string? Actions { get; set; }

        public string? Icon { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        [NotMapped]
        public List<ActionItem> ActionList { get; set; }

        [NotMapped]
        public string ParentName { get; set; }
    }
}
