using AutoMapper.Features;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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
            return await _customerRepository.GetCustomer();
        }

    }
}
