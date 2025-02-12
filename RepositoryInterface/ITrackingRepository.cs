using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
	public interface ITrackingRepository
	{
		Task<IEnumerable<TrackingDto>> GetTrackingById(long Id);
		Task<IEnumerable<TrackingDto>> GetTracking();
		Task<IEnumerable<TrackingDto>> AddTracking(IEnumerable<TrackingDto> data);
		Task<IEnumerable<TrackingDto>> UpdateTracking(IEnumerable<TrackingDto> data);
		Task<IEnumerable<TrackingDto>> GetTrackingbyStockId(long Id);

		Task<IEnumerable<TrackingDto>> UpdateFormbyColumn(IEnumerable<TrackingDto> data, string[] columns);
    }
}
