using System;
using KryptonAPI.Data.Models;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Repository
{
    /// <summary>
    /// <see cref="IRepositoryFactory" />
    /// </summary>
    public class RepositoryFactory<TContext> : IRepositoryFactory<TContext>
    {
        protected readonly IUnitOfWorkScope _unitOfWorkScope;
        
        /// <summary>
        /// Creates a new instance of the Repository Factory
        /// </summary>
        /// <param name="unitOfWorkScope">The unit of work scope to use to retrieve UnitOfWorkContexts</param>
        public RepositoryFactory(IUnitOfWorkScope unitOfWorkScope)
        {
            if(unitOfWorkScope == null) throw new ArgumentNullException(nameof(unitOfWorkScope));
            
            _unitOfWorkScope = unitOfWorkScope;
        }

        /// <summary>
        /// <see cref="IRepositoryFactory.GetRepository<TEntity>" />
        /// </summary>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return new Repository<TEntity>(_unitOfWorkScope.GetContext<TContext>());
        }
    }
}
