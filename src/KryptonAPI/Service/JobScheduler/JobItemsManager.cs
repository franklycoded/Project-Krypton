using System;
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
        private readonly IDataContractMapper<JobItem, TaskDto> _taskDataContractMapper;
        
        public JobItemsManager(IUnitOfWork unitOfWork,
            IRepository<JobItem> repository,
            IDataContractMapper<JobItem, JobItemDto> dataContractMapper,
            IDataContractMapper<JobItem, TaskDto> taskDataContractMapper,
            IJobItemsQueue jobItemsQueue) 
            : base(unitOfWork, repository, dataContractMapper)
        {
            if(jobItemsQueue == null) throw new ArgumentNullException(nameof(jobItemsQueue));
            if(taskDataContractMapper == null) throw new ArgumentNullException(nameof(taskDataContractMapper));

            _jobItemsQueue = jobItemsQueue;
            _taskDataContractMapper = taskDataContractMapper;
        }

        /// <summary>
        /// <see cref="IJobItemsManager.GetNextFromQueueAsync" />
        /// </summary>
        public async Task<TaskDto> GetNextFromQueueAsync()
        {
            var queuedJobItem = _jobItemsQueue.GetNext();

            if(queuedJobItem == null) return null;

            var jobItem = await _repository.GetByIdAsync(queuedJobItem.Id);

            if(jobItem == null) throw new Exception("Can't find task in database!");

            jobItem.StatusId = (long)EJobStatus.Running;
            await _unitOfWork.SaveChangesAsync();

            return _taskDataContractMapper.MapEntityToDto(jobItem);
        }

        /// <summary>
        /// <see cref="IJobItemsManager.SubmitTaskResult" />
        /// </summary>
        public async Task<bool> SubmitTaskResultAsync(TaskResultDto taskResult)
        {
            throw new NotImplementedException();
        }
    }
}
