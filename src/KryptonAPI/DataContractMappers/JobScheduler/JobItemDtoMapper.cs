using System;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers.JobScheduler
{
    /// <summary>
    /// <see cref="IDataContractMapper" />
    /// </summary>
    public class JobItemDtoMapper : IDataContractMapper<JobItem, JobItemDto>
    {
        /// <summary>
        /// <see cref="IDataContractMapper.MapDtoToEntity" />
        /// </summary>
        public JobItem MapDtoToEntity(JobItemDto dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IDataContractMapper.MapEntityToDto" />
        /// </summary>
        public JobItemDto MapEntityToDto(JobItem entity)
        {
            return new JobItemDto(){
                    JobItemId = entity.JobItemId,
                    StatusId = entity.StatusId,
                    JsonResult = entity.JsonResult,
                    JobId = entity.JobId,
                    CreatedUTC = entity.CreatedUTC,
                    ModifiedUTC = entity.ModifiedUTC
                };
        }
    }
}
