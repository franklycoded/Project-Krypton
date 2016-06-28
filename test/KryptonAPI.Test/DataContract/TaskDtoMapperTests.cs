using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;
using NUnit.Framework;

namespace KryptonAPI.Test.DataContract
{
    [TestFixture]
    public class TaskDtoMapperTests
    {
        [Test]
        public void Test_MapTaskDtoToJobItem_OneArgumentVersion_NotSupportException(){
            Assert.Throws<NotSupportedException>(() => {
                var mapper = new TaskDtoMapper();
                mapper.MapDtoToEntity(new TaskDto());
            });
        }
        
        [Test]
        public void Test_MapTaskDtoToJobItem_JobItemIdDoesntMatch_ArgumentOutOfRangeException(){
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItem = new JobItem();
                jobItem.Id = 1;
                var taskDto = new TaskDto();
                taskDto.JobItemId = 3;

                var mapper = new TaskDtoMapper();
                mapper.MapDtoToEntity(taskDto, jobItem);     
            });
        }
        
        [Test]
        public void Test_MapTaskDtoToJobItem_MapJsonResultOnly(){
            var jobItem = new JobItem();
            jobItem.Id = 1;


            var taskDto = new TaskDto();


        }
    }
}
