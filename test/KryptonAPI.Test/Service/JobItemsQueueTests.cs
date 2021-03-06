using System;
using KryptonAPI.Configuration;
using KryptonAPI.Service.JobScheduler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace KryptonAPI.Test.Service
{
    [TestFixture]
    public class JobItemsQueueTests
    {
        private Mock<IOptions<JobItemsQueueConfiguration>> _mockJobItemsQueueConfiguraiton;
        private Mock<ILoggerFactory> _mockLoggerFactory;

        [SetUp]
        public void Init(){
            _mockJobItemsQueueConfiguraiton = new Mock<IOptions<JobItemsQueueConfiguration>>();
            _mockLoggerFactory = new Mock<ILoggerFactory>();
        }

        [Test]
        public void Test_NoQueueConfigurationProvided_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var jobItemsQueue = new JobItemsQueue(null, _mockLoggerFactory.Object);
            });
        }

        [Test]
        public void Test_NoLoggerFactoryProvided_ArgumentNullException(){
            Assert.Throws<ArgumentNullException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, null);
            });
        }

        [Test]
        public void Test_HostnameNull_ArgumentOutOfRangeException(){
            var config = new JobItemsQueueConfiguration();
            config.Hostname = null;
            config.Port = 5000;
            config.QueueName = "test_queue";
            
            _mockJobItemsQueueConfiguraiton.SetupGet(m => m.Value).Returns(config);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, _mockLoggerFactory.Object);
            });
        }

        [Test]
        public void Test_HostnameEmpty_ArgumentOutOfRangeException(){
            var config = new JobItemsQueueConfiguration();
            config.Hostname = " ";
            config.Port = 5000;
            config.QueueName = "test_queue";
            
            _mockJobItemsQueueConfiguraiton.SetupGet(m => m.Value).Returns(config);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, _mockLoggerFactory.Object);
            });
        }

        [Test]
        public void Test_PortZero_ArgumentOutOfRangeException(){
            var config = new JobItemsQueueConfiguration();
            config.Hostname = "testhost";
            config.Port = 0;
            config.QueueName = "test_queue";
            
            _mockJobItemsQueueConfiguraiton.SetupGet(m => m.Value).Returns(config);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, _mockLoggerFactory.Object);
            });
        }

        [Test]
        public void Test_QueueNameNull_ArgumentOutOfRangeException(){
            var config = new JobItemsQueueConfiguration();
            config.Hostname = "testhost";
            config.Port = 5000;
            config.QueueName = null;
            
            _mockJobItemsQueueConfiguraiton.SetupGet(m => m.Value).Returns(config);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, _mockLoggerFactory.Object);
            });
        }

        [Test]
        public void Test_QueueNameEmpty_ArgumentOutOfRangeException(){
            var config = new JobItemsQueueConfiguration();
            config.Hostname = "testhost";
            config.Port = 5000;
            config.QueueName = " ";
            
            _mockJobItemsQueueConfiguraiton.SetupGet(m => m.Value).Returns(config);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var jobItemsQueue = new JobItemsQueue(_mockJobItemsQueueConfiguraiton.Object, _mockLoggerFactory.Object);
            });
        }
    }
}
