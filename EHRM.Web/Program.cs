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



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
// Ensure AppUser is initialize
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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Calendar}/{action=GetEvents}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=SavePunchInAsync}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=UpdatePunchOutAsync}");
//app.MapControllerRoute(
//    name: "GetEvents",
//    pattern: "Calendar/GetEvents",
//    defaults: new { controller = "Calendar", action = "GetEvents" }
//);


app.Run();