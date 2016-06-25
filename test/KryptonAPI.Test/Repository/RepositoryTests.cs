using System;
using KryptonAPI.Data.Models;
using KryptonAPI.Repository;
using NUnit.Framework;

namespace KryptonAPI.Test.Repository
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void Test_ContextNull_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var repo = new Repository<IEntity>(null);
            });
        }
    }
}
