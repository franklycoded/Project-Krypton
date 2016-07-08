using System;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// Adds and removes items from the jobitem queue
    /// </summary>
    public interface IJobItemsQueue : IDisposable
    {
        /// <summary>
        /// Gets the next job item from the queue
        /// </summary>
        /// <returns>The QueueJobItem, repesenting the next job item in the queue</returns>
        QueueJobItem GetNext();
        
        /// <summary>
        /// Adds a new job item to the queue
        /// </summary>
        /// <param name="jobItem">The job item to add</param>
        /// <returns>True if adding the job item was successful, false otherwise</returns>
        bool Add(QueueJobItem jobItem);
    }
}
