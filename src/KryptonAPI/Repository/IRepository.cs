using System.Threading.Tasks;

namespace KryptonAPI.Repository
{
    /// <summary>
    /// Repository for the specified entity
    /// </summary>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Retrieves the entity by id
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>The retreived entity</returns>
        Task<TEntity> GetByIdAsync(long id);
        
        /// <summary>
        /// Adds the entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        TEntity Add(TEntity entity);
        
        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">The entity with the updated data</param>
        /// <returns>The updatd entity</returns>
        TEntity Update(TEntity entity);
        
        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete(TEntity entity);
    }
}
