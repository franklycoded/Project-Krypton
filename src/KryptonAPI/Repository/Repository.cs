using System;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _context;
        
        public Repository(IUnitOfWorkContext unitOfWorkContext)
        {
            if(unitOfWorkContext == null) throw new ArgumentNullException(nameof(unitOfWorkContext));
            
            _context = unitOfWorkContext.Context;
        }

        public TEntity Add(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity).Entity;
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public TEntity Update(TEntity entity)
        {
            return _context.Set<TEntity>().Update(entity).Entity;
        }
    }
}
