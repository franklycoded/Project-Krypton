using System;
using KryptonAPI.Configuration;
using KryptonAPI.DataContracts.JobScheduler;
using Microsoft.Extensions.Options;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// <see cref="IJobItemsQueue" />
    /// </summary>
    public class JobItemsQueue : IJobItemsQueue
    {
        
        
        public JobItemsQueue(IOptions<JobItemsQueueConfiguration> queueConfiguration)
        {
            System.Console.WriteLine("hostname: " + queueConfiguration.Value.Hostname);
        }

        /// <summary>
        /// <see cref="IJobItemsQueue.Add" />
        /// </summary>
        public bool Add(QueueJobItem jobItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IJobItemsQueue.GetNext" />
        /// </summary>
        public QueueJobItem GetNext()
        {
            throw new NotImplementedException();
        }
    }
}
