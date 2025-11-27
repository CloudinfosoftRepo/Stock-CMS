using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomer();
        Task<IEnumerable<CustomerDto>> GetCustomerByval(string val);
        Task<IEnumerable<CustomerDto>> GetCustomersById(long id);
        Task<long> AddCustomer(CustomerDto data);
        Task<Int32> UpdateCustomer(CustomerDto data);

        Task<long> UpdateCustomerbyColumn(CustomerDto customer);

        Task<long> DeleteCustomerbyColumn(CustomerDto customer);

        Task<long> GetEnqiryCustomersCount();

    }
}
