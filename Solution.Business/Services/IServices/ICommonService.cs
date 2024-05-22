namespace Solution.Business.Services.IServices
{
    public interface ICommonService
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
