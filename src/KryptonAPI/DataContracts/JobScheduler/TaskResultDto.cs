using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts.JobScheduler
{
    /// <summary>
    /// Represents the result of an executed task. Related to a single JobItem
    /// </summary>
    [DataContract]
    public class TaskResultDto
    {
        [DataMember]
        public long JobItemId { get; set; }
        
        [DataMember]
        public string TaskResult { get; set; }
        
        [DataMember]
        public bool IsSuccessful { get; set; }
        
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
