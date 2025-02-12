using AutoMapper.Features;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _StockRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDocRepository _docRepository;
        private readonly ICompanyRepository _companyRepository;
       
        public StockService(IStockRepository StockRepository,IUserRepository userRepository,ICustomerRepository customerRepository, IDocRepository docRepository, ICompanyRepository companyRepository)
        {
            _StockRepository = StockRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _docRepository = docRepository;
            _companyRepository = companyRepository;
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

            if (isExist.Id > 0)
            {  
                    data.CreatedBy = isExist.CreatedBy;
                    data.CreatedAt = isExist.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;
                data.IsActive = isExist.IsActive;

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

        public async Task<Int32> UpdateStockJson(long id, string jsonString, int updatedBy)
        {
            var isExist = await _StockRepository.GetStockById(id);
            var data = isExist;

            if (isExist.Id > 0)
            {
                data.StockJson = jsonString;
                data.UpdatedBy = updatedBy;
                data.UpdatedAt = DateTime.Now;

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

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
			{
				x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
				x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
				return x;
			});

            //var companyid = result.Select(x => x?.CompanyId).Distinct().ToArray();
            //var company =  await _companyRepository.GetCompanyByIds(companyid);
            //var results = result.Select(x =>
            //{
            //    x.CompanyName1 = company.FirstOrDefault(u => u.Id == x.CompanyId)?.CompanyName;
            //    return x;
            //});

            //var holdersids = results.Select(x => x?.FirstHolderId).Concat(data.Select(x => x?.SecondHolderId)).Concat(data.Select(x => x?.ThirdHolderId)).Distinct().ToArray();
            //var holders = await _docRepository.GetDocByIds(holdersids);
            //var result1 = results.Select(x =>
            //{
            //    x.FirstHolder1 = holders.FirstOrDefault(u => u.Id == x.FirstHolderId)?.Name;
            //    x.SecondHolder1 = holders.FirstOrDefault(u => u.Id == x.SecondHolderId)?.Name;
            //    x.ThirdHolder1 = holders.FirstOrDefault(u => u.Id == x.ThirdHolderId)?.Name;
            //    return x;
            //});

            return result;
        }

		public async Task<StockDto> GetStockById(long id)
		{  
			var result = await _StockRepository.GetStockById(id);
			var customer = await _customerRepository.GetCustomerById(result.CustomerId ?? 0);
            result.Customer = customer;
            var compony = await _companyRepository.GetCompanyById(result.CompanyId ?? 0);
            result.Company = compony.FirstOrDefault();
            var holder1 = await _docRepository.GetDocById(result.FirstHolderId ?? 0);
            result.FirstHolderData = holder1.FirstOrDefault(); 
            var holder2 = await _docRepository.GetDocById(result.SecondHolderId ?? 0);
            result.SecondHolderData = holder2.FirstOrDefault(); 
            var holder3 = await _docRepository.GetDocById(result.ThirdHolderId ?? 0);
            result.ThirdHolderData = holder3.FirstOrDefault();

            return result;
		}


        public async Task<IEnumerable<DocDto>> GetHolderByStockId(long id)
        {
            var stock = await _StockRepository.GetStockById(id);
            long?[] holdersids = { stock.FirstHolderId, stock.SecondHolderId, stock.ThirdHolderId };
            var holders = await _docRepository.GetDocByIds(holdersids);

            return holders;
        }
	}
}
