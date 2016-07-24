using System;
using System.Threading.Tasks;
using Crud.Net.Core.DataContractMapper;
using Crud.Net.Core.Repository;
using Crud.Net.Core.Service;
using Crud.Net.Core.UnitOfWork;
using KryptonAPI.Data;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// <see cref="IJobItemsManager" />
    /// </summary>
    public class JobItemsManager : CrudService<KryptonAPIContext, JobItem, JobItemDto>, IJobItemsManager
    {
        private readonly IJobItemsQueue _jobItemsQueue;
        private readonly ICrudDtoMapper<JobItem, TaskDto> _taskDataContractMapper;
        
        public JobItemsManager(IUnitOfWork unitOfWork,
            ICrudRepository<JobItem> repository,
            ICrudDtoMapper<JobItem, JobItemDto> dataContractMapper,
            ICrudDtoMapper<JobItem, TaskDto> taskDataContractMapper,
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
        public async Task<long> SubmitTaskResultAsync(TaskResultDto taskResult)
        {
            if(taskResult == null) throw new ArgumentNullException(nameof(taskResult));
            
            var jobItem = await _repository.GetByIdAsync(taskResult.JobItemId);

            if(jobItem == null) throw new Exception(string.Format("Can't find job item with id {0} in the database!", taskResult.JobItemId));

            jobItem.JsonResult = taskResult.TaskResult;
            jobItem.StatusId = (long)((taskResult.IsSuccessful) ? EJobStatus.Success : EJobStatus.Fail);
            jobItem.ErrorMessage = taskResult.ErrorMessage;

            await _unitOfWork.SaveChangesAsync();

            return jobItem.Id;
        }
    }
}
