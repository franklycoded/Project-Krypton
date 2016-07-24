using System.Threading.Tasks;
using Crud.Net.Core.Service;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// Class to carry out operations related to JobItems
    /// </summary>
    public interface IJobItemsManager : ICrudService<JobItem, JobItemDto>
    {
        /// <summary>
        /// Gets the next job item to process from the queue
        /// </summary>
        /// <returns>The next job item to process</returns>
        Task<TaskDto> GetNextFromQueueAsync();

        /// <summary>
        /// Submits a task result
        /// </summary>
        /// <param name="taskResult">The submitted task result</param>
        /// <returns>The id of the updated JobItem</returns>
        Task<long> SubmitTaskResultAsync(TaskResultDto taskResult);
    }
}
