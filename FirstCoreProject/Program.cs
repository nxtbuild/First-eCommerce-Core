using FirstCoreProject.DAL;
using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.DAL.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FirstCoreProject.Models;
using Microsoft.Extensions.DependencyInjection;
using FirstCoreProject.CommonHelper;
using Microsoft.AspNetCore.Identity.UI.Services;
using FirstCoreProject.DAL.DbInitalizer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitalizer, DbInitalizer>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

// Payment Gatway Configure

//builder.Services.Configure<PaymentGatway>(builder.Configuration.GetSection("PaymentSettings"));


builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Bina Login Access Nhi hoga Action .. Redirect Kr dega

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
});

builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

// Set Session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

// For Stripe Gatway

//StripeConfigration.ApiKey = builder.Configuration.GetSection("PaymentSettings:SecretKey").Get<String>();





app.UseAuthentication();;

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();     // Identity Ko Rendar Krna Ke Liye Reg/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void datasedding()
{
    using(var scope=app.Services.CreateScope())
    {
        var DbInitalizer = scope.ServiceProvider.GetRequiredService<IDbInitalizer>();
        DbInitalizer.Initalizer();
    }
}

