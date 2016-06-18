using System;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers
{
    /// <summary>
    /// <see cref="IDataContractMapperFactory" />
    /// </summary>
    public class DataContractMapperFactory : IDataContractMapperFactory
    {
        /// <summary>
        /// <see cref="IDataContractMapper<JobItem, JobItemDto>.MapDtoToEntity" />
        /// </summary>
        public JobItem MapDtoToEntity(JobItemDto dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IDataContractMapper<JobItem, JobItemDto>.MapEntityToDto" />
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
