using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// Class to carry out operations related to JobItems
    /// </summary>
    public interface IJobItemsManager : ICRUDManager<JobItem, JobItemDto>
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
        /// <returns>True if the submission was successful, false otherwise</returns>
        Task<bool> SubmitTaskResultAsync(TaskResultDto taskResult);
    }
}
