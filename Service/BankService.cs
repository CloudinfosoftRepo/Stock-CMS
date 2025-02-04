using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUserRepository _userRepository;

        public BankService(IBankRepository bankRepository, IUserRepository userRepository)
        {
            _bankRepository = bankRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<BankDto>> GetBankByClientId(long clientid)
        {
            var data = await _bankRepository.GetBankByClientId(clientid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });
            return result;
        }

        public async Task<IEnumerable<BankDto>> GetBankByLegalHeirId(long clientid)
        {
            var data = await _bankRepository.GetBankByLegalHeirId(clientid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });
            return result;
        }

        public async Task<long> AddBank(BankDto data)
        {
            var isExist = await _bankRepository.GetBankByClientId(data.ClientId ?? 0);
            if (isExist.Any()) { return -1; }
            else
            {
                List<BankDto> dataList = new List<BankDto> { data };
                var result = await _bankRepository.AddBank(dataList);
                if (result.Any())
                {
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public async Task<Int32> UpdateBank(BankDto data)
        {
            var isExist = await _bankRepository.GetBankByClientId(data.ClientId ?? 0);
            //var chk = await _StockRepository.GetStockByName(data.StockName);
            //bool isMatch = chk.Any(x => x.StockName.ToLower() == data.StockName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Any())
            {
                var dataFirst = isExist.FirstOrDefault();
                data.CreatedBy = dataFirst.CreatedBy;
                data.CreatedAt = dataFirst.CreatedAt;
                data.UpdatedBy = data.UpdatedBy;
                data.UpdatedAt = DateTime.Now;
                data.IsActive = dataFirst.IsActive;

                List<BankDto> updateList = new List<BankDto> { data };
                var result = await _bankRepository.UpdateBank(updateList);
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

        public async Task<Int32> UpdateBank1(BankDto data)
        {
            var isExist = await _bankRepository.GetBankByLegalHeirId(data.LegalHeirId ?? 0);
            //var chk = await _StockRepository.GetStockByName(data.StockName);
            //bool isMatch = chk.Any(x => x.StockName.ToLower() == data.StockName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Any())
            {
                var dataFirst = isExist.FirstOrDefault();
                data.CreatedBy = dataFirst.CreatedBy;
                data.CreatedAt = dataFirst.CreatedAt;
                data.UpdatedBy = data.UpdatedBy;
                data.UpdatedAt = DateTime.Now;
                data.IsActive = dataFirst.IsActive;

                List<BankDto> updateList = new List<BankDto> { data };
                var result = await _bankRepository.UpdateBank(updateList);
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
    }
}
