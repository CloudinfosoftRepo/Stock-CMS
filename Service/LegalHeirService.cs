using Stock_CMS.Common;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class LegalHeirService : ILegalHeirService
    {
        private readonly ILegalHeirRepository _legalHeirRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;


        public LegalHeirService(ILegalHeirRepository legalHeirRepository, IUserRepository userRepository, FileUpload fileUpload)
        {
            _legalHeirRepository = legalHeirRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
        }

        public async Task<long> AddLegalHeir(LegalHeirDto data)
        {
            var isExist = await _legalHeirRepository.GetLegalHeirByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                List<LegalHeirDto> dataList = new List<LegalHeirDto> { data };


                if (data != null)
                {

                    if (data.AadharFile != null)
                    {
                        var aadharUpload = _fileUpload.StoreFile("ClientAadhar", data.AadharFile,"Aadhar");
                        if (aadharUpload.status == true)
                        {
                            data.AadharUrl = aadharUpload.message;
                        }
                    }
                    if (data.PanFile != null)
                    {
                        var panUpload = _fileUpload.StoreFile("ClientPan", data.PanFile,"Pan");
                        if (panUpload.status == true)
                        {
                            data.Panurl = panUpload.message;
                        }
                    }
                    if (data.DeathcertiFile != null)
                    {
                        var DeathcertiUpload = _fileUpload.StoreFile("ClientDeathCerti", data.DeathcertiFile, "Death Certi");
                        if (DeathcertiUpload.status == true)
                        {
                            data.DeathCertiUrl = DeathcertiUpload.message;
                        }
                    }
                }
                var result = await _legalHeirRepository.AddLegalHeir(dataList);
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
        public async Task<Int32> UpdateLegalHeir(LegalHeirDto data)
        {
            var isExist = await _legalHeirRepository.GetLegalHeirById(data.Id);
            var chk = await _legalHeirRepository.GetLegalHeirByAadhar(data.Aadhar);
            bool isMatch = chk.Any(x => x.Aadhar == data.Aadhar && x.Id != data.Id);
            if (isMatch)
            {
                return -1;
            }

            if (isExist.Any())
            {
                var existingProduct = isExist.FirstOrDefault();
                if (existingProduct != null)
                {
                    data.IsActive = existingProduct.IsActive;
                    data.CreatedBy = existingProduct.CreatedBy;
                    data.CreatedAt = existingProduct.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;



                    if (data.AadharFile != null)
                    {
                        var aadharUpload = _fileUpload.StoreFile("ClientAadhar", data.AadharFile,"Aadhar");
                        if (aadharUpload.status == true)
                        {
                            data.AadharUrl = aadharUpload.message;
                        }
                    }
                    else
                    {
                        data.AadharUrl = existingProduct.AadharUrl;
                    }
                    if (data.PanFile != null)
                    {
                        var panUpload = _fileUpload.StoreFile("ClientPan", data.PanFile,"Pan");
                        if (panUpload.status == true)
                        {
                            data.Panurl = panUpload.message;
                        }
                    }
                    else
                    {
                        data.Panurl = existingProduct.Panurl;
                    }
                    if (data.DeathcertiFile != null)
                    {
                        var deathcertiUpload = _fileUpload.StoreFile("ClientDeathCerti", data.DeathcertiFile, "Death Certi");
                        if (deathcertiUpload.status == true)
                        {
                            data.DeathCertiUrl = deathcertiUpload.message;
                        }
                    }
                    else
                    {
                        data.DeathCertiUrl = existingProduct.DeathCertiUrl;
                    }
                }

                List<LegalHeirDto> updateList = new List<LegalHeirDto> { data };
                var result = await _legalHeirRepository.UpdateLegalHeir(updateList);
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
        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByClientId(long id)
        {
            var data = await _legalHeirRepository.GetLegalHeirByClientId(id);

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

        public async Task<LegalHeirDto> GetLegalHeirById(long id)
        {
            var data = await _legalHeirRepository.GetLegalHeirById(id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result.FirstOrDefault();
        }

        public async Task<long> UpdateLegalHeirbyColumn(LegalHeirDto data)
        {
            data.IsActive = false;
            List<LegalHeirDto> dtos = new List<LegalHeirDto>() { data };
            var result = await _legalHeirRepository.UpdateLegalHeirbyColumn(dtos, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<IEnumerable<LegalHeirDto>> GetClaimentLegalHeirByClientId(long id)
        {
            var data = await _legalHeirRepository.GetClaimentLegalHeirByClientId(id);

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
