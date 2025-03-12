using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IStockRepository
	{

        Task<StockDto> GetStockById(long Id);
        Task<IEnumerable<StockDto>> GetStock();
        Task<IEnumerable<StockDto>> AddStock(IEnumerable<StockDto> data);
        Task<IEnumerable<StockDto>> UpdateStock(IEnumerable<StockDto> data);
        Task<IEnumerable<StockDto>> GetStockByClientId(long Id);

        Task<IEnumerable<StockDto>> GetStockByinfo(long Id, long componyId, string folioNo);

        Task<IEnumerable<StockDto>> GetStocksByStatus(string status);


    }
}
