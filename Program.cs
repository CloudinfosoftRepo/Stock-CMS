using Stock_CMS.ViewModel;

var builder = WebApplication.CreateBuilder(args);

//mail settings
builder.Services.Configure<SmtpSettingsViewModel>(builder.Configuration.GetSection("SmtpSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
