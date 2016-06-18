using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers
{
    /// <summary>
    /// The factory to create data contract mappers
    /// </summary>
    public interface IDataContractMapperFactory : IDataContractMapper<JobItem, JobItemDto>
    {
    }
}
