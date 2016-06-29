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
        
        /// <summary>
        /// Creates a new instance of the JobItemsQueue class
        /// </summary>
        /// <param name="queueConfiguration">The configuration to use for setting up the queue connection</param>
        public JobItemsQueue(IOptions<JobItemsQueueConfiguration> queueConfiguration)
        {
            if(queueConfiguration == null) throw new ArgumentNullException(nameof(queueConfiguration));

            if(string.IsNullOrWhiteSpace(queueConfiguration.Value.Hostname)) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.Hostname), "The hostname of the queue connection cannot be empty!");

            if(queueConfiguration.Value.Port == 0) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.Port), "The port number of the queue connection cannot be 0!");

            if(string.IsNullOrWhiteSpace(queueConfiguration.Value.QueueName)) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.QueueName), "The name of the queue cannot be empty!");
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
