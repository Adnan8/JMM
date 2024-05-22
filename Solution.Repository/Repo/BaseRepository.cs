using Microsoft.EntityFrameworkCore;
using Solution.DAL.Data;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private AppDbContext _context;
        public int CompId { get;
            
            set; }
        public AppDbContext getContext()
        {

            return _context;

        }

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public Task<List<T>> GetList(int pageSize, int pageNumber)
        {
            return _context.Set<T>().Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        }

        public async Task Delete(T entity)
        {
            var e = _context.Set<T>().Find(entity);
            _context.Set<T>().Remove(e);
        }

        public async Task Delete(int id)
        {
            var obj = await _context.Set<T>().FindAsync(id);
            if (obj != null)
                _context.Remove(obj);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> Insert(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertList(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Count();
        }

        public async Task<T> InsertToGetObject(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task GetSaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public Task<bool> IsExist(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AnyAsync(predicate);
        }

        //public Task<T> GetById(int Id)
        //{
        //    return _context.Set<T>().FindAsync(Id);
        //}

        public async Task<List<T>> GetQueryableById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public int RemoveList(List<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
            return _context.SaveChanges();
        }

        public async Task<T> GetById(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
            //throw new NotImplementedException();
        }
        public async Task<int> UpdateEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }


    }
}
