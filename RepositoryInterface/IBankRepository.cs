using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IBankRepository
    {
        Task<IEnumerable<BankDto>> AddBank(IEnumerable<BankDto> data);

        Task<IEnumerable<BankDto>> UpdateBank(IEnumerable<BankDto> data);

        Task<IEnumerable<BankDto>> UpdateBankbyColumn(IEnumerable<BankDto> data, string[] columns);

        Task<IEnumerable<BankDto>> GetBankByClientId(long Id);

        Task<IEnumerable<BankDto>> GetBankByLegalHeirId(long Id);

        Task<BankDto> GetBankById(long Id);
    }
}
