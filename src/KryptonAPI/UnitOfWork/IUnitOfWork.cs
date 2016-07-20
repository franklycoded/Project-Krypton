using System;
using System.Threading.Tasks;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// Interface to save changes across UnitOfWorkContexts
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves the changes across all maintained UnitOfWorkContexts 
        /// </summary>
        void SaveChanges();
        
        /// <summary>
        /// Saves the changes asynchronously across all maintained UnitOfWorkContexts
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
