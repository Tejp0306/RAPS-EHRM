using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EHRM.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddDbContext<EhrmContext>
//    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("EHRMConnection")));
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to login page if not authenticated
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if access is denied
    });

builder.Services.AddAuthorization(options =>
{
    // Configure other authorization policies if necessary
});

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Essential for GDPR compliance
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Enable session before UseAutho

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
