using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
	public interface IRtaRepository
	{
		Task<IEnumerable<RtaDto>> GetRtaById(long Id);
		Task<IEnumerable<RtaDto>> GetRtaByName(string Name);
		Task<IEnumerable<RtaDto>> GetRta();
		Task<IEnumerable<RtaDto>> AddRta(IEnumerable<RtaDto> data);
		Task<IEnumerable<RtaDto>> UpdateRta(IEnumerable<RtaDto> data);

	}
}
