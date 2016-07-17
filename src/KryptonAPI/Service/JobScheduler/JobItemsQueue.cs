using System;
using System.Text;
using KryptonAPI.Configuration;
using KryptonAPI.DataContracts.JobScheduler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace KryptonAPI.Service.JobScheduler
{
    /// <summary>
    /// <see cref="IJobItemsQueue" />
    /// </summary>
    public class JobItemsQueue : IJobItemsQueue
    {
        private readonly JobItemsQueueConfiguration _queueConfiguration;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger _logger;
        private IConnection _connection;

        /// <summary>
        /// Creates a new instance of the JobItemsQueue class
        /// </summary>
        /// <param name="queueConfiguration">The configuration to use for setting up the queue connection</param>
        public JobItemsQueue(IOptions<JobItemsQueueConfiguration> queueConfiguration, ILoggerFactory loggerFactory)
        {
            if(queueConfiguration == null) throw new ArgumentNullException(nameof(queueConfiguration));

            if(loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger(typeof(JobItemsQueue));

            if(string.IsNullOrWhiteSpace(queueConfiguration.Value.Hostname)) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.Hostname), "The hostname of the queue connection cannot be empty!");

            if(queueConfiguration.Value.Port == 0) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.Port), "The port number of the queue connection cannot be 0!");

            if(string.IsNullOrWhiteSpace(queueConfiguration.Value.QueueName)) throw new ArgumentOutOfRangeException(nameof(queueConfiguration.Value.QueueName), "The name of the queue cannot be empty!");

            _queueConfiguration = queueConfiguration.Value;

            // Creating connection
            _connectionFactory = new ConnectionFactory() { 
                HostName = _queueConfiguration.Hostname, 
                Port = _queueConfiguration.Port 
            };

            _connection = _connectionFactory.CreateConnection();

            // Declaring queue
            using(var model = _connection.CreateModel()){
                model.QueueDeclare(queue: _queueConfiguration.QueueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                
                model.CreateBasicProperties().Persistent = true;
            }
        }

        /// <summary>
        /// <see cref="IJobItemsQueue.Add" />
        /// </summary>
        public bool Add(QueueJobItem jobItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IJobItemsQueue.GetNext" />
        /// </summary>
        public QueueJobItem GetNext()
        {
            using(var model = _connection.CreateModel())
            {
                var serialisedQueueItem = model.BasicGet(_queueConfiguration.QueueName, true);
                
                if(serialisedQueueItem == null) return null;

                var jsonString = Encoding.UTF8.GetString(serialisedQueueItem.Body);
                var queueItem = JsonConvert.DeserializeObject<QueueJobItem>(jsonString);
                
                if(queueItem == null) {
                    _logger.LogError("Error while deserialising item in queue: " + jsonString);
                    return null;
                }

                return queueItem;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_connection != null){
                        _connection.Dispose();
                    }
                }

                _connection = null;

                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
