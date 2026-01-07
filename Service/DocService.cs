using AutoMapper.Features;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using Stock_CMS.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class DocService : IDocService
	{
        private readonly IDocRepository _DocRepository;
		private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;
        private readonly NormalizeModel _normalizeModel;
        private readonly IRelationMappingRepository _relationMappingRepository;
        private readonly ILegalHeirRepository _legalHeirRepository;
        private readonly INomineeRepository _nomineeRepository;

        public DocService(IDocRepository DocRepository, IUserRepository userRepository, FileUpload fileUpload, NormalizeModel normalizeModel,IRelationMappingRepository relationMappingRepository, ILegalHeirRepository legalHeirRepository, INomineeRepository nomineeRepository)
        {
            _DocRepository = DocRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
            _normalizeModel = normalizeModel;
            _relationMappingRepository = relationMappingRepository;
            _legalHeirRepository = legalHeirRepository;
            _nomineeRepository = nomineeRepository;
        }

        public async Task<long> AddDoc(DocDto data)
        {
            data = _normalizeModel.FilterDoc(data);

            var isExist = await _DocRepository.GetDocByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                data.DateOfDeath = _normalizeModel.ConvertToIST(data.DateOfDeath);
                data.Dob = _normalizeModel.ConvertToIST(data.Dob);

                List<DocDto> dataList = new List<DocDto> { data };


                if (data!= null)
                {

					if (data.AadharFile != null)
					{
						var aadharUpload = _fileUpload.StoreFile("ClientAadhar", data.AadharFile,"Adhar");
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
                        var DeathcertiUpload = _fileUpload.StoreFile("ClientDeathCerti", data.DeathcertiFile,"Death Certi");
                        if (DeathcertiUpload.status == true)
                        {
                            data.DeathCertiUrl = DeathcertiUpload.message;
                        }
                    }
                    if (data.VoterFile != null)
                    {
                        var voterUpload = _fileUpload.StoreFile("ClientVoter", data.VoterFile,"Voter");
                        if (voterUpload.status == true)
                        {
                            data.VoterIdUrl = voterUpload.message;
                        }
                    }
                    if (data.PassportFile != null)
                    {
                        var passportUpload = _fileUpload.StoreFile("ClientPassport", data.PassportFile,"Passport");
                        if (passportUpload.status == true)
                        {
                            data.PassportUrl = passportUpload.message;
                        }
                    }
                }
                var result = await _DocRepository.AddDoc(dataList);
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
        public async Task<Int32> UpdateDoc(DocDto data)
        {
            data = _normalizeModel.FilterDoc(data);

            var isExist = await _DocRepository.GetDocById(data.Id);
            var chk = await _DocRepository.GetDocByInfo(data);
            bool isMatch = chk.Any(x => x.Name.ToLower() == data.Name.ToLower() && x.Id != data.Id);
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
                    data.Dob =  _normalizeModel.ConvertToIST(data.Dob);
                    data.DateOfDeath =_normalizeModel.ConvertToIST(data.DateOfDeath);
                    data.WitnessJson = existingProduct.WitnessJson;

                    if (data.AadharFile != null)
                    {
                        var aadharUpload = _fileUpload.StoreFile("ClientAadhar", data.AadharFile, "Adhar");
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
                    if (data.VoterFile != null)
                    {
                        var voterIdUpload = _fileUpload.StoreFile("ClientVoter", data.VoterFile, "Voter");
                        if (voterIdUpload.status == true)
                        {
                            data.VoterIdUrl = voterIdUpload.message;
                        }
                    }
                    else
                    {
                        data.VoterIdUrl = existingProduct.VoterIdUrl;
                    }
                    if (data.PassportFile != null)
                    {
                        var passportUpload = _fileUpload.StoreFile("ClientPassport", data.PassportFile, "Passport");
                        if (passportUpload.status == true)
                        {
                            data.PassportUrl = passportUpload.message;
                        }
                    }
                    else
                    {
                        data.PassportUrl = existingProduct.PassportUrl;
                    }
                }

				List<DocDto> updateList = new List<DocDto> { data };
                var result = await _DocRepository.UpdateDoc(updateList);
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

        public async Task<long> UpdateDocbyColumn(DocDto data)
        {
            data.IsActive = false;

            List<DocDto> dataList = new List<DocDto>() { data };
            var result = await _DocRepository.UpdateDocByColumnn(dataList, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<IEnumerable<DocDto>> GetDocByClientId(long id)
		{
			var data = await _DocRepository.GetdocByClientId(id);

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

        public async Task<DocDto> GetDocById(long id)
        {
            var data = await _DocRepository.GetDocById(id);

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

        public async Task<Int32> UpdateWitnessJson(long id, string jsonString, int updatedBy)
        {
            var isExist = await _DocRepository.GetOneDocById(id);
            var data = isExist;

            if (isExist.Id > 0)
            {
                data.WitnessJson = jsonString;
                data.UpdatedBy = updatedBy;
                data.UpdatedAt = DateTime.Now;

                List<DocDto> updateList = new List<DocDto> { data };
                var result = await _DocRepository.UpdateDoc(updateList);
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

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMapping(long id,string holdertype)
        {
            IEnumerable<RelationMappingDto> data = new List<RelationMappingDto>();
            if( holdertype.ToUpper() == "holder".ToUpper())
            {
                data = await _relationMappingRepository.GetRelationMappingsByHolderId(id);
            }
            else
            {
                data = await _relationMappingRepository.GetRelationMappingsByLegalHeirId(id);
            }

            var legalheirid = data.Select(x => x.LegalHeirId).Distinct().ToArray();
            var legalheir = await _legalHeirRepository.GetLegalHeirByIds(legalheirid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                x.LegalHeirName = legalheir.FirstOrDefault(u => u.Id == x.LegalHeirId)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<RelationMappingDto>> GetNomineeRelationMapping(long id, string holdertype)
        {
            IEnumerable<RelationMappingDto> data = new List<RelationMappingDto>();

            if (holdertype.ToUpper() == "holder".ToUpper())
            {
                data = await _relationMappingRepository.GetRelationMappingsByHolderIdAndNominee(id);
            }
            else
            {
                data = await _relationMappingRepository.GetRelationMappingsByLegalHeirIdAndNominee(id);
            }            
            
            var nomineeid = data.Select(x => x.NomineeId).Distinct().ToArray();
            var nominee = await _nomineeRepository.GetNomineeByIds(nomineeid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                x.NomineeName = nominee.FirstOrDefault(u => u.Id == x.NomineeId)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<RelationMappingDto>> GetNomineeRelationByClientId(long id)
        {
            IEnumerable<RelationMappingDto> data = new List<RelationMappingDto>();
        
                data = await _relationMappingRepository.GetRelationMappingsByHolderIdAndNominee(id);
            
            var nomineeid = data.Select(x => x.NomineeId).Distinct().ToArray();
            var nominee = await _nomineeRepository.GetNomineeByIds(nomineeid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                x.NomineeName = nominee.FirstOrDefault(u => u.Id == x.NomineeId)?.Name;
                return x;
            });

            return result;
        }

        public async Task<long> AddRelationMapping(IEnumerable<RelationMappingDto> data)
        {
            //var isExist = await _StockRepository.GetStockByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{

            var result = await _relationMappingRepository.AddRelationMappings(data);
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

        public async Task<Int32> UpdateRelationMapping(string holdertype,IEnumerable<RelationMappingDto> data)
        {
            IEnumerable<RelationMappingDto> isExist = new List<RelationMappingDto>();
            if (holdertype.ToLower() == "LegalHeir".ToLower())
            {
                var ids = data.Select(x => x.LegalHeirParentId).Distinct().ToArray();
                isExist = await _relationMappingRepository.GetRelationMappingsByLegalHeirIds(ids);
            }
            else if (holdertype.ToLower() == "Nominee".ToLower())
            {
                var ids = data.Select(x => x.NomineeId).Distinct().ToArray();
                isExist = await _relationMappingRepository.GetRelationMappingsByNomineeIds(ids);
            }
            else
            {
                var ids = data.Select(x => x.HolderId).Distinct().ToArray();
                isExist = await _relationMappingRepository.GetRelationMappingsByHolderIds(ids);
            }

            //var chk = await _StockRepository.GetStockByName(data.StockName);
            //bool isMatch = chk.Any(x => x.StockName.ToLower() == data.StockName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            foreach (var item in isExist)
            {
                item.IsActive = item.IsActive;
                item.UpdatedBy = data.FirstOrDefault().UpdatedBy;
                item.UpdatedAt = DateTime.Now;
                item.CreatedBy = item.CreatedBy;
                item.CreatedAt = item.CreatedAt;
            }

            if (isExist.Any())
            {
                var result = await _relationMappingRepository.AddOrUpdateRelationMappings(data);
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

        public async Task<long> UpdateRelationMappingbyColumn(RelationMappingDto data)
        {
            data.IsActive = false;

            List<RelationMappingDto> dataList = new List<RelationMappingDto>() { data };
            var result = await _relationMappingRepository.UpdateeRelationMappingsByColumn(dataList, ["IsActive", "UpdatedAt", "UpdatedBy"]);
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
}
