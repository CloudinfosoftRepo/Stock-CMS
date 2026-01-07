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
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName??"NA"))
				.ForMember(dest => dest.FirstHolderName, opt => opt.MapFrom(src => src.FirstHolder.Name??"NA"))
				.ForMember(dest => dest.SecondHolderName, opt => opt.MapFrom(src => src.SecondHolder.Name))
				.ForMember(dest => dest.ThirdHolderName, opt => opt.MapFrom(src => src.ThirdHolder.Name))
				.ReverseMap();

			CreateMap<TblDoc, DocDto>().ReverseMap();
            CreateMap<TblGenratedForm, GenratedFormDto>()
    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Stock.Company.CompanyName))
    .ForMember(dest => dest.FolioNo, opt => opt.MapFrom(src => src.Stock.FolioNo))
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Stock.Customer.CustomerName)).ReverseMap();

            CreateMap<TblForm, FormDto>().ReverseMap();
			CreateMap<TblBank, BankDto>().ReverseMap();
			CreateMap<TblLegalHeir, LegalHeirDto>().ReverseMap();
            CreateMap<TblHolderDoc, HolderDocsDto>().ReverseMap();
            CreateMap<TblRtaCompany, RtaDto>().ReverseMap();
            CreateMap<TblCompany, CompanyDto>()
                .ForMember(dest => dest.RtaName, opt => opt.MapFrom(src => src.Rta.RtaName))
                .ForMember(dest => dest.RtaAddress, opt => opt.MapFrom(src => src.Rta.RtaAddress))
                .ReverseMap();
            CreateMap<TblTracking, TrackingDto>()
     .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Stock.Company.CompanyName))
     .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Stock.Customer.CustomerName))
     .ForMember(dest => dest.FolioNo, opt => opt.MapFrom(src => src.Stock.FolioNo))
     .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Stock.Qty))
    .ReverseMap();

            CreateMap<TblMenu, MenuDto>().ReverseMap();
            CreateMap<TblPermission, PermissionDto>().ReverseMap();

            CreateMap<TblRelationMapping, RelationMappingDto>().ReverseMap();

            CreateMap<TblNominee, NomineeDto>().ReverseMap();


        }
    }
}
