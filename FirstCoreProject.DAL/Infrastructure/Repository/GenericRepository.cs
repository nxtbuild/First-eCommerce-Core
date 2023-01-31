using FirstCoreProject.DAL.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;

          //  _context.Products.Include(x => x.Category);

            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate=null,string? includeProperties=null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if(includeProperties!=null)
            {
                foreach(var item in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.ToList();
        }

        public T GetT(Expression<Func<T, bool>> predicate, string? includeProperties=null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(predicate);

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.FirstOrDefault();
        }
    }
}
