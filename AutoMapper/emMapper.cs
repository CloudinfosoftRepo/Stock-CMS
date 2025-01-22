using AutoMapper;
using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.AutoMapper
{
	public class emMapper : Profile
	{
		public emMapper()
		{
            CreateMap<TblRole, RoleDto>().ReverseMap();
            CreateMap<TblUser, UserDto>().ReverseMap();
            CreateMap<TblCustomer, CustomerDto>().ReverseMap();
			
			CreateMap<TblStock, StockDto>()
			    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CustomerName))
			    .ForMember(dest => dest.IsClient, opt => opt.MapFrom(src => src.Customer.IsClient))
				.ReverseMap();

			CreateMap<TblDoc, DocDto>().ReverseMap();
			CreateMap<TblGenratedForm, GenratedFormDto>().ReverseMap();

		}
    }
}
