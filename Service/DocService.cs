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
    public class DocService : IDocService
	{
        private readonly IDocRepository _DocRepository;
		private readonly IUserRepository _userRepository;
        private readonly FileUpload _fileUpload;
        private readonly NormalizeModel _normalizeModel;

        public DocService(IDocRepository DocRepository, IUserRepository userRepository, FileUpload fileUpload, NormalizeModel normalizeModel)
        {
            _DocRepository = DocRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
            _normalizeModel = normalizeModel;
        }

        public async Task<long> AddDoc(DocDto data)
        {
            data = _normalizeModel.FilterDoc(data);

            var isExist = await _DocRepository.GetDocByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
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
        public async Task<IEnumerable<DocDto>> GetDocByClientId(long id)
		{
			var data = await _DocRepository.GetdocByClientId(id);

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

        public async Task<DocDto> GetDocById(long id)
        {
            var data = await _DocRepository.GetDocById(id);

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

    }
}
