using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// Abstraction of top of the dbcontext for better testability
    /// </summary>
    public interface IUnitOfWorkContext
    {
        /// <summary>
        /// Gets the EF db context
        /// </summary>
        /// <returns></returns>
        DbContext Context { get; }
        
        /// <summary>
        /// Saves changes to the EF db context
        /// </summary>
        void SaveChanges();
        
        /// <summary>
        /// Saves changes to the EF db context asynchronously
        /// </summary>
        /// <returns>The Task object of the async operation</returns>
        Task<int> SaveChangesAsync();
    }
}
