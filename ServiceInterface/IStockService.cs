using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IStockService
    {
        Task<IEnumerable<StockDto>> GetStock();
        Task<long> AddStock(StockDto data);
        Task<Int32> UpdateStock(StockDto data);


    }
}
