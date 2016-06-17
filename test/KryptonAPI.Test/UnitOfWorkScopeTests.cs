using System;
using System.Threading;
using System.Threading.Tasks;
using KryptonAPI.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test
{
    [TestFixture]
    public class UnitOfWorkScopeTests
    {
        private Mock<IUnitOfWorkContextFactory> _mockUnitOfWorkContextFactory;
        
        [SetUp]
        public void InitTest(){
            _mockUnitOfWorkContextFactory = new Mock<IUnitOfWorkContextFactory>();
        }
        
        [Test]
        public void Test_NoUnitOfWorkContextFactory_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var unitOfWork = new UnitOfWorkScope(null);
            });
        }

        [Test]
        public void Test_GetContext_SameTypeCreatedOnlyOnce(){
            var testContext1 = new Mock<IUnitOfWorkContext>();
            var testContext2 = new Mock<IUnitOfWorkContext>();

            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<bool>()).Returns(testContext1.Object);
            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<int>()).Returns(testContext2.Object);

            var uow = new UnitOfWorkScope(_mockUnitOfWorkContextFactory.Object);

            uow.GetContext<bool>();
            uow.GetContext<int>();
            uow.GetContext<int>();
            uow.GetContext<bool>();

            _mockUnitOfWorkContextFactory.Verify(m => m.GetContext<bool>(), Times.Once);
            _mockUnitOfWorkContextFactory.Verify(m => m.GetContext<int>(), Times.Once);
        }

        [Test]
        public void Test_TwoContexts_SaveChanges_CalledOnEachContext(){
            var testContext1 = new Mock<IUnitOfWorkContext>();
            var testContext2 = new Mock<IUnitOfWorkContext>();

            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<bool>()).Returns(testContext1.Object);
            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<int>()).Returns(testContext2.Object);

            var uow = new UnitOfWorkScope(_mockUnitOfWorkContextFactory.Object);

            uow.GetContext<bool>();
            uow.GetContext<int>();

            uow.SaveChanges();

            testContext1.Verify(c => c.SaveChanges(), Times.Once);
            testContext2.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void Test_TwoContexts_SaveChangesAsync_CalledOnEachContext(){
            var testContext1 = new Mock<IUnitOfWorkContext>();
            var testContext2 = new Mock<IUnitOfWorkContext>();

            testContext1.Setup(m => m.SaveChangesAsync()).Returns(Task.Factory.StartNew(() => {
            }));
            testContext2.Setup(m => m.SaveChangesAsync()).Returns(Task.Factory.StartNew(() => {
            }));

            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<bool>()).Returns(testContext1.Object);
            _mockUnitOfWorkContextFactory.Setup(m => m.GetContext<int>()).Returns(testContext2.Object);

            var uow = new UnitOfWorkScope(_mockUnitOfWorkContextFactory.Object);

            uow.GetContext<bool>();
            uow.GetContext<int>();

            uow.SaveChangesAsync().Wait();

            testContext1.Verify(c => c.SaveChangesAsync(), Times.Once);
            testContext2.Verify(c => c.SaveChangesAsync(), Times.Once);
        }
    }
}