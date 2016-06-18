using KryptonAPI.Data;
using KryptonAPI.Repository.JobScheduler;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Repository
{
    /// <summary>
    /// <see cref="IKryptonAPIRepositoryFactory" />
    /// </summary>
    public class KryptonAPIRepositoryFactory : RepositoryFactory<KryptonAPIContext>, IKryptonAPIRepositoryFactory
    {
        /// <summary>
        /// Creates a new instance of the KryptonAPIRepositoryFactory
        /// </summary>
        /// <param name="unitOfWorkScope">The unit of work scope to use when creating repositories</param>
        public KryptonAPIRepositoryFactory(IUnitOfWorkScope unitOfWorkScope) : base(unitOfWorkScope)
        {
        }

        /// <summary>
        /// <see cref="IKryptonAPIRepositoryFactory.GetJobItemsRepository" />
        /// </summary>
        public IJobItemsRepository GetJobItemsRepository()
        {
            return new JobItemsRepository(_unitOfWorkScope.GetContext<KryptonAPIContext>());
        }
    }
}
