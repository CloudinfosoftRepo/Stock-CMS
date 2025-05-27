using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class PermissionDto
    {
        public int Id { get; set; }

        public int? MenuId { get; set; }

        public int? RoleId { get; set; }

        public int? UserId { get; set; }

        public string? Actions { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }


        [NotMapped]
        public List<ActionItem> ActionList { get; set; }
    }

    public class ActionItem
    {
        public string Action { get; set; }
        public bool IsEnabled { get; set; }
    }

}
