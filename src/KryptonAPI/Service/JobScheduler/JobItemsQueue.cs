using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// <see cref="IJobItemsQueue" />
    /// </summary>
    public class JobItemsQueue : IJobItemsQueue
    {
        public JobItemsQueue()
        {
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
