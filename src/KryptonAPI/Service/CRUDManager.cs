using System;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.DataContractMappers;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Service
{
    /// <summary>
    /// <see cref="ICRUDManager" />
    /// </summary>
    public class CRUDManager<TContext, TEntity, TDto> : ICRUDManager<TEntity, TDto> where TContext : class where TEntity : class, IEntity 
    where TDto: class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;
        private readonly IDataContractMapper<TEntity, TDto> _dataContractMapper;

        /// <summary>
        /// Creates a new instance of the CRUD Manager
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance to use for persistence</param>
        /// <param name="repository">The repository to use for data persistence</param>
        /// <param name="dataContractMapper">The dto mapper</param>
        public CRUDManager(IUnitOfWork unitOfWork, IRepository<TEntity> repository, IDataContractMapper<TEntity, TDto> dataContractMapper)
        {
            if(unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if(repository == null) throw new ArgumentNullException(nameof(repository));
            if(dataContractMapper == null) throw new ArgumentNullException(nameof(dataContractMapper));

            _unitOfWork = unitOfWork;
            _repository = repository;
            _dataContractMapper = dataContractMapper;
        }

        public Task<TDto> Add(TDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<TDto> GetById(long id)
        {
            var jobItem = await _repository.GetByIdAsync(id);

            if(jobItem !=null){
                return _dataContractMapper.MapEntityToDto(jobItem);
            }

            return null;
        }

        public Task<TDto> Update(TDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
