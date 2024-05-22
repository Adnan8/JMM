using Solution.DAL.Data;
using Solution.Repository.Repo.IRepo;

namespace Solution.Repository.Repo
{
    public class UnitofWork : IUnitofWork
    {
        private readonly AppDbContext _context;
        private IThemeDetailRepository _themeDetailRepository;
        private IDDLDtlsRepository _ddldtlsRepository;
        private IDdlhdrRepository _ddlhdrRepository;
        private ICompanyRepository _companyRepository;
        private IPermissionRepository _permissionRepository;
        private IMenuRepository _menuRepository;
        private IRolePermissionRepository _rolePermissionRepository;
        private IRoleMenuRepository _rolemenuRepository;
		private ILoginHistoryRepository _loginhistoryRepository;
        private IDocumentRepository _documentRepository;
        private IClassRepository _classRepository;
        private IStudentRepository _studentRepository;
        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }
        public IThemeDetailRepository ThemeDetailRepository
        {
            get
            {
                if (_themeDetailRepository == null)
                {
                    _themeDetailRepository = new ThemeDetailRepository(_context);
                }
                return _themeDetailRepository;
            }
        }

        public IDDLDtlsRepository DDLDtlsRepository
        {
            get
            {
                if (_ddldtlsRepository == null)
                {
                    _ddldtlsRepository = new DDLDtlsRepository(_context);
                }
                return _ddldtlsRepository;
            }
        }

        public IDdlhdrRepository DdlhdrRepository
        {
            get
            {
                if (_ddlhdrRepository == null)
                {
                    _ddlhdrRepository = new DdlhdrRepository(_context);
                }
                return _ddlhdrRepository;
            }
        }
        public ICompanyRepository CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_context);
                }
                return _companyRepository;
            }
        }
      
        public IMenuRepository MenuRepository
        {
            get
            {
                if (_menuRepository == null)
                {
                    _menuRepository = new MenuRepository(_context);
                }
                return _menuRepository;
            }
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                if (_permissionRepository == null)
                {
                    _permissionRepository = new PermissionRepository(_context);
                }
                return _permissionRepository;
            }
        }

        public IRolePermissionRepository RolePermissionRepository
        {
            get
            {
                if (_rolePermissionRepository == null)
                {
                    _rolePermissionRepository = new RolePermissionRepository(_context);
                }
                return _rolePermissionRepository;
            }
        }

        public IRoleMenuRepository RoleMenuRepository
        {
            get
            {
                if (_rolemenuRepository == null)
                {
                    _rolemenuRepository = new RoleMenuRepository(_context);
                }
                return _rolemenuRepository;
            }
        }
		public ILoginHistoryRepository LoginHistoryRepository
		{
			get
			{
				if (_loginhistoryRepository == null)
				{
					_loginhistoryRepository = new LoginHistoryRepository(_context);
				}
				return _loginhistoryRepository;
			}
		}
        public IDocumentRepository DocumentRepository
        {
            get
            {
                if (_documentRepository == null)
                {
                    _documentRepository = new DocumentRepository(_context);
                }
                return _documentRepository;
            }
        }
        public IClassRepository ClassRepository
        {
            get
            {
                if (_classRepository == null)
                {
                    _classRepository = new ClassRepository(_context);
                }
                return _classRepository;
            }
        }
        public IStudentRepository StudentRepository
        {
            get
            {
                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_context);
                }
                return _studentRepository;
            }
        }

    }

}
