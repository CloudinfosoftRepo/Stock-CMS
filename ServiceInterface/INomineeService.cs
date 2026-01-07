using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface INomineeService
    {
        Task<long> AddNominee(NomineeDto data);
        Task<Int32> UpdateNominee(NomineeDto data);
        Task<IEnumerable<NomineeDto>> GetNomineeByCustomerId(long id);
        Task<IEnumerable<NomineeDto>> GetNomineeByClientId(long id);
        Task<IEnumerable<NomineeDto>> GetNomineeByLegalHeirId(long id);
        Task<NomineeDto> GetNomineeById(long id);
        Task<long> UpdateNomineebyColumn(NomineeDto data);

    }
}
