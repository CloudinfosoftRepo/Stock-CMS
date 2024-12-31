using Microsoft.EntityFrameworkCore;
using AutoMapper;
using log4net.Config;
using Stock_CMS.AutoMapper;
using Stock_CMS.Entity;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;
using Stock_CMS.Data;


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
        services.AddDbContext<StockCmsContext>(options =>
        {
            options.UseSqlServer("Name=ConnectionStrings:DBConnection");
        });

        // Add AutoMapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new stockMapper());
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

    }
}
