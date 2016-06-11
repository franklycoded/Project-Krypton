using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KryptonAPI.Repository
{
    public class UnitOfWorkContext<TContext> : IUnitOfWorkContext<TContext> where TContext:new()
    {
        private TContext _context;
        
        public UnitOfWorkContext()
        {
            _context = new TContext();
        }

        public TContext GetContext()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
