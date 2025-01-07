using AutoMapper.Features;
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

		public DocService(IDocRepository DocRepository, IUserRepository userRepository)
        {
            _DocRepository = DocRepository;
            _userRepository = userRepository;
        }

        public async Task<long> AddDoc(DocDto data)
        {
            var isExist = await _DocRepository.GetDocByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                List<DocDto> dataList = new List<DocDto> { data };
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
            var isExist = await _DocRepository.GetDocById(data.Id);
            var chk = await _DocRepository.GetDocByName(data.Name);
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

        
    }
}
