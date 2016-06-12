using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.UnitOfWork
{
    public class UnitOfWorkContext : IUnitOfWorkContext
    {
        private DbContext _context;
        
        public UnitOfWorkContext(DbContext context){
            _context = context;
        }
        
        public DbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
