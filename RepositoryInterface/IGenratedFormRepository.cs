using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IGenratedFormRepository
	{

        Task<IEnumerable<GenratedFormDto>> GetGenratedFormByInfo(GenratedFormDto data);
        Task<GenratedFormDto> GetGenratedFormById(long Id);
        Task<IEnumerable<GenratedFormDto>> GetGenratedForm();
        Task<IEnumerable<GenratedFormDto>> GenrateForm(IEnumerable<GenratedFormDto> data);
        Task<IEnumerable<GenratedFormDto>> UpdateGenratedForm(IEnumerable<GenratedFormDto> data);

        Task<IEnumerable<GenratedFormDto>> UpdateFormbyColumn(IEnumerable<GenratedFormDto> data, string[] columns);
        Task<IEnumerable<GenratedFormDto>> GetGenratedFormByStockId(long Id);
    }
}
