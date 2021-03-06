using Crud.Net.Core.DataContractMapper;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers.JobScheduler
{
    /// <summary>
    /// <see cref="IDataContractMapper" />
    /// </summary>
    public class JobItemDtoMapper : CrudDtoMapper<JobItem, JobItemDto>
    {
        protected override JobItem OnMapDtoToEntity(JobItemDto dto, JobItem entity)
        {
            entity.StatusId = dto.StatusId;
            entity.JsonResult = dto.JsonResult;
            entity.ErrorMessage = dto.ErrorMessage;
            entity.JobId = dto.JobId;
            entity.Code = dto.Code;
            entity.JsonData = dto.JsonData;

            return entity;
        }

        protected override JobItemDto OnMapEntityToDto(JobItem entity, JobItemDto dto)
        {
            dto.StatusId = entity.StatusId;
            dto.JsonResult = entity.JsonResult;
            dto.ErrorMessage = entity.ErrorMessage;
            dto.JobId = entity.JobId;
            dto.Code = entity.Code;
            dto.JsonData = entity.JsonData;

            return dto;
        }
    }
}
