using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class CustomerRepository : EfRepository<TblCustomer, CustomerDto>, ICustomerRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public CustomerRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetCustomerById(long Id)
        {
            return await GetOne(x => x.Id == Id);
        }
        public async Task<IEnumerable<CustomerDto>> GetCustomerByName(string customerName)
        {
            return await GetMany(x => x.CustomerName.ToLower() == customerName.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<CustomerDto>> GetCustomerByInfo(CustomerDto data)
        {
            return await GetMany(x => x.CustomerName.ToLower() == data.CustomerName.ToLower() &&  x.Mobile.ToLower() == data.Mobile.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<CustomerDto>> GetCustomer()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<CustomerDto>> AddCustomer(IEnumerable<CustomerDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<CustomerDto>> UpdateCustomer(IEnumerable<CustomerDto> data)
        {

            return await UpdateEntities(data);
        }

        public async Task<IEnumerable<CustomerDto>> UpdateCustomerbyColumn(IEnumerable<CustomerDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data , columns);
        }


    }
}
