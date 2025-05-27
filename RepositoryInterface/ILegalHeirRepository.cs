using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface ILegalHeirRepository
    {
        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByInfo(LegalHeirDto data);

        Task<IEnumerable<LegalHeirDto>> AddLegalHeir(IEnumerable<LegalHeirDto> data);

        Task<IEnumerable<LegalHeirDto>> UpdateLegalHeir(IEnumerable<LegalHeirDto> data);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirById(long Id);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByIds(long?[] ids);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByAadhar(string aadhar);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByClientId(long Id);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByCustomerId(long Id);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByCustomerIdWithoutLegalHeir(long Id, long legalheirId);

        Task<IEnumerable<LegalHeirDto>> GetClaimentLegalHeirByClientId(long Id);

        Task<IEnumerable<LegalHeirDto>> GetClaimentLegalHeirByLegalHeirIds(long?[] Id);

        Task<IEnumerable<LegalHeirDto>> GetNOCLegalHeirByLegalHeirIds(long?[] Id);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByLegalHeirIds(long?[] Id);

        Task<IEnumerable<LegalHeirDto>> UpdateLegalHeirbyColumn(IEnumerable<LegalHeirDto> data, string[] columns);
    }
}
