using System;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.DataContractMappers;
using KryptonAPI.DataContracts;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Service
{
    /// <summary>
    /// <see cref="ICRUDManager" />
    /// </summary>
    public class CRUDManager<TContext, TEntity, TDto> : ICRUDManager<TEntity, TDto> where TContext : class where TEntity : class, IEntity 
    where TDto: class, ICRUDDto
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
            
            // Making sure that Id is 0 and the date fields are generated on the server
            dto.Id = 0;
            dto.CreatedUTC = DateTime.UtcNow;
            dto.ModifiedUTC = DateTime.UtcNow;
            
            var newEntity = _dataContractMapper.MapDtoToEntity(dto);

            _repository.Add(newEntity);
            await _unitOfWork.SaveChangesAsync();
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
            if(dto == null) throw new ArgumentNullException(nameof(dto));

            var entity = await _repository.GetByIdAsync(dto.Id);

            if(entity == null) return null;

            // Making sure the createdutc date can't be modified and the modifyutc date gets updated
            dto.CreatedUTC = entity.CreatedUTC;
            dto.ModifiedUTC = DateTime.UtcNow;

            entity = _dataContractMapper.MapDtoToEntity(dto, entity);
            
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _dataContractMapper.MapEntityToDto(entity);
        }
    }
}
