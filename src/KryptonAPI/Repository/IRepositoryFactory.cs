using KryptonAPI.Data.Models;

namespace KryptonAPI.Repository
{
    /// <summary>
    /// Creates repositories for the given TContext
    /// </summary>
    public interface IRepositoryFactory<TContext>
    {
        /// <summary>
        /// Creates a new repository that acts on the TEntity objects via ordinary CRUD operations
        /// </summary>
        /// <returns>The repository for TEntity objects</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        
        /// <summary>
        /// Creates a new repository of the specified TRepository type that acts on the TEntity objects
        /// </summary>
        /// <returns>The repository of type TRepository</returns>
        TRepository GetRepository<TEntity, TRepository>();
    }
}
