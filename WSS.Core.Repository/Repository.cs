using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WSS.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDataBaseContext _context;

        public Repository(AppDataBaseContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            try
            {
                _context.Add(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        public IQueryable<T> Queryable()
        {
            return _context.Set<T>();
        }     
        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        //public T FindById(int id, params Expression<Func<T, object>>[] includeProperties)
        //{
        //    return FindAll()
        //}
        //public T FindById(int id, params Expression<Func<T, object>>[] includeProperties)
        //{
        //    return FindAll(includeProperties).Where(x=>x.);
        //}
        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        //public void Remove(int id)
        //{
        //    Remove(FindById(id));
        //}

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            //_context.Entry(entity).State = EntityState.Modified;
            //_context.SaveChanges();
            _context.Set<T>().Update(entity);
        }
       
      
    }
}
