using System;
using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Service.JobScheduler
{
    public class JobItemsManager : IJobItemsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKryptonAPIRepositoryFactory _repositoryFactory;
        
        public JobItemsManager(IUnitOfWork unitOfWork, IKryptonAPIRepositoryFactory repositoryFactory)
        {
            if(unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if(repositoryFactory == null) throw new ArgumentNullException(nameof(repositoryFactory));

            _unitOfWork = unitOfWork;
            _repositoryFactory = repositoryFactory;
        }

        public Task<JobItemDto> Add(JobItemDto jobItemDto)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<JobItemDto> GetById(long id)
        {
            var repo = _repositoryFactory.GetRepository<JobItem>();
            var jobItem = await repo.GetByIdAsync(id);

            if(jobItem !=null){
                return new JobItemDto(){
                    JobItemId = jobItem.JobItemId,
                    StatusId = jobItem.StatusId,
                    JsonResult = jobItem.JsonResult,
                    JobId = jobItem.JobId,
                    CreatedUTC = jobItem.CreatedUTC,
                    ModifiedUTC = jobItem.ModifiedUTC
                };
            }

            return null;
            
        }

        public Task<JobItemDto> Update(JobItemDto jobItemDto)
        {
            throw new NotImplementedException();
        }
    }
}
