namespace Stock_CMS.Models
{
    public class RoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
