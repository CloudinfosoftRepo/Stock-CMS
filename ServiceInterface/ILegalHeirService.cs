using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface ILegalHeirService
    {
        Task<long> AddLegalHeir(LegalHeirDto data);

        Task<Int32> UpdateLegalHeir(LegalHeirDto data);

        Task<IEnumerable<LegalHeirDto>> GetLegalHeirByClientId(long id);

        Task<IEnumerable<LegalHeirDto>> GetClaimentLegalHeirByClientId(long id);

        Task<LegalHeirDto> GetLegalHeirById(long id);

        Task<long> UpdateLegalHeirbyColumn(LegalHeirDto data);
    }
}
