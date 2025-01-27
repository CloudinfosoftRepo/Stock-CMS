using AutoMapper.Features;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class GenratedFormService : IGenratedFormService
    {
        private readonly IGenratedFormRepository _GenratedFormRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
       
        public GenratedFormService(IGenratedFormRepository GenratedFormRepository,IUserRepository userRepository,ICustomerRepository customerRepository)
        {
            _GenratedFormRepository = GenratedFormRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        public async Task<long> GenrateForm(GenratedFormDto data)
        {
            //var isExist = await _GenratedFormRepository.GetGenratedFormByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{
                List<GenratedFormDto> dataList = new List<GenratedFormDto> { data };
                var result = await _GenratedFormRepository.GenrateForm(dataList);
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
        public async Task<Int32> UpdateGenratedForm(GenratedFormDto data)
        {
            var isExist = await _GenratedFormRepository.GetGenratedFormById(data.Id);
            //var chk = await _GenratedFormRepository.GetGenratedFormByName(data.GenratedFormName);
            //bool isMatch = chk.Any(x => x.GenratedFormName.ToLower() == data.GenratedFormName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Id > 0)
            {  
                    data.CreatedBy = isExist.CreatedBy;
                    data.CreatedAt = isExist.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;

                List<GenratedFormDto> updateList = new List<GenratedFormDto> { data };
                var result = await _GenratedFormRepository.UpdateGenratedForm(updateList);
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

        public async Task<long> UpdateFormbyColumn(GenratedFormDto data)
        {
            data.IsActive = false;
            List<GenratedFormDto> formDtos = new List<GenratedFormDto>() { data };
            var result = await _GenratedFormRepository.UpdateFormbyColumn(formDtos, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<GenratedFormDto> GetGenratedFormById(long id)
		{  
			var result = await _GenratedFormRepository.GetGenratedFormById(id);
			return result;
		}

        public async Task<IEnumerable<GenratedFormDto>> GetGenratedFormByStockId(long id)
        {
            var result = await _GenratedFormRepository.GetGenratedFormByStockId(id);
            return result;
        }

    }
}
