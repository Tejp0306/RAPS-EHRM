using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EHRM.Infrastructure.Configurations;
using Microsoft.Extensions.FileProviders;
using EHRM.ServiceLayer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EHRM.Infrastructure.Middleware;
using EHRM.Infrastructure.Extension;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddServices(builder.Configuration);
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// To serve files from a specific folder (e.g., Files), you can do this:
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files")),
    RequestPath = "/Files"
});
app.UseRouting();

app.UseSession(); // Enable session before UseAutho
app.UseMiddleware<SessionExpirationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// Define routes properly:
app.UseEndpoints(endpoints =>
{
    // Default route — Login Page
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    // Additional routes (custom logic)
    endpoints.MapControllerRoute(
        name: "calendar-events",
        pattern: "Calendar/GetEvents",
        defaults: new { controller = "Calendar", action = "GetEvents" });

    endpoints.MapControllerRoute(
        name: "punch-in",
        pattern: "Dashboard/SavePunchInAsync",
        defaults: new { controller = "Dashboard", action = "SavePunchInAsync" });

    endpoints.MapControllerRoute(
        name: "punch-out",
        pattern: "Dashboard/UpdatePunchOutAsync",
        defaults: new { controller = "Dashboard", action = "UpdatePunchOutAsync" });
});

app.Run();

//using EHRM.DAL.Database;
//using EHRM.DAL.UnitOfWork;
//using EHRM.ServiceLayer.Master;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.EntityFrameworkCore;
//using EHRM.Infrastructure.Configurations;
//using Microsoft.Extensions.FileProviders;
//using EHRM.ServiceLayer.Helpers;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using EHRM.Infrastructure.Middleware;
//using EHRM.Infrastructure.Extension;
//using Logger;
//using Microsoft.AspNetCore.Builder;
//using Finbuckle.MultiTenant; // <-- Make sure you have this using directive

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddServices(builder.Configuration);

//// === MULTI-TENANCY SETUP ===
//builder.Services.AddMultiTenant<TenantInfo>()
//    .WithInMemoryStore(static options =>
//    {
//        options.Tenants.Add(new TenantInfo
//        {
//            Id = "company1",
//            Identifier = "company1",
//            Name = "Company One",
//            ConnectionString = "Server=.;Database=HRMS_Company1;Trusted_Connection=True;"
//        });

//        options.Tenants.Add(new TenantInfo
//        {
//            Id = "company2",
//            Identifier = "company2",
//            Name = "Company Two",
//            ConnectionString = "Server=.;Database=HRMS_Company2;Trusted_Connection=True;"
//        });
//    })
//    .WithRouteStrategy(); // You can also use WithHostStrategy(), etc.

//var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILoggerManager>();
//app.ConfigureExceptionHandler(logger);

//// Middleware and HTTP request pipeline
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//// Serve from /Files path
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files")),
//    RequestPath = "/Files"
//});

//app.UseRouting();


//app.UseMultiTenant();

//app.UseSession();
//app.UseMiddleware<SessionExpirationMiddleware>();

//app.UseAuthentication();
//app.UseAuthorization();

//// Define custom and default routes
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Account}/{action=Login}/{id?}");

//    endpoints.MapControllerRoute(
//        name: "calendar-events",
//        pattern: "Calendar/GetEvents",
//        defaults: new { controller = "Calendar", action = "GetEvents" });

//    endpoints.MapControllerRoute(
//        name: "punch-in",
//        pattern: "Dashboard/SavePunchInAsync",
//        defaults: new { controller = "Dashboard", action = "SavePunchInAsync" });

//    endpoints.MapControllerRoute(
//        name: "punch-out",
//        pattern: "Dashboard/UpdatePunchOutAsync",
//        defaults: new { controller = "Dashboard", action = "UpdatePunchOutAsync" });
//});

//app.Run();
