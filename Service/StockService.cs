using AutoMapper.Features;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _StockRepository;
        private readonly IUserRepository _userRepository;
       

        public StockService(IStockRepository StockRepository,IUserRepository userRepository)
        {
            _StockRepository = StockRepository;
            _userRepository = userRepository;
        }

        public async Task<long> AddStock(StockDto data)
        {
            //var isExist = await _StockRepository.GetStockByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{
                List<StockDto> dataList = new List<StockDto> { data };
                var result = await _StockRepository.AddStock(dataList);
                if (result.Any())
                {
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            //}
        }
        public async Task<Int32> UpdateStock(StockDto data)
        {
            var isExist = await _StockRepository.GetStockById(data.Id);
            //var chk = await _StockRepository.GetStockByName(data.StockName);
            //bool isMatch = chk.Any(x => x.StockName.ToLower() == data.StockName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Any())
            {
                var existingProduct = isExist.FirstOrDefault();
                if (existingProduct != null)
                {
                    data.CreatedBy = existingProduct.CreatedBy;
                    data.CreatedAt = existingProduct.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;

                }

                List<StockDto> updateList = new List<StockDto> { data };
                var result = await _StockRepository.UpdateStock(updateList);
                if (result.Any())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }
        public async Task<IEnumerable<StockDto>> GetStockByClientId(long  clientid)
        {
            var data = await _StockRepository.GetStockByClientId(clientid);
            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x=> x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
			var result = data.Select(x =>
			{
				x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
				x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
				return x;
			});
			return result;
        }

    }
}
