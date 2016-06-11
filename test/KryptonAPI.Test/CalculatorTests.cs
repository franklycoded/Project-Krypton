using NUnit.Framework;

namespace KryptonAPI.Test
{
    [TestFixture]
    public class CalculatorTests
    {
        [TestCase(1, 1, 2)]
        [TestCase(-1, -1, -2)]
        [TestCase(100, 5, 105)]
        public void CanAddNumbers(int x, int y, int expected)
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}