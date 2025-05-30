using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IStockService
    {
        //Task<IEnumerable<StockDto>> GetStock();
        Task<long> AddStock(StockDto data);
        Task<Int32> UpdateStock(StockDto data);

        Task<Int32> UpdateStockJson(long id, string jsonString, int updatedBy);

        Task<long> UpdateStockbyColumn(StockDto stock);

        Task<long> DeleteStockbyColumn(StockDto stock);

        Task<StocksDetailsDto> GetStockByClientId(long clientid, string val);
        Task<StockDto> GetStockById(long id);

        Task<IEnumerable<DocDto>> GetHolderByStockId(long id);

        Task<string> UploadStockExcel(UploadStockDto data1, int createdBy);

        Task<Int32> UpdateNomineeJson(long id, string jsonString, int updatedBy);

        Task<dynamic> GetStocksStatusCount();

        Task<dynamic> GetStocksByStatus();

        Task<dynamic> GetStocksByStatusName(string status);

        Task<dynamic> GetUnpaidAmountByClient();
    }
}
