namespace KryptonAPI.Configuration
{
    /// <summary>
    /// Configuration class for the JobItemsQueue class
    /// </summary>
    public class JobItemsQueueConfiguration
    {
        /// <summary>
        /// Gets or sets the hostname
        /// </summary>
        public string Hostname { get; set; }
        
        /// <summary>
        /// Gets or sets the port
        /// </summary>
        public int Port { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier of the queue
        /// </summary>
        public string QueueName { get; set; }
    }
}
