using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// <see cref="IUnitOfWorkContext" />
    /// </summary>
    public class UnitOfWorkContext : IUnitOfWorkContext
    {
        private DbContext _context;
        
        /// <summary>
        /// Creates a new instance of the UnitOfWorkContext
        /// </summary>
        /// <param name="context">The EF db context</param>
        public UnitOfWorkContext(DbContext context){
            if(context == null) throw new ArgumentNullException(nameof(context));
            
            _context = context;
        }
        
        /// <summary>
        /// <see cref="IUnitOfWorkContext.Context" />
        /// </summary>
        public DbContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// <see cref="IUnitOfWorkContext.SaveChanges" />
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// <see cref="IUnitOfWorkContext.SaveChangesAsync" />
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
