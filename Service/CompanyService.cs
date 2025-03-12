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
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRtaRepository _rtaRepository;
        private readonly FileUpload _fileUpload;

        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository, IRtaRepository rtaRepository, FileUpload fileUpload)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _rtaRepository = rtaRepository;
            _fileUpload = fileUpload;
        }

        public async Task<int> AddCompany(CompanyDto data)
        {
            var isExist = await _companyRepository.GetCompanyByName(data.CompanyName);
            if (isExist.Any()) { return -1; }
            else
            {
                List<CompanyDto> dataList = new List<CompanyDto> { data };

                var result = await _companyRepository.AddCompany(dataList);
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
        public async Task<Int32> UpdateCompany(CompanyDto data)
        {
            var isExist = await _companyRepository.GetCompanyById(data.Id);
            var chk = await _companyRepository.GetCompanyByName(data.CompanyName);
            bool isMatch = chk.Any(x => x.CompanyName.ToLower() == data.CompanyName.ToLower() && x.Id != data.Id);
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

                List<CompanyDto> updateList = new List<CompanyDto> { data };
                var result = await _companyRepository.UpdateCompany(updateList);
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
        public async Task<IEnumerable<CompanyDto>> GetCompany()
        {
            var data = await _companyRepository.GetCompany(); 
            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var rta = await _rtaRepository.GetRta();

            var result = from c in data
                         join r in rta on c.Rtaid equals r.Id 
                         join cb in users on c.CreatedBy equals cb.Id into cbGroup
                         from cb in cbGroup.DefaultIfEmpty()
                         join ub in users on c.UpdatedBy equals ub.Id into ubGroup
                         from ub in ubGroup.DefaultIfEmpty()
                         select new CompanyDto
                         {
                             Id = c.Id,
                             CompanyName = c.CompanyName,
                             CompanyPinCode = c.CompanyPinCode,
                             CompanyAddress = c.CompanyAddress,
                             CreatedAt = c.CreatedAt,
                             UpdatedAt = c.UpdatedAt,
                             CreatedByName = cb?.Name,
                             UpdatedByName = ub?.Name,
                             RtaAddress = r.RtaAddress,
                             RtaName = r.RtaName,
                             Rtaid = r.Id,
                             IsActive = c.IsActive,
                         };

            return result;
        }


    }
}
