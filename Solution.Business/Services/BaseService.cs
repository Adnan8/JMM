using AutoMapper;
using Solution.Business.Services.IServices;
using Solution.DAL.Data;

namespace Solution.Business.Services
{
    public class BaseService : IBaseService
    {
        
        private readonly AppDbContext _DbContext;
        private AppDbContext _db;
        private IMapper _mapper;

        public BaseService(IMapper mapper, AppDbContext db)
        {
            _db = db;
        }

      
        public int CompanyIdDecode()
        {
            throw new NotImplementedException();
        }

        public string CompanyIdEncode(int request)
        {
            throw new NotImplementedException();
        }

        public int Decode(string request)
        {
            throw new NotImplementedException();
        }

        public string Encode(int request)
        {
            throw new NotImplementedException();
        }

        public string GetLoginUserId()
        {
            throw new NotImplementedException();
        }
    }
}
