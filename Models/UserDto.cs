using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_CMS.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Password { get; set; }

        public long? ContactNo { get; set; }

        public string? Address { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public string? CurrentOtp { get; set; }

        public bool? IsLock { get; set; }

        public DateTime? ExpiryTime { get; set; }

        public int? FailedAttempts { get; set; }

        [NotMapped]
        public string? CreatedByName { get; set; }
        [NotMapped]
        public string? UpdatedByName { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
    }

    public class SignInRequest
    {
        public string LoginUsername { get; set; } = null!;
        public string Password { get; set; } = null!;

        //public double? Test { get; set; }
        //public float? Test1 { get; set; }
        //public decimal? Test2 { get; set; }
        //public int? Test3{ get; set; }

    }

    public class Menudto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public string ParentMenuName { get; set; }

        public string Icon { get; set; }
    }

    public class MenuViewModel
    {
        public int? ParentId { get; set; }
        public string ParentMenuName { get; set; }
        public string Icon { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}
