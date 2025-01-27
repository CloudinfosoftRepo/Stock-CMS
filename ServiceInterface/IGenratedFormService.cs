using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IGenratedFormService
    {
        Task<long> GenrateForm(GenratedFormDto data);
        Task<Int32> UpdateGenratedForm(GenratedFormDto data);

        Task<long> UpdateFormbyColumn(GenratedFormDto data);
        Task<GenratedFormDto> GetGenratedFormById(long id);

        Task<IEnumerable<GenratedFormDto>> GetGenratedFormByStockId(long id);

    }
}
