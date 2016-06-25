using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers.JobScheduler
{
    /// <summary>
    /// <see cref="IDataContractMapper" />
    /// </summary>
    public class JobItemDtoMapper : DataContractMapper<JobItem, JobItemDto>
    {
        protected override JobItem OnMapDtoToEntity(JobItemDto dto, JobItem entity)
        {
            entity.StatusId = dto.StatusId;
            entity.JsonResult = dto.JsonResult;
            entity.JobId = dto.JobId;

            return entity;
        }

        protected override JobItemDto OnMapEntityToDto(JobItem entity, JobItemDto dto)
        {
            dto.StatusId = entity.StatusId;
            dto.JsonResult = entity.JsonResult;
            dto.JobId = entity.JobId;

            return dto;
        }
    }
}
