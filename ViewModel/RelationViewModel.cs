using Stock_CMS.Models;

namespace Stock_CMS.ViewModel
{
    public class RelationViewModel
    {
        public string holdertype { get; set; }

        public IEnumerable<RelationMappingDto> relationdata { get; set; } 
    }
}
