using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomer();
        Task<long> AddCustomer(CustomerDto data);
        Task<Int32> UpdateCustomer(CustomerDto data);


    }
}
