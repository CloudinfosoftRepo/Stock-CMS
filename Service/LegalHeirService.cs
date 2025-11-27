using Stock_CMS.Common;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class LegalHeirService : ILegalHeirService
    {
        private readonly ILegalHeirRepository _legalHeirRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;
        private readonly NormalizeModel _normalizeModel;
        private readonly IRelationMappingRepository _relationMappingRepository;

        public LegalHeirService(ILegalHeirRepository legalHeirRepository, IUserRepository userRepository, FileUpload fileUpload,NormalizeModel normalizeModel, IRelationMappingRepository relationMappingRepository)
        {
            _legalHeirRepository = legalHeirRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
            _normalizeModel = normalizeModel;
            _relationMappingRepository = relationMappingRepository;
        }

        public async Task<long> AddLegalHeir(LegalHeirDto data)
        {
            //var isExist = await _legalHeirRepository.GetLegalHeirByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{

                data.DateOfDeath = _normalizeModel.ConvertToIST(data.DateOfDeath);
                data.Dob = _normalizeModel.ConvertToIST(data.Dob);

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
            //}
        }
        public async Task<Int32> UpdateLegalHeir(LegalHeirDto data)
        {
            var isExist = await _legalHeirRepository.GetLegalHeirById(data.Id);
            //var chk = await _legalHeirRepository.GetLegalHeirByAadhar(data.Aadhar);
            //bool isMatch = chk.Any(x => x.Name == data.Name && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

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
                    data.Dob = data.Dob != null ? _normalizeModel.ConvertToIST(data.Dob) : existingProduct.Dob;
                    data.DateOfDeath = data.DateOfDeath != null ? _normalizeModel.ConvertToIST(data.DateOfDeath) : existingProduct.DateOfDeath;


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

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByCustomerId(long id)
        {
            var data = await _legalHeirRepository.GetLegalHeirByCustomerId(id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByCustomerIdWithoutLegalHeir(long id, long legalheirId)
        {
            var data = await _legalHeirRepository.GetLegalHeirByCustomerIdWithoutLegalHeir(id,legalheirId);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByClientId(long id)
        { 
            var relation = await _relationMappingRepository.GetRelationMappingsByHolderId(id);

            var legalHeirId = relation.Select(x => x.LegalHeirId).Distinct().ToArray();
        
            var data = await _legalHeirRepository.GetLegalHeirByLegalHeirIds(legalHeirId);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            foreach (var item in result)
            {
                item.RelationWithDead = relation.Where(x => x.LegalHeirId == item.Id && x.HolderId == id).FirstOrDefault().RelationWithDead;
            }

            return result;
        }

        public async Task<LegalHeirDto> GetLegalHeirById(long id)
        {
            var data = await _legalHeirRepository.GetLegalHeirById(id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
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
            var relation = await _relationMappingRepository.GetRelationMappingsByHolderId(id);

            var legalHeirId = relation.Select(x => x.LegalHeirId).Distinct().ToArray();
            var data = await _legalHeirRepository.GetClaimentLegalHeirByLegalHeirIds(legalHeirId);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            foreach (var item in result)
            {
                item.RelationWithDead = relation.Where(x => x.LegalHeirId == item.Id && x.HolderId == id).FirstOrDefault().RelationWithDead;
            }

            return result;
        }

        public async Task<IEnumerable<LegalHeirDto>> GetNOCLegalHeirByLegalHeirIds(long id)
        {
            var relation = await _relationMappingRepository.GetRelationMappingsByHolderId(id);

            var legalHeirId = relation.Select(x => x.LegalHeirId).Distinct().ToArray();
            var data = await _legalHeirRepository.GetNOCLegalHeirByLegalHeirIds(legalHeirId);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                if (x.DateOfDeath.HasValue)
                {
                    x.DateOfDeath =
                        DateTime.SpecifyKind(x.DateOfDeath.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            foreach (var item in result)
            {
                item.RelationWithDead = relation.Where(x => x.LegalHeirId == item.Id && x.HolderId == id).FirstOrDefault().RelationWithDead;
            }

            return result;
        }
    }
}
