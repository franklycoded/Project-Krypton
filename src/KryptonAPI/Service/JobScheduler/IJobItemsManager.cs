using System.Threading.Tasks;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// Service class to work on JobItems
    /// </summary>
    public interface IJobItemsManager
    {
        Task<JobItemDto> GetById(long id);
        Task<JobItemDto> Add(JobItemDto jobItemDto);
        Task<JobItemDto> Update(JobItemDto jobItemDto);
        void Delete(long id);
    }
}
