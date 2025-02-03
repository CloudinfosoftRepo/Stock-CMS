using AutoMapper.Features;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class RtaService : IRtaService
    {
        private readonly IRtaRepository _rtaRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;

        public RtaService(IRtaRepository RtaRepository, IUserRepository userRepository, FileUpload fileUpload)
        {
            _rtaRepository = RtaRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
        }

        public async Task<int> AddRta(RtaDto data)
        {
            var isExist = await _rtaRepository.GetRtaByName(data.RtaName);
            if (isExist.Any()) { return -1; }
            else
            {
                List<RtaDto> dataList = new List<RtaDto> { data };

                var result = await _rtaRepository.AddRta(dataList);
                if (result.Any())
                {
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public async Task<Int32> UpdateRta(RtaDto data)
        {
            var isExist = await _rtaRepository.GetRtaById(data.Id);
            var chk = await _rtaRepository.GetRtaByName(data.RtaName);
            bool isMatch = chk.Any(x => x.RtaName.ToLower() == data.RtaName.ToLower() && x.Id != data.Id);
            if (isMatch)
            {
                return -1;
            }

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
                }

                List<RtaDto> updateList = new List<RtaDto> { data };
                var result = await _rtaRepository.UpdateRta(updateList);
                if (result.Any())
                {
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
        public async Task<IEnumerable<RtaDto>> GetRta()
        {
            var data = await _rtaRepository.GetRta();

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


    }
}
