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

        /// <summary>
        /// <see cref="ICRUDManager.AddAsync" />
        /// </summary>
        public async Task<TDto> AddAsync(TDto dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto));

            var newEntity = _dataContractMapper.MapDtoToEntity(dto);

            newEntity.CreatedUTC = DateTime.UtcNow;
            newEntity.ModifiedUTC = DateTime.UtcNow;

            _repository.Add(newEntity);
            System.Console.WriteLine("before save");
            await _unitOfWork.SaveChangesAsync();
            System.Console.WriteLine("returning result");
            return _dataContractMapper.MapEntityToDto(newEntity);
        }

        /// <summary>
        /// <see cref="ICRUDManager.DeleteAsync" />
        /// </summary>
        public async Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="ICRUDManager.GetByIdAsync" />
        /// </summary>
        public async Task<TDto> GetByIdAsync(long id)
        {
            var jobItem = await _repository.GetByIdAsync(id);
            
            if(jobItem !=null){
                return _dataContractMapper.MapEntityToDto(jobItem);
            }

            return null;
        }

        /// <summary>
        /// <see cref="ICRUDManager.UpdateAsync" />
        /// </summary>
        public async Task<TDto> UpdateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
