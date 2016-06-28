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
        private Mock<IDataContractMapper<JobItem, TaskDto>> _mockTaskDataContractMapper;
        private Mock<IJobItemsQueue> _mockJobItemsQueue;
        
        [SetUp]
        public void Init(){
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IRepository<JobItem>>();
            _mockDataContractMapper = new Mock<IDataContractMapper<JobItem, JobItemDto>>();
            _mockJobItemsQueue = new Mock<IJobItemsQueue>();
            _mockTaskDataContractMapper = new Mock<IDataContractMapper<JobItem, TaskDto>>();
        }

        [Test]
        public void Test_NoJobItemsQueue_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, null);                
            });
        }

        [Test]
        public void Test_NoTaskDataContractMapper_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, null, _mockJobItemsQueue.Object);                
            });
        }

        [Test]
        public void Test_NextJobItemFound_TaskReturned(){
            var queuedJobItem = new QueueJobItem(1);
            var jobItem = new JobItem(){
                Id = 1,
                Code = "code",
                JsonData = "data"
            };
            var taskDto = new TaskDto();

            _mockJobItemsQueue.Setup(m => m.GetNext()).Returns(queuedJobItem);
            _mockRepository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(jobItem);
            _mockTaskDataContractMapper.Setup(m => m.MapEntityToDto(jobItem)).Returns(taskDto);

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            var result = manager.GetNextFromQueueAsync().Result;

            _mockJobItemsQueue.Verify(m => m.GetNext(), Times.Once);
            _mockRepository.Verify(m => m.GetByIdAsync(1), Times.Once);
            _mockTaskDataContractMapper.Verify(m => m.MapEntityToDto(jobItem), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreSame(result, taskDto);
        }

        [Test]
        public void Test_NoJobItemsToProcess_ReturnNull(){
            _mockJobItemsQueue.Setup(m => m.GetNext()).Returns(default(QueueJobItem));

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            var result = manager.GetNextFromQueueAsync().Result;

            Assert.IsNull(result);
            _mockJobItemsQueue.Verify(m => m.GetNext(), Times.Once);
            _mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_ErrorWhileReadingQueue_LetExceptionBubbleUp(){
            Assert.ThrowsAsync<Exception>(() => {
                _mockJobItemsQueue.Setup(m => m.GetNext()).Throws(new Exception());

                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

                return manager.GetNextFromQueueAsync();    
            });
        }

        [Test]
        public void Test_ErrorWhileLookingUpJobitem_LetExceptionBubbleUp(){
            Assert.ThrowsAsync<Exception>(() => {
                var queuedJobItem = new QueueJobItem(1);
                
                _mockJobItemsQueue.Setup(m => m.GetNext()).Returns(queuedJobItem);
                _mockRepository.Setup(m => m.GetByIdAsync(1)).Throws(new Exception());

                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

                return manager.GetNextFromQueueAsync();    
            });
        }

        [Test]
        public void Test_JobItemNotFoundInDatabase_ExceptionThrown(){
            Assert.ThrowsAsync<Exception>(() => {
                var queuedJobItem = new QueueJobItem(1);
                
                _mockJobItemsQueue.Setup(m => m.GetNext()).Returns(queuedJobItem);
                _mockRepository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(null);

                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

                return manager.GetNextFromQueueAsync();    
            });
        }

        [Test]
        public void Test_NextJobItemFound_StatusSetToRunning(){
            var queuedJobItem = new QueueJobItem(1);
            var jobItem = new JobItem(){
                Id = 1,
                Code = "code",
                JsonData = "data",
                StatusId = 2
            };
            var taskDto = new TaskDto();

            _mockJobItemsQueue.Setup(m => m.GetNext()).Returns(queuedJobItem);
            _mockRepository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(jobItem);
            _mockTaskDataContractMapper.Setup(m => m.MapEntityToDto(jobItem)).Returns(taskDto);

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            var result = manager.GetNextFromQueueAsync().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(jobItem.StatusId, (long)EJobStatus.Running);
            _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
        }
    }
}
