using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.DataContractMappers;
using KryptonAPI.Repository;
using KryptonAPI.Service;
using KryptonAPI.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Service
{
    public class TestEntity : IEntity
    {
        public long Id
        {
            get
            {
                return 1;
            }
        }
    }

    public class TestDto {

    }

    public class TestContext {

    }
    
    [TestFixture]
    public class CRUDManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<TestEntity>> _mockRepository;
        private Mock<IDataContractMapper<TestEntity, TestDto>> _mockDataContractMapper;
        
        [SetUp]
        public void Init(){
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IRepository<TestEntity>>();
            _mockDataContractMapper = new Mock<IDataContractMapper<TestEntity, TestDto>>();
        }

        [Test]
        public void Test_NoUnitofWork_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new CRUDManager<TestContext, TestEntity, TestDto>(null, _mockRepository.Object, _mockDataContractMapper.Object);
            });
        }

        [Test]
        public void Test_NoRepository_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new CRUDManager<TestContext, TestEntity, TestDto>(_mockUnitOfWork.Object, null, _mockDataContractMapper.Object);
            });
        }

        [Test]
        public void Test_NoDtoMapper_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var manager = new CRUDManager<TestContext, TestEntity, TestDto>(_mockUnitOfWork.Object, _mockRepository.Object, null);
            });
        }

        [Test]
        public void Test_GetById_EntityNotFound_ReturnNull(){
            _mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null);

            var manager = new CRUDManager<TestContext, TestEntity, TestDto>(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object);

            var result = manager.GetById(1);

            Assert.IsNull(result.Result);
        }

        [Test]
        public void Test_GetById_EntityFound_Mapped(){
            var testEntity = new TestEntity();
            var testDto = new TestDto();

            _mockRepository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(testEntity);

            _mockDataContractMapper.Setup(m => m.MapEntityToDto(testEntity)).Returns(testDto);

            var manager = new CRUDManager<TestContext, TestEntity, TestDto>(_mockUnitOfWork.Object, _mockRepository.Object, _mockDataContractMapper.Object);

            var result = manager.GetById(1);

            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result == testDto);
            _mockDataContractMapper.Verify(m => m.MapEntityToDto(testEntity), Times.Once);
        }
    }
}
