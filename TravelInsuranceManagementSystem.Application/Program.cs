using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using System.Text;

using System.Security.Claims;

using TravelInsuranceManagementSystem.Application.Data;

using TravelInsuranceManagementSystem.Application.Models;

// Import your new namespaces

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Repo.Implementation;

using TravelInsuranceManagementSystem.Services.Interfaces;

using TravelInsuranceManagementSystem.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// 1) Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Register Repositories (Data Access)

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IAgentRepository, AgentRepository>();

builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IUserDashboardRepository, UserDashboardRepository>();

// 3) Register Services (Business Logic)

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IAgentService, AgentService>();

builder.Services.AddScoped<IClaimService, ClaimService>();

builder.Services.AddScoped<IPolicyService, PolicyService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IUserDashboardService, UserDashboardService>();

// 4) Register Microsoft Identity Password Hasher

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// 5) Session Configuration

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>

{

    options.IdleTimeout = TimeSpan.FromMinutes(60);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

});

// 6) Authentication Configuration (Hybrid JWT & Cookie)

var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>

{

    options.DefaultScheme = "SmartAuth";

    options.DefaultChallengeScheme = "SmartAuth";

})

.AddPolicyScheme("SmartAuth", "JWT or Cookie", options =>

{

    options.ForwardDefaultSelector = context =>

    {

        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))

        {

            return JwtBearerDefaults.AuthenticationScheme;

        }

        return CookieAuthenticationDefaults.AuthenticationScheme;

    };

})

.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>

{

    options.LoginPath = "/Account/SignIn";

    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Cookie.Name = "TravelBuddyAuthCookie";

})

.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>

{

    options.TokenValidationParameters = new TokenValidationParameters

    {

        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = true,

        ValidIssuer = jwtSettings["Issuer"],

        ValidateAudience = true,

        ValidAudience = jwtSettings["Audience"],

        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero,

        RoleClaimType = ClaimTypes.Role,

        NameClaimType = ClaimTypes.Name

    };

});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middlewares

if (!app.Environment.IsDevelopment())

{

    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();

}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
