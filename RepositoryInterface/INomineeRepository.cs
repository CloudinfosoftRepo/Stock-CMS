using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface INomineeRepository
    {
        Task<IEnumerable<NomineeDto>> GetNomineeById(long Id);
        Task<NomineeDto> GetOneNomineeById(long Id);
        Task<IEnumerable<NomineeDto>> GetNomineeByInfo(NomineeDto data);
        Task<IEnumerable<NomineeDto>> GetNomineeByClientId(long Id);
        Task<IEnumerable<NomineeDto>> GetNomineeByName(string Name);
        Task<IEnumerable<NomineeDto>> GetNominee();
        Task<IEnumerable<NomineeDto>> AddNominee(IEnumerable<NomineeDto> data);
        Task<IEnumerable<NomineeDto>> UpdateNominee(IEnumerable<NomineeDto> data);
        Task<IEnumerable<NomineeDto>> UpdateNomineeByColumnn(IEnumerable<NomineeDto> data, string[] columns);
        Task<IEnumerable<NomineeDto>> GetNomineeByIds(long?[] ids);

    }
}
