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

		public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<long> AddCustomer(CustomerDto data)
        {
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
            List<CustomerDto> customerDtos = new List<CustomerDto>() { customer };
			var result = await _customerRepository.UpdateCustomerbyColumn(customerDtos, ["IsClient", "UpdatedAt", "UpdatedBy"]);
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
}
