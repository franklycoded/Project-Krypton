using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.Repository;
using KryptonAPI.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Repository
{
    [TestFixture]
    public class RepositoryFactoryTests
    {
        public class RepositoryFactoryTestContext {

        }
        
        public class RepositoryFactoryTestEntity : IEntity {
            public DateTime CreatedUTC { get; set; }
            public DateTime ModifiedUTC { get; set; }
            public long Id { get; set; }
        }

        private Mock<IUnitOfWorkScope> _mockUnitOfWorkScope;
        private Mock<IUnitOfWorkContext> _mockUnitOfWorkContext;

        [SetUp]
        public void Init(){
            _mockUnitOfWorkScope = new Mock<IUnitOfWorkScope>();
            _mockUnitOfWorkContext = new Mock<IUnitOfWorkContext>();
        }

        [Test]
        public void Test_NoUnitOfWorkScope_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
               var factory = new RepositoryFactory<RepositoryFactoryTestContext>(null);
            });
        }

        [Test]
        public void Test_GetContext_UnitOfWorkScopeCalled(){
            _mockUnitOfWorkScope.Setup(m => m.GetContext<RepositoryFactoryTestContext>()).Returns(_mockUnitOfWorkContext.Object);

            var factory = new RepositoryFactory<RepositoryFactoryTestContext>(_mockUnitOfWorkScope.Object);

            var repo = factory.GetRepository<RepositoryFactoryTestEntity>();

            Assert.IsTrue(repo is IRepository<RepositoryFactoryTestEntity>);
            _mockUnitOfWorkScope.Verify(m => m.GetContext<RepositoryFactoryTestContext>(), Times.Once);
        }
    }
}
