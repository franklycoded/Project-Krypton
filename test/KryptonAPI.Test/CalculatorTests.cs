using KryptonAPI.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test
{
    [TestFixture]
    public class CalculatorTests
    {
        // [TestCase(1, 1, 2)]
        // [TestCase(-1, -1, -2)]
        // [TestCase(100, 5, 105)]
        // public void CanAddNumbers(int x, int y, int expected)
        // {
        //     Assert.That(1 + 1, Is.EqualTo(2));
        // }

        [Test]
        public void MyTest(){
            var mymock = new Mock<IUnitOfWork>();

            mymock.Object.SaveChanges();

            mymock.Verify(m => m.SaveChanges(), Times.Exactly(1));
        }
    }
}