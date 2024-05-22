using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Solution.DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Configuration;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Solution.DAL.Models.Base;
using Microsoft.EntityFrameworkCore.Storage;
using System.IdentityModel.Tokens.Jwt;
using Solution.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Solution.DAL.Data;

public partial class AppDbContext : IdentityDbContext<ApplicationUser>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IServiceProvider serviceProvider)
		: base(options)
	{
        _httpContextAccessor = httpContextAccessor;
		Configuration = configuration;
        _serviceProvider = serviceProvider;
    }

	public IConfiguration Configuration { get; }

	public virtual DbSet<ThemeDetail> ThemeDetails { get; set; }
	public virtual DbSet<DDLDtls> Ddldtls { get; set; }
	public virtual DbSet<Ddlhdr> Ddlhdrs { get; set; }
	public virtual DbSet<Company> Companies { get; set; }
	public virtual DbSet<Student> Student { get; set; }
	public virtual DbSet<AcademicYear> AcademicYears { get; set; }
	public DbSet<ExceptionLog> ExceptionLogs { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<RoleChildFrontEndPermission> RoleChildFrontEndPermissions { get; set; }
    public virtual DbSet<RoleFrontEndPermission> RoleFrontEndPermissions { get; set; }
    public virtual DbSet<FrontendPermission> FrontendPermissions { get; set; }
    public DbSet<RoleMenu> RoleMenu { get; set; }
    public DbSet<Class> Class { get; set; }
    public DbSet<StudentClass> StudentClass { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Menu> Menus { get; set; }
	public DbSet<LoginHistory> LoginHistories { get; set; }
    public DbSet<Document> Documents { get; set; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")),
            ServiceLifetime.Scoped);  
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        int? tenantId = GetInitialTenantId();  // Handle this method to fetch your initial tenant ID correctly

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetTenantFilter(entityType.ClrType, tenantId));
            }
        }
    }

    private int? GetInitialTenantId()
    {
        // Logic to determine the initial tenant ID
        return null;  // Assuming null or 0 is to be handled specially
    }

    private LambdaExpression GetTenantFilter(Type type, int? tenantId)
    {
        var parameter = Expression.Parameter(type, "e");
        var compIdProperty = Expression.PropertyOrField(parameter, "CompId");
        Expression compIdValue = Expression.Constant(tenantId?.ToString(), typeof(string)); // Convert tenantId to string

        Expression equalExpression;
        if (tenantId == null || tenantId == 0)
        {
            equalExpression = Expression.Constant(true);  
        }
        else
        {
            equalExpression = Expression.Equal(compIdProperty, compIdValue);
        }

        return Expression.Lambda(equalExpression, parameter);
    }


    private async Task<int?> GetCurrentTenantId()
    {
        var userManager = _serviceProvider.GetService<UserManager<ApplicationUser>>();
        var userClaims = _httpContextAccessor.HttpContext?.User;
        var userId = userClaims?.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);
        if (user == null || !user.CompId.HasValue || user.CompId == 0)
        {
            // Handle case where CompId is null or zero
            return null;
        }
        return user.CompId;
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await UpdateAuditFields();  
        return await base.SaveChangesAsync(cancellationToken);
    }


    private async Task UpdateAuditFields()
    {
		try
		{

            var entries = ChangeTracker.Entries<BaseModel>();
            var userClamis = _httpContextAccessor.HttpContext?.User;
            var userId = userClamis.FindFirstValue(ClaimTypes.NameIdentifier);
            var companyId = await GetCurrentTenantId();

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseModel entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = userId;
                        entity.CreatedOn = DateTime.Now;
                        entity.UpdatedBy = userId;
                        entity.UpdatedOn = DateTime.Now;
                        entity.CompId = companyId.ToString();
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                        entry.Property(nameof(BaseModel.CreatedOn)).IsModified = false;
                        entity.UpdatedBy = userId;
                        entity.UpdatedOn = DateTime.Now;
                    }
                }
            }
        }
		catch (Exception)
		{

			throw;
		}
       
    }


}
