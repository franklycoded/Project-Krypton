using System;
using KryptonAPI.Controllers;
using KryptonAPI.Data.Models;
using KryptonAPI.DataContracts;
using KryptonAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Api
{
    public class CRUDEntity : IEntity {
        public long Id { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }

    public class CRUDDto : ICRUDDto {
        public long Id { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
    
    [TestFixture]
    public class CRUDControllerTests
    {
        private Mock<ICRUDManager<CRUDEntity, CRUDDto>> _mockCRUDManager;

        [SetUp]
        public void Init(){
            _mockCRUDManager = new Mock<ICRUDManager<CRUDEntity, CRUDDto>>();
        }

        [Test]
        public void Test_NoCRUDManager_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var controller = new CRUDController<CRUDEntity, CRUDDto>(null);
            });
        }

        [Test]
        public void Test_GetById_NotFound_Return_404(){
            _mockCRUDManager.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(null);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.GetById(1).Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
            _mockCRUDManager.Verify(m => m.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public void Test_GetById_Found_Return_200(){
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(testDto);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.GetById(1).Result;

            Assert.IsInstanceOf(typeof(OkObjectResult), result); 
            _mockCRUDManager.Verify(m => m.GetByIdAsync(1), Times.Once);       
        }

        [Test]
        public void Test_GetById_Error_Return_500(){
            _mockCRUDManager.Setup(m => m.GetByIdAsync(1)).ThrowsAsync(new Exception());

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.GetById(1).Result;

            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
            _mockCRUDManager.Verify(m => m.GetByIdAsync(1), Times.Once);    
        }

        [Test]
        public void Test_Post_Success_Return_200(){
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.AddAsync(testDto)).ReturnsAsync(testDto);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Post(testDto).Result;

            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            _mockCRUDManager.Verify(m => m.AddAsync(testDto), Times.Once);
        }

        [Test]
        public void Test_Post_Error_Return_500(){
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.AddAsync(testDto)).ThrowsAsync(new Exception());

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Post(testDto).Result;

            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
            _mockCRUDManager.Verify(m => m.AddAsync(testDto), Times.Once);
        }

        [Test]
        public void Test_Put_Success_Return_200(){
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.UpdateAsync(testDto)).ReturnsAsync(testDto);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Put(testDto).Result;

            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            _mockCRUDManager.Verify(m => m.UpdateAsync(testDto), Times.Once);
        }

        [Test]
        public void Test_Put_NotFound_Return_404()
        {
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.UpdateAsync(testDto)).ReturnsAsync(null);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Put(testDto).Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
            _mockCRUDManager.Verify(m => m.UpdateAsync(testDto), Times.Once);
        }

        [Test]
        public void Test_Put_Error_Return_500(){
            var testDto = new CRUDDto(){
                Id = 1
            };

            _mockCRUDManager.Setup(m => m.UpdateAsync(testDto)).ThrowsAsync(new Exception());

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Put(testDto).Result;

            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
            _mockCRUDManager.Verify(m => m.UpdateAsync(testDto), Times.Once);
        }

        [Test]
        public void Test_Delete_Success_Return_200(){
            _mockCRUDManager.Setup(m => m.DeleteAsync(1)).ReturnsAsync(true);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Delete(1).Result;

            Assert.IsInstanceOf(typeof(OkResult), result);
            _mockCRUDManager.Verify(m => m.DeleteAsync(1), Times.Once);
        }

        [Test]
        public void Test_Delete_NotFound_Return_404()
        {
            _mockCRUDManager.Setup(m => m.DeleteAsync(1)).ReturnsAsync(false);

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Delete(1).Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
            _mockCRUDManager.Verify(m => m.DeleteAsync(1), Times.Once);
        }

        [Test]
        public void Test_Delete_Error_Return_500(){
            _mockCRUDManager.Setup(m => m.DeleteAsync(1)).ThrowsAsync(new Exception());

            var controller = new CRUDController<CRUDEntity, CRUDDto>(_mockCRUDManager.Object);

            var result = controller.Delete(1).Result;
            
            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
            _mockCRUDManager.Verify(m => m.DeleteAsync(1), Times.Once);
        }
    }
}
