﻿using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface ICustomerRepository
    {

        Task<IEnumerable<CustomerDto>> GetCustomerById(long Id);
        Task<IEnumerable<CustomerDto>> GetCustomerByName(string customerName);
        Task<IEnumerable<CustomerDto>> GetCustomerByInfo(CustomerDto data);
        Task<IEnumerable<CustomerDto>> GetCustomer();
        Task<IEnumerable<CustomerDto>> AddCustomer(IEnumerable<CustomerDto> data);
        Task<IEnumerable<CustomerDto>> UpdateCustomer(IEnumerable<CustomerDto> data);



    }
}