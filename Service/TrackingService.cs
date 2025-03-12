using AutoMapper.Features;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;
        private readonly IStockRepository _stockRepository;

        public TrackingService(ITrackingRepository trackingRepository, IUserRepository userRepository, FileUpload fileUpload, IStockRepository stockRepository)
        {
            _trackingRepository = trackingRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
            _stockRepository = stockRepository;
        }

        public async Task<long> AddTracking(TrackingDto data)
        {
            
            {
                List<TrackingDto> dataList = new List<TrackingDto> { data };


                if (data != null)
                {

                    if (data.SendFile != null)
                    {
                        var sendFileUpload = _fileUpload.StoreFile("ClientSend", data.SendFile, "Send File");
                        if (sendFileUpload.status == true)
                        {
                            data.SendUrl = sendFileUpload.message;
                        }
                    }
                    if (data.ResponseFile != null)
                    {
                        var responseFileUpload = _fileUpload.StoreFile("ClientResponse", data.ResponseFile, "Response File");
                        if (responseFileUpload.status == true)
                        {
                            data.ResponseUrl = responseFileUpload.message;
                        }
                    }
                }
                var result = await _trackingRepository.AddTracking(dataList);
                if (result.Any())
                {
                    var stock = await _stockRepository.GetStockById(result.FirstOrDefault().StockId);
                    stock.ClaimStatus = result.FirstOrDefault().Status;
                    List<StockDto> stockList = new List<StockDto> { stock };
                    var response = await _stockRepository.UpdateStock(stockList);
                    if (response.Any())
                    {
                        return result.FirstOrDefault().Id;
                    }
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public async Task<long> UpdateTracking(TrackingDto data)
        {
            var isExist = await _trackingRepository.GetTrackingById(data.Id);
            //var chk = await _trackingRepository.GetTrackingByName(data.Process);
            //bool isMatch = chk.Any(x => x.Process.ToLower() == data.Process.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Any())
            {
                var existingProduct = isExist.FirstOrDefault();
                if (existingProduct != null)
                {
                    data.CreatedBy = existingProduct.CreatedBy;
                    data.CreatedAt = existingProduct.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;
                    data.IsActive = existingProduct.IsActive;

                    if (data != null)
                    {

                        if (data.SendFile != null)
                        {
                            var sendFileUpload = _fileUpload.StoreFile("ClientSend", data.SendFile, "Send File");
                            if (sendFileUpload.status == true)
                            {
                                data.SendUrl = sendFileUpload.message;
                            }
                        }
                        if (data.ResponseFile != null)
                        {
                            var responseFileUpload = _fileUpload.StoreFile("ClientResponse", data.ResponseFile, "Response File");
                            if (responseFileUpload.status == true)
                            {
                                data.ResponseUrl = responseFileUpload.message;
                            }
                        }
                    }
                }

                List<TrackingDto> updateList = new List<TrackingDto> { data };
                var result = await _trackingRepository.UpdateTracking(updateList);
                if (result.Any())
                {
                    var stock = await _stockRepository.GetStockById(result.FirstOrDefault().StockId);
                    stock.ClaimStatus = result.FirstOrDefault().Status;
                    List<StockDto> stockList = new List<StockDto> { stock };
                    var response = await _stockRepository.UpdateStock(stockList);
                    if (response.Any())
                    {
                        return 1;
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }
        public async Task<IEnumerable<TrackingDto>> GetTracking()
        {
            var data = await _trackingRepository.GetTracking();

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<TrackingDto>> GetTrackingbyStockId(long Id)
        {
            var data = await _trackingRepository.GetTrackingbyStockId(Id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });
            return result;
        }

        public async Task<long> UpdateTrackingbyColumn(TrackingDto data)
        {
            data.IsActive = false;
            List<TrackingDto> formDtos = new List<TrackingDto>() { data };
            var result = await _trackingRepository.UpdateFormbyColumn(formDtos, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }

        //public async Task<TrackingDto> GetTrackingById(long id)
        //{
        //    var result = await _trackingRepository.GetTrackingById(id);
        //    //var customer = await _trackingRepository.GetTrackingById(result.CustomerId);
        //    //result.Customer = customer;
        //    return result;
        //}

    }
}
