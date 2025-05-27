using Stock_CMS.Models;

namespace Stock_CMS.ModelView
{
    public class MenuViewModel
    {
        public int? ParentId { get; set; }
        public string? ParentMenuName { get; set; }
        public string? Icon { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}
