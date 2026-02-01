using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Repo.Data;

using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Repo.Implementation;

using TravelInsuranceManagementSystem.Services.Interfaces;

using TravelInsuranceManagementSystem.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// 1) Database Configuration (UPDATED)

builder.Services.AddDbContext<ApplicationDbContext>(options =>

    options.UseSqlServer(

        builder.Configuration.GetConnectionString("DefaultConnection"),

        // This line is CRITICAL for your multi-project structure:

        b => b.MigrationsAssembly("TravelInsuranceManagementSystem.Repo")

    ));

// 2) IDENTITY CONFIGURATION

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>

{

    options.Password.RequireDigit = true;

    options.Password.RequiredLength = 8;

    options.Password.RequireNonAlphanumeric = true;

    options.Password.RequireUppercase = true;

    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;

})

.AddEntityFrameworkStores<ApplicationDbContext>()

.AddDefaultTokenProviders();

// 3) Cookie Settings

builder.Services.ConfigureApplicationCookie(options =>

{

    options.LoginPath = "/Account/SignIn";

    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Cookie.Name = "TravelInsuranceAuthCookie";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

});

// 4) Register Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IAgentRepository, AgentRepository>();

builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IUserDashboardRepository, UserDashboardRepository>();

// 5) Register Services

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IAgentService, AgentService>();

builder.Services.AddScoped<IClaimService, ClaimService>();

builder.Services.AddScoped<IPolicyService, PolicyService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IUserDashboardService, UserDashboardService>();

// 6) MVC and Sessions

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>

{

    options.IdleTimeout = TimeSpan.FromMinutes(60);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

});

var app = builder.Build();

// 7) Middlewares

if (!app.Environment.IsDevelopment())

{

    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();

}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Authentication MUST come before Authorization

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
