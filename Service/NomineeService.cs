using Stock_CMS.Common;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class NomineeService : INomineeService
    {
        private readonly INomineeRepository _nomineeRepository;
        private readonly IUserRepository _userRepository;
        private readonly NormalizeModel _normalizeModel;
        private readonly IRelationMappingRepository _relationMappingRepository;

        public NomineeService(INomineeRepository nomineeRepository, IUserRepository userRepository, NormalizeModel normalizeModel, IRelationMappingRepository relationMappingRepository)
        {
            _nomineeRepository = nomineeRepository;
            _userRepository = userRepository;
            _normalizeModel = normalizeModel;
            _relationMappingRepository = relationMappingRepository;
        }

        public async Task<long> AddNominee(NomineeDto data)
        {
            //var isExist = await _nomineeRepository.GetNomineeByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{

            data.Dob = _normalizeModel.ConvertToIST(data.Dob);

            List<NomineeDto> dataList = new List<NomineeDto> { data };

            var result = await _nomineeRepository.AddNominee(dataList);
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
        public async Task<Int32> UpdateNominee(NomineeDto data)
        {
            var isExist = await _nomineeRepository.GetNomineeById(data.Id);
            //var chk = await _nomineeRepository.GetNomineeByAadhar(data.Aadhar);
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
                    data.Dob = _normalizeModel.ConvertToIST(data.Dob);
                }

                List<NomineeDto> updateList = new List<NomineeDto> { data };
                var result = await _nomineeRepository.UpdateNominee(updateList);
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

        public async Task<IEnumerable<NomineeDto>> GetNomineeByCustomerId(long id)
        {
            var data = await _nomineeRepository.GetNomineeByClientId(id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result;
        }

        public async Task<IEnumerable<NomineeDto>> GetNomineeByClientId(long id)
        {
            var relation = await _relationMappingRepository.GetRelationMappingsByHolderIdAndNominee(id);

            var nomineeIds = relation.Select(x => x.NomineeId).Distinct().ToArray();
            var data = await _nomineeRepository.GetNomineeByIds(nomineeIds);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            foreach (var item in result)
            {
                item.RelationWithDead = relation.Where(x => x.NomineeId == item.Id && x.HolderId == id).FirstOrDefault().RelationWithDead;
            }

            return result;
        }
        public async Task<IEnumerable<NomineeDto>> GetNomineeByLegalHeirId(long id)
        {
            var relation = await _relationMappingRepository.GetRelationMappingsByLegalHeirIdAndNominee(id);

            var nomineeIds = relation.Select(x => x.NomineeId).Distinct().ToArray();
            var data = await _nomineeRepository.GetNomineeByIds(nomineeIds);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            foreach (var item in result)
            {
                item.RelationWithDead = relation.Where(x => x.NomineeId == item.Id && x.LegalHeirParentId == id).FirstOrDefault().RelationWithDead;
            }

            return result;
        }

        public async Task<NomineeDto> GetNomineeById(long id)
        {
            var data = await _nomineeRepository.GetNomineeById(id);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
            {
                if (x.Dob.HasValue)
                {
                    x.Dob =
                        DateTime.SpecifyKind(x.Dob.Value.Date, DateTimeKind.Unspecified);
                }
                x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
                x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
                return x;
            });

            return result.FirstOrDefault();
        }

        public async Task<long> UpdateNomineebyColumn(NomineeDto data)
        {
            data.IsActive = false;
            List<NomineeDto> dtos = new List<NomineeDto>() { data };
            var result = await _nomineeRepository.UpdateNomineeByColumnn(dtos, ["IsActive", "UpdatedAt", "UpdatedBy"]);
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
