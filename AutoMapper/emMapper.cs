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
				.ForMember(dest => dest.CompanyName1, opt => opt.MapFrom(src => src.Company.CompanyName))
				.ForMember(dest => dest.FirstHolder1, opt => opt.MapFrom(src => src.FirstHolderNavigation.Name))
				.ForMember(dest => dest.SecondHolder1, opt => opt.MapFrom(src => src.SecondHolderNavigation.Name))
				.ForMember(dest => dest.ThirdHolder1, opt => opt.MapFrom(src => src.ThirdHolderNavigation.Name))
				.ReverseMap();

			CreateMap<TblDoc, DocDto>().ReverseMap();
            CreateMap<TblGenratedForm, GenratedFormDto>()
    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Stock.CompanyName))
    .ForMember(dest => dest.FolioNo, opt => opt.MapFrom(src => src.Stock.FolioNo))
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Stock.Customer.CustomerName)).ReverseMap();

            CreateMap<TblForm, FormDto>().ReverseMap();
			CreateMap<TblBank, BankDto>().ReverseMap();
			CreateMap<TblLegalHeir, LegalHeirDto>().ReverseMap();
            CreateMap<TblHolderDoc, HolderDocsDto>().ReverseMap();
            CreateMap<TblRtaCompany, RtaDto>().ReverseMap();
            CreateMap<TblCompany, CompanyDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Rta.RtaName))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.Rta.RtaAddress))
                .ReverseMap();

        }
    }
}
