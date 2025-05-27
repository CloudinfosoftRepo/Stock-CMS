using System.Text.RegularExpressions;
using AutoMapper.Features;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
		private readonly IUserRepository _userRepository;
        private readonly IStockRepository _stockRepository;

		public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository,IStockRepository stockRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _stockRepository = stockRepository;
        }

        public async Task<long> AddCustomer(CustomerDto data)
        {
            string financialYear = GetFinancialYear(DateTime.Now);

            data.FinancialYear = financialYear;
            var isExist = await _customerRepository.GetCustomerByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                List<CustomerDto> dataList = new List<CustomerDto> { data };
                var result = await _customerRepository.AddCustomer(dataList);
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
        public async Task<Int32> UpdateCustomer(CustomerDto data)
        {
            var isExist = await _customerRepository.GetCustomerById(data.Id);
            var chk = await _customerRepository.GetCustomerByName(data.CustomerName);
            bool isMatch = chk.Any(x => x.CustomerName.ToLower() == data.CustomerName.ToLower() && x.Id != data.Id);
            if (isMatch)
            {
                return -1;
            }

            if (isExist.Id > 0)
            {

                data.FinancialYear = isExist.FinancialYear;
                data.CreatedBy = isExist.CreatedBy;
                    data.CreatedAt = isExist.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;
                data.IsActive = isExist.IsActive;
               

                List<CustomerDto> updateList = new List<CustomerDto> { data };
                var result = await _customerRepository.UpdateCustomer(updateList);
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
        public async Task<IEnumerable<CustomerDto>> GetCustomer()
        {
			var data = await _customerRepository.GetCustomer();

            var client = data.OrderBy(x => x.FileNo).ToList();

			var ids = client.Select(x => x.CreatedBy).Concat(client.Select(x => x.UpdatedBy)).Distinct().ToArray();
			var users = await _userRepository.GetUsersByIds(ids);
			var result = client.Select(x =>
			{
				x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
				x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
				return x;
			});

			return result;
		}

        public async Task<IEnumerable<CustomerDto>> GetCustomersById(long id)
        {
            var data = await _customerRepository.GetCustomersById(id);

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


        public async Task<long> UpdateCustomerbyColumn(CustomerDto customer)
        {
            customer.IsClient = true;
            customer.FileNo = customer.FileNo == null ? await GenerateFileNoAsync() : customer.FileNo;

            List<CustomerDto> customerDtos = new List<CustomerDto>() { customer };
			var result = await _customerRepository.UpdateCustomerbyColumn(customerDtos, ["IsClient", "UpdatedAt", "UpdatedBy", "FileNo"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<long> DeleteCustomerbyColumn(CustomerDto customer)
        {
            customer.IsActive = false;
            List<CustomerDto> customerDtos = new List<CustomerDto>() { customer };
            var result = await _customerRepository.UpdateCustomerbyColumn(customerDtos, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                var clientId = result.FirstOrDefault().Id;
                var stock = await _stockRepository.GetStockByClientId(clientId);
                foreach (var item in stock)
                {
                    item.IsActive = false;  
                }
                var updatestock = await _stockRepository.UpdateStock(stock);
                if (updatestock.Any())
                {
                    return clientId; 
                }
                else
                { 
                    return 0; 
                } 
            }
            else
            {
                return 0;
            }
        }

        public async Task<long> GetEnqiryCustomersCount()
        {
            var data = await _customerRepository.GetEnquiryCustomer();

            var result = data.Count();

            return result;
        }

        // added function
        private string GetFinancialYear(DateTime date)
        {
            int startYear = date.Month >= 4 ? date.Year : date.Year - 1;
            int endYear = startYear + 1;
            return $"{startYear}-{endYear.ToString().Substring(2, 2)}"; // Format as YYYY-YY (e.g., 2024-25)
        }

        private async Task<string> GenerateFileNoAsync()
        {
            var data = await _customerRepository.GetCustomer();
            var validFileNos = data
         .Where(x => !string.IsNullOrEmpty(x.FileNo) && Regex.IsMatch(x.FileNo, @"^#File\d{4}$"))
         .OrderByDescending(x => x.FileNo)
         .ToList();

            var lastFileNo = validFileNos.FirstOrDefault();

            if (lastFileNo.FileNo == null)
            {
                return $"#File0001";
            }

                var sequencePart = lastFileNo.FileNo.Substring(lastFileNo.FileNo.Length - 4);
                if (int.TryParse(sequencePart, out int sequenceNumber))
                {
                    sequenceNumber++;

                    return $"#File{sequenceNumber:D4}";
                }

            return $"#File0001";
        }

    }
}
