using System;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;

namespace KryptonAPI.DataContractMappers.JobScheduler
{
    /// <summary>
    /// Maps a JobItem entity to a TaskDto and vice versa
    /// </summary>
    public class TaskDtoMapper : IDataContractMapper<JobItem, TaskDto>
    {
        /// <summary>
        /// This operation is not supported
        /// </summary>
        public JobItem MapDtoToEntity(TaskDto dto)
        {
            throw new NotSupportedException("It's not a valid operation to create a new JobItem instance from a TaskDto!");
        }

        /// <summary>
        /// Maps the JsonResult between the TaskDto and the JobItem entity instances
        /// </summary>
        /// <param name="dto">The TaskDto instance to map</param>
        /// <param name="entity">The JobItem instance to be mapped</param>
        /// <returns>The mapper JobItem instance</returns>
        public JobItem MapDtoToEntity(TaskDto dto, JobItem entity)
        {
            entity.JsonResult = dto.JsonResult;
            return entity;
        }

        /// <summary>
        /// Creates a new TaskDto instance based on the JobItem entity
        /// </summary>
        /// <param name="entity">The JobItem entity to map</param>
        /// <returns>The new TaskDto instance</returns>
        public TaskDto MapEntityToDto(JobItem entity)
        {
            var dto = new TaskDto(){
                JobItemId = entity.Id,
                Code = entity.Code,
                JsonData = entity.JsonData,
                JsonResult = entity.JsonResult
            };

            return dto;
        }

        /// <summary>
        /// This operation is not supported
        /// </summary>
        public TaskDto MapEntityToDto(JobItem entity, TaskDto dto)
        {
            throw new NotSupportedException("It's not a valid operation to modify a TaskDto instance using the data contract mapper!");
        }
    }
}
