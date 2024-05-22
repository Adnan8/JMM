using AutoMapper;
using Solution.Business.Services.IServices;
using Solution.DAL.Data;

namespace Solution.Business.Services
{
    public class CommonService : ICommonService
    {
        private readonly IMapper _mapper;

        public CommonService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
