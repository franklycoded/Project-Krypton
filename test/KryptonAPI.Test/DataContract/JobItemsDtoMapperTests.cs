using System;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;
using NUnit.Framework;

namespace KryptonAPI.Test.DataContract
{
    [TestFixture]
    public class JobItemsDtoMapperTests
    {
        [Test]
        public void Test_MapJobItemToJobItemDto(){
            var mapper = new JobItemDtoMapper();
            var createdDate = DateTime.UtcNow;
            var modifiedDate = DateTime.UtcNow.AddDays(1);

            var entity = new JobItem(){
                Id = 1,
                StatusId = 2,
                JsonResult = "jsonResult",
                ErrorMessage = "errorMessage",
                Code = "code",
                JsonData = "jsonData",
                JobId = 3,
                CreatedUTC = createdDate,
                ModifiedUTC = modifiedDate
            };

            var dto = mapper.MapEntityToDto(entity);

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.StatusId, entity.StatusId);
            Assert.AreEqual(dto.JsonResult, entity.JsonResult);
            Assert.AreEqual(dto.ErrorMessage, entity.ErrorMessage);
            Assert.AreEqual(dto.Code, entity.Code);
            Assert.AreEqual(dto.JsonData, entity.JsonData);
            Assert.AreEqual(dto.JobId, entity.JobId);
            Assert.AreEqual(dto.CreatedUTC, entity.CreatedUTC);
            Assert.AreEqual(dto.ModifiedUTC, entity.ModifiedUTC);
        }

        [Test]
        public void Test_MapJobItemDtoToJobItem(){
            var mapper = new JobItemDtoMapper();
            var createdDate = DateTime.UtcNow;
            var modifiedDate = DateTime.UtcNow.AddDays(1);

            var dto = new JobItemDto(){
                Id = 1,
                StatusId = 2,
                JsonResult = "jsonResult",
                ErrorMessage = "errorMessage",
                Code = "code",
                JsonData = "jsonData",
                JobId = 3,
                CreatedUTC = createdDate,
                ModifiedUTC = modifiedDate
            };

            var entity = mapper.MapDtoToEntity(dto);

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.StatusId, entity.StatusId);
            Assert.AreEqual(dto.JsonResult, entity.JsonResult);
            Assert.AreEqual(dto.ErrorMessage, entity.ErrorMessage);
            Assert.AreEqual(dto.Code, entity.Code);
            Assert.AreEqual(dto.JsonData, entity.JsonData);
            Assert.AreEqual(dto.JobId, entity.JobId);
            Assert.AreEqual(dto.CreatedUTC, entity.CreatedUTC);
            Assert.AreEqual(dto.ModifiedUTC, entity.ModifiedUTC);
        }
    }
}
