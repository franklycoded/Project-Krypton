using KryptonAPI.Data;
using KryptonAPI.Repository.JobScheduler;

namespace KryptonAPI.Repository
{
    /// <summary>
    /// Repository factory for the KryptonAPIContext
    /// </summary>
    public interface IKryptonAPIRepositoryFactory : IRepositoryFactory<KryptonAPIContext>
    {
        /// <summary>
        /// Creates a new JobItemsRepository
        /// </summary>
        /// <returns>The new JobItemsRepository</returns>
        IJobItemsRepository GetJobItemsRepository();
    }
}
