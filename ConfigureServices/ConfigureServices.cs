using AutoMapper;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Stock_CMS.AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;
using Stock_CMS.ViewModel;
using System.Configuration;


public static class ConfigureServices
{
    public static void AddCustomServices(this IServiceCollection services)
    {

        // Configure log4net

        XmlConfigurator.Configure(new FileInfo("log4net.config"));



        // Add session services
        services.AddSession(options =>
        {
            // Set a short timeout for easy testing.
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Add DbContext
        services.AddDbContext<DmCmsContext>(options =>
        {
            options.UseSqlServer("Name=ConnectionStrings:DBConnection");
        });

        // Add AutoMapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new emMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);


        // Register services
      
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IRoleRepository, RoleRepository>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();

		services.AddTransient<IStockService, StockService>();
		services.AddTransient<IStockRepository, StockRepository>();
        
        services.AddTransient<FileUpload>();
        services.AddTransient<NormalizeModel>();

        services.AddTransient<IDocService, DocService>();
		services.AddTransient<IDocRepository, DocRepository>();

		services.AddTransient<IGenratedFormService, GenratedFormService>();
		services.AddTransient<IGenratedFormRepository, GenratedFormRepository>();

		services.AddTransient<IFormService, FormService>();
		services.AddTransient<IFormRepository, FormRepository>();
        
        services.AddTransient<IBankService, BankService>();
		services.AddTransient<IBankRepository, BankRepository>();

        services.AddTransient<ILegalHeirService, LegalHeirService>();
        services.AddTransient<ILegalHeirRepository, LegalHeirRepository>();

        services.AddTransient<IHolderDocService, HolderDocService>();
        services.AddTransient<IHolderDocRepository, HolderDocRepository>();

        services.AddTransient<IRtaService, RtaService>();
        services.AddTransient<IRtaRepository, RtaRepository>();

        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<ICompanyRepository, CompanyRepository>();

        services.AddTransient<ITrackingService, TrackingService>();
        services.AddTransient<ITrackingRepository, TrackingRepository>();

        services.AddTransient<IMenuRepository, MenuRepository>();
        services.AddTransient<IPermissionService, PermissionService>(); 
        services.AddTransient<IPermissionRepository, PermissionRepository>();

        services.AddTransient<IRelationMappingRepository, RelationMappingRepository>();

        services.AddTransient<IFileService, FileService>();

        services.AddTransient<IOTPService, OTPService>();

        services.AddTransient<INomineeService, NomineeService>();
        services.AddTransient<INomineeRepository, NomineeRepository>();

    }
}
