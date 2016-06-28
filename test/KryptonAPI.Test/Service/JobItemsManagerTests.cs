using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Repository;
using KryptonAPI.Service.JobScheduler;
using KryptonAPI.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Service
{
    [TestFixture]
    public class JobItemsManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<JobItem>> _mockRepository;
        private Mock<IDataContractMapper<JobItem, JobItemDto>> _mockDataContractMapper;
        private Mock<IJobItemsQueue> _mockJobItemsQueue;
        
        [SetUp]
        public void Init(){
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IRepository<JobItem>>();
            _mockDataContractMapper = new Mock<IDataContractMapper<JobItem, JobItemDto>>();
            _mockJobItemsQueue = new Mock<IJobItemsQueue>();
        }

        [Test]
        public void Test_NoJobItemsQueue_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, null);                
            });
        }

        [Test]
        public void Test_NextJobItemFound_TaskReturned(){
            // var jobItem = new JobItem(){
            //     Id = 1,

            // }
        }
    }
}
