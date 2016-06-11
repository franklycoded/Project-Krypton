using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KryptonAPI.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
    {
        public Repository()
        {
        }

        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(long id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
