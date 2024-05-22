using AutoMapper;
using Solution.Common;
using Solution.Common.ViewModel;
using Solution.DAL.Models;

namespace Solution.Business.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Company, CompanyVM>()
              .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<CompanyVM, Company>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));
            CreateMap<Menu, MenuVM>()
              .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<MenuVM, Menu>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));
            CreateMap<ThemeDetail, ThemeDetailVM>()
                .ForMember(dest => dest.Id,
                           opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<ThemeDetailVM, ThemeDetail>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));

            CreateMap<DDLDtls, DDLDtlsVM>()
               .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id))
               .ForMember(dest => dest.DdlhdrId, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.DdlhdrId));

            CreateMap<DDLDtlsVM, DDLDtls>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id))
                .ForMember(dest => dest.DdlhdrId, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.DdlhdrId));

            CreateMap<Ddlhdr, DdlhdrVM>()
              .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<DdlhdrVM, Ddlhdr>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));
           
            CreateMap<Permission, PermissionVM>()
             .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<PermissionVM, Permission>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));

            CreateMap<Menu, MenuVM>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<MenuVM, Menu>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));

            CreateMap<Document, DocumentVM>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id));

            CreateMap<DocumentVM, Document>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id));

            CreateMap<Class, ClassVM>()
            .ForMember(dest => dest.ClassId, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.ClassId));

            CreateMap<ClassVM, Class>()
                .ForMember(dest => dest.ClassId, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.ClassId));
          

            CreateMap<RolePermission, RolePermissionVM>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id))
                .ForMember(dest => dest.PermissionId, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.PermissionId));

            // Mapping from RolePermissionVM to RolePermission
            CreateMap<RolePermissionVM, RolePermission>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id))
                .ForMember(dest => dest.PermissionId, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.PermissionId));
            CreateMap<RoleMenu, RoleMenuVM>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.Id))
                .ForMember(dest => dest.MenuId, opt => opt.ConvertUsing(new IntIdToStringConverter(), src => src.MenuId));

            // Mapping from RolePermissionVM to RolePermission
            CreateMap<RoleMenuVM, RoleMenu>()
                .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.Id))
                .ForMember(dest => dest.MenuId, opt => opt.ConvertUsing(new HashIdToIntConverter(), src => src.MenuId));

        }



    }
}
