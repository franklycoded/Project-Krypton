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

        
    }
}
