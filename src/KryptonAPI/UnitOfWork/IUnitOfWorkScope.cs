using System;
using System.Threading.Tasks;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// Interface of the UnitOfWorkScope
    /// </summary>
    public interface IUnitOfWorkScope : IDisposable
    {
        /// <summary>
        /// Gets a new UnitOfWorkContext from the context cache.
        /// If the context with the specified type doesn't exist, it creates a new one and adds it to the cache
        /// </summary>
        /// <returns>The UnitOfWorkContext with the specified type</returns>
        IUnitOfWorkContext GetContext<TContext>();

        /// <summary>
        /// Saves the changes across all maintained UnitOfWorkContexts 
        /// </summary>
        void SaveChanges();
        
        /// <summary>
        /// Saves the changes asynchronously across all maintained UnitOfWorkContexts
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
