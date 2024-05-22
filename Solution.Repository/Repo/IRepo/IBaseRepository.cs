using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Repository.Repo.IRepo
{
    public interface IBaseRepository<T> : IDisposable
    {
        IQueryable<T> All { get; }
        Task<List<T>> GetList(int pageSize, int pageNumber);
        Task<int> Insert(T entity);
        Task<T> InsertToGetObject(T entity);
        Task<int> InsertList(List<T> entity);
        Task<bool> Update(T entity);
        Task Delete(int id);
        Task Delete(T entity);
        void SaveChanges();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<bool> IsExist(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int Id);
        Task<List<T>> GetQueryableById(Expression<Func<T, bool>> predicate);
        int RemoveList(List<T> entity);
        Task<int> UpdateEntity(T entity);
    }
}
