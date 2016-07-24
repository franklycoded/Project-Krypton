using System;
using Crud.Net.Core.DataContractMapper;
using Crud.Net.Core.Repository;
using Crud.Net.Core.UnitOfWork;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Service.JobScheduler;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Service
{
    [TestFixture]
    public class JobItemsManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICrudRepository<JobItem>> _mockRepository;
        private Mock<ICrudDtoMapper<JobItem, JobItemDto>> _mockDataContractMapper;
        private Mock<ICrudDtoMapper<JobItem, TaskDto>> _mockTaskDataContractMapper;
        private Mock<IJobItemsQueue> _mockJobItemsQueue;
        
        [SetUp]
        public void Init(){
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<ICrudRepository<JobItem>>();
            _mockDataContractMapper = new Mock<ICrudDtoMapper<JobItem, JobItemDto>>();
            _mockJobItemsQueue = new Mock<IJobItemsQueue>();
            _mockTaskDataContractMapper = new Mock<ICrudDtoMapper<JobItem, TaskDto>>();
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

        [Test]
        public void Test_SubmitTaskResultAsync_NullTaskResult_ArgumentNullException(){
            Assert.ThrowsAsync<ArgumentNullException>(() => {
                var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

                return manager.SubmitTaskResultAsync(null); 
            });
        }

        [Test]
        public void Test_SubmitTaskResultAsync_SuccessfulTaskResult_UpdateJobItem_ReturnsJobItemId(){
            var jobItem = new JobItem(){
                Id = 2,
                StatusId = 3,
                JsonResult = "noresult",
                ErrorMessage = null
            };

            var taskResult = new TaskResultDto(){
                JobItemId = 2,
                TaskResult = "task result",
                IsSuccessful = true,
                ErrorMessage = null
            };
            
            _mockRepository.Setup(m => m.GetByIdAsync(2)).ReturnsAsync(jobItem);

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            var result = manager.SubmitTaskResultAsync(taskResult).Result;

            Assert.AreEqual(result, jobItem.Id);
            _mockRepository.Verify(m => m.GetByIdAsync(2), Times.Once);
            Assert.AreEqual(jobItem.JsonResult, taskResult.TaskResult);
            Assert.AreEqual(jobItem.StatusId, 4);
            Assert.IsNull(jobItem.ErrorMessage);
            _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void Test_SubmitTaskResultAsync_ErrorTaskResult_UpdateJobItem(){
            var jobItem = new JobItem(){
                Id = 2,
                StatusId = 3,
                JsonResult = "noresult",
                ErrorMessage = null
            };

            var taskResult = new TaskResultDto(){
                JobItemId = 2,
                TaskResult = "error task result",
                IsSuccessful = false,
                ErrorMessage = "task result error message"
            };
            
            _mockRepository.Setup(m => m.GetByIdAsync(2)).ReturnsAsync(jobItem);

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            var result = manager.SubmitTaskResultAsync(taskResult).Result;

            Assert.AreEqual(result, jobItem.Id);
            _mockRepository.Verify(m => m.GetByIdAsync(2), Times.Once);
            Assert.AreEqual(jobItem.JsonResult, taskResult.TaskResult);
            Assert.AreEqual(jobItem.StatusId, 5);
            Assert.AreEqual(jobItem.ErrorMessage, taskResult.ErrorMessage);
            _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void Test_SubmitTaskResultAsync_JobItemNotFound_ThrowException(){
            var taskResult = new TaskResultDto(){
                JobItemId = 2,
                TaskResult = "error task result",
                IsSuccessful = false,
                ErrorMessage = "task result error message"
            };
            
            _mockRepository.Setup(m => m.GetByIdAsync(2)).ReturnsAsync(null);

            var manager = new JobItemsManager(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object, _mockTaskDataContractMapper.Object, _mockJobItemsQueue.Object);

            Assert.ThrowsAsync<Exception>(() => {
                return manager.SubmitTaskResultAsync(taskResult);
            });
            
            _mockRepository.Verify(m => m.GetByIdAsync(2), Times.Once);
        }
    }
}
