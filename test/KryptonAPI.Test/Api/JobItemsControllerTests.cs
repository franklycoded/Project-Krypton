using System;
using KryptonAPI.Controllers.JobScheduler;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Service.JobScheduler;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Api
{
    [TestFixture]
    public class JobItemsControllerTests
    {
        private Mock<IJobItemsManager> _mockJobItemsManager;

        [SetUp]
        public void Init(){
            _mockJobItemsManager = new Mock<IJobItemsManager>();
        }

        [Test]
        public void Test_NoJobItemsManager_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var controller = new JobItemsController(null);
            });
        }

        [Test]
        public void Test_GetNextAsync_NextItemExists_Return_200(){
            var taskDto = new TaskDto();
            
            _mockJobItemsManager.Setup(m => m.GetNextFromQueueAsync()).ReturnsAsync(taskDto);

            var controller = new JobItemsController(_mockJobItemsManager.Object);
            var result = controller.GetNextAsync().Result;

            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreSame(((result as OkObjectResult).Value as TaskDto), taskDto);
        }

        [Test]
        public void Test_GetNextAsync_NextItemDoesntExist_Return_404(){
            _mockJobItemsManager.Setup(m => m.GetNextFromQueueAsync()).ReturnsAsync(null);

            var controller = new JobItemsController(_mockJobItemsManager.Object);
            var result = controller.GetNextAsync().Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void Test_GetNextAsync_Error_Return_500(){
            _mockJobItemsManager.Setup(m => m.GetNextFromQueueAsync()).ThrowsAsync(new Exception());

            var controller = new JobItemsController(_mockJobItemsManager.Object);
            var result = controller.GetNextAsync().Result;

            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
        }
    }
}
