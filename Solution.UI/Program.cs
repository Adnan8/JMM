using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.DAL.Data;
using Solution.DAL.Models;
using Solution.Business.Utility;
using Solution.UI.Controllers;
using Solution.UI.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Configuration for connection string
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

// Register custom services and filters
builder.Services.RegisterCustomServices();
builder.Services.AddScoped<AsyncSessionActionFilter>();
builder.Services.AddControllers(config =>
{
    config.Filters.Add<AsyncSessionActionFilter>();
});

// HttpContextAccessor needed for accessing current HTTP context
builder.Services.AddHttpContextAccessor();

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Singleton service registration
builder.Services.AddSingleton<IPermissionService, PermissionService>();

// MVC Controllers with views and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Tenant middleware
app.UseMiddleware<TenantMiddleware>();

// Root URL redirection middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/AccountUI/Index");
    }
    else
    {
        await next();
    }
});

// Endpoint routing
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "tenant",
        pattern: "{tenantId}/{controller=AccountUI}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=AccountUI}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();
