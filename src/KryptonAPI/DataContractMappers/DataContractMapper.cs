using KryptonAPI.Data.Models;
using KryptonAPI.DataContracts;

namespace KryptonAPI.DataContractMappers
{
    public abstract class DataContractMapper<TEntity, TDto> : IDataContractMapper<TEntity, TDto> 
    where TEntity : class, IEntity, new()
    where TDto : class, ICRUDDto, new()
    {
        /// <summary>
        /// <see cref="IDataContractMapper.MapDtoToEntity" />
        /// </summary>
        public TEntity MapDtoToEntity(TDto dto)
        {
            var entity = OnMapDtoToEntity(dto, new TEntity());
            
            // Making sure the derived class doesn't change these values
            entity.Id = dto.Id;
            entity.CreatedUTC = dto.CreatedUTC;
            entity.ModifiedUTC = dto.ModifiedUTC;

            return entity;
        }

        /// <summary>
        /// <see cref="IDataContractMapper.MapEntityToDto" />
        /// </summary>
        public TDto MapEntityToDto(TEntity entity)
        {
            var dto = OnMapEntityToDto(entity, new TDto());
            
            // Making sure the derived class doesn't change these values
            dto.Id = entity.Id;
            dto.CreatedUTC = entity.CreatedUTC;
            dto.ModifiedUTC = entity.ModifiedUTC;

            return OnMapEntityToDto(entity, dto);
        }

        // <summary>
        /// <see cref="IDataContractMapper.MapEntityToDto" />
        /// </summary>
        public TDto MapEntityToDto(TEntity entity, TDto existingDto)
        {
            var dto = OnMapEntityToDto(entity, existingDto);
            
            // Making sure the derived class doesn't change these values
            dto.Id = entity.Id;
            dto.CreatedUTC = entity.CreatedUTC;
            dto.ModifiedUTC = entity.ModifiedUTC;

            return OnMapEntityToDto(entity, dto);
        }

        /// <summary>
        /// <see cref="IDataContractMapper.MapDtoToEntity" />
        /// </summary>
        public TEntity MapDtoToEntity(TDto dto, TEntity existingEntity)
        {
            var entity = OnMapDtoToEntity(dto, existingEntity);
            
            // Making sure the derived class doesn't change these values
            entity.Id = dto.Id;
            entity.CreatedUTC = dto.CreatedUTC;
            entity.ModifiedUTC = dto.ModifiedUTC;

            return entity;
        }

        protected abstract TDto OnMapEntityToDto(TEntity entity, TDto dto);
        protected abstract TEntity OnMapDtoToEntity(TDto dto, TEntity entity);
        
    }
}
