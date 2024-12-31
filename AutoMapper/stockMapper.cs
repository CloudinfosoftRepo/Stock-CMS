using AutoMapper;
using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.AutoMapper
{
	public class stockMapper : Profile
	{
		public stockMapper()
		{
            CreateMap<TblRole, RoleDto>().ReverseMap();
            CreateMap<TblUser, UserDto>().ReverseMap();
            CreateMap<TblCustomer, CustomerDto>().ReverseMap();

        }
    }
}
