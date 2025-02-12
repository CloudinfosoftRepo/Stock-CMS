using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IBankService
    {
        Task<IEnumerable<BankDto>> GetBankByClientId(long clientid);
        Task<IEnumerable<BankDto>> GetBankByLegalHeirId(long clientid);

        Task<long> AddBank(BankDto data);

        Task<long> AddBank1(BankDto data);

        Task<Int32> UpdateBank(BankDto data);

        Task<Int32> UpdateBank1(BankDto data);
    }
}
