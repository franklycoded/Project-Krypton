using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// <see cref="IJobItemsManager" />
    /// </summary>
    public class JobItemsManager : CRUDManager<KryptonAPIContext, JobItem, JobItemDto>, IJobItemsManager
    {
        private readonly IJobItemsQueue _jobItemsQueue;
        
        public JobItemsManager(IUnitOfWork unitOfWork,
            IRepository<JobItem> repository,
            IDataContractMapper<JobItem, JobItemDto> dataContractMapper,
            IJobItemsQueue jobItemsQueue) 
            : base(unitOfWork, repository, dataContractMapper)
        {
            if(jobItemsQueue == null) throw new ArgumentNullException(nameof(jobItemsQueue));
            
            _jobItemsQueue = jobItemsQueue;
        }

        /// <summary>
        /// <see cref="IJobItemsManager.GetNextFromQueueAsync" />
        /// </summary>
        public Task<TaskDto> GetNextFromQueueAsync()
        {
            throw new NotImplementedException();
        }
    }
}
