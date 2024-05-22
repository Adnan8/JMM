using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Repository.Repo.IRepo
{
    public interface IUnitofWork
    {
        IThemeDetailRepository ThemeDetailRepository { get; }
        IDDLDtlsRepository DDLDtlsRepository { get; }
        IDdlhdrRepository DdlhdrRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IMenuRepository MenuRepository { get; }
        IRoleMenuRepository RoleMenuRepository { get; }
		ILoginHistoryRepository LoginHistoryRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        IClassRepository ClassRepository { get; }
        IStudentRepository StudentRepository { get; }
    }
}
