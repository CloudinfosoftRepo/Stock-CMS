using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface ITrackingService
    {
        Task<long> AddTracking(TrackingDto data);
        Task<long> UpdateTracking(TrackingDto data);
        Task<IEnumerable<TrackingDto>> GetTracking();

        Task<IEnumerable<TrackingDto>> GetTrackingbyStockId(long Id);

        Task<long> UpdateTrackingbyColumn(TrackingDto data);
    }
}
