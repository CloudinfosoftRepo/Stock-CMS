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
    public class HolderDocService : IHolderDocService
    {
        private readonly IHolderDocRepository _holderDocRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;

        public HolderDocService(IHolderDocRepository HolderDocRepository, IUserRepository userRepository, FileUpload fileUpload)
        {
            _holderDocRepository = HolderDocRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
        }

        public async Task<long> AddHolderDoc(HolderDocsDto data)
        {
            var isExist = await _holderDocRepository.GetHolderDocByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                List<HolderDocsDto> dataList = new List<HolderDocsDto> { data };


                if (data != null)
                {

                    if (data.DocFile != null)
                    {
                        var docUpload = _fileUpload.StoreFile("ClientDoc", data.DocFile,data.DocumentName);
                        if (docUpload.status == true)
                        {
                            data.DocUrl = docUpload.message;
                        }
                    }
                }
                var result = await _holderDocRepository.AddHolderDoc(dataList);
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
        public async Task<Int32> UpdateHolderDoc(HolderDocsDto data)
        {
            var isExist = await _holderDocRepository.GetHolderDocById(data.Id);
            var chk = await _holderDocRepository.GetHolderDocByName(data.DocumentName);
            bool isMatch = chk.Any(x => x.DocumentName.ToLower() == data.DocumentName.ToLower() && x.Id != data.Id);
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



                    if (data.DocFile != null)
                    {
                        var docUpload = _fileUpload.StoreFile("ClientDoc", data.DocFile,data.DocumentName);
                        if (docUpload.status == true)
                        {
                            data.DocUrl = docUpload.message;
                        }
                    }
                    else
                    {
                        data.DocUrl = existingProduct.DocUrl;
                    }
                   

                }

                List<HolderDocsDto> updateList = new List<HolderDocsDto> { data };
                var result = await _holderDocRepository.UpdateHolderDoc(updateList);
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
        public async Task<IEnumerable<HolderDocsDto>> GetHolderDocByHolderId(long holderId)
        {
            var data = await _holderDocRepository.GetHolderDocByHolderId(holderId);

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
