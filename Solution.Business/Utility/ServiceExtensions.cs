using Microsoft.Extensions.DependencyInjection;
using Solution.Business.Services.IServices;
using Solution.Business.Services;
using Solution.Repository.Repo.IRepo;
using Solution.Repository.Repo;
using Solution.Repository;
using HashidsNet;
using Solution.Business.Mapper;
using Solution.Common;

namespace Solution.Business.Utility
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
        {
            // Register Hashids with custom parameters
            services.AddSingleton<Hashids>();
            services.AddSingleton<IHashids>(_ => new Hashids(ConstantUnique.HashidsName, ConstantUnique.HashidsLength));

            services.AddAutoMapper(typeof(Mapping));
            services.AddSingleton<ICommonService, CommonService>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDDLDtlsService, DDLDtlsService>();
            services.AddScoped<IDdlhdrService, DdlhdrService>();
            services.AddScoped<IThemeDetailService, ThemeDetailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IRoleMenuService, RoleMenuService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IClassService, ClassService>();
            return services;
        }
    }
}
