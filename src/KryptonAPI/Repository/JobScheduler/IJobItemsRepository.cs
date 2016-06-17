using KryptonAPI.Data.Models.JobScheduler;

namespace KryptonAPI.Repository.JobScheduler
{
    public interface IJobItemsRepository : IRepository<JobItem>
    {
        void TestOperation();
    }
}
