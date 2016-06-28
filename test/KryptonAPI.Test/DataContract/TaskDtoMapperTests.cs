using System;
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
        public void Test_MapTaskDtoToJobItem_TwoArgumentVersion_NotSupportException(){
            Assert.Throws<NotSupportedException>(() => {
                var mapper = new TaskDtoMapper();
                mapper.MapDtoToEntity(new TaskDto(), new JobItem());
            });
        }
        
        [Test]
        public void Test_MapJobItemToTaskDto_TwoArgumentVersion_NotSupportException(){
            Assert.Throws<NotSupportedException>(() => {
                var mapper = new TaskDtoMapper();
                mapper.MapEntityToDto(new JobItem(), new TaskDto());
            });
        }

        [Test]
        public void Test_MapEntitytoDto(){
            var jobItem = new JobItem();
            jobItem.Id = 1;
            jobItem.Code = "code";
            jobItem.JsonData = "jsonData";

            var mapper = new TaskDtoMapper();
            var dto = mapper.MapEntityToDto(jobItem);

            Assert.AreEqual(dto.JobItemId, jobItem.Id);
            Assert.AreEqual(dto.Code, jobItem.Code);
            Assert.AreEqual(dto.JsonData, jobItem.JsonData);
        }
    }
}
