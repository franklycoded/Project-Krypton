using System;
using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Service.JobScheduler
{
    public class JobItemsManager : IJobItemsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKryptonAPIRepositoryFactory _repositoryFactory;
        private readonly IDataContractMapperFactory _dataContractMapperFactory;
        
        public JobItemsManager(IUnitOfWork unitOfWork, IKryptonAPIRepositoryFactory repositoryFactory, IDataContractMapperFactory dataContractMapperFactory)
        {
            if(unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if(repositoryFactory == null) throw new ArgumentNullException(nameof(repositoryFactory));
            if(dataContractMapperFactory == null) throw new ArgumentNullException(nameof(dataContractMapperFactory));

            _unitOfWork = unitOfWork;
            _repositoryFactory = repositoryFactory;
            _dataContractMapperFactory = dataContractMapperFactory;
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
                return _dataContractMapperFactory.MapEntityToDto(jobItem);
            }

            return null;
        }

        public Task<JobItemDto> Update(JobItemDto jobItemDto)
        {
            throw new NotImplementedException();
        }
    }
}
