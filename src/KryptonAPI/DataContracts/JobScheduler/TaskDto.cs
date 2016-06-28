using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts.JobScheduler
{
    /// <summary>
    /// Represents a task to be executed. Related to a single Job Item.
    /// </summary>
    [DataContract]
    public class TaskDto
    {
        [DataMember]
        public long JobItemId { get; set; }
        
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string JsonData { get; set; }

        [DataMember]
        public string JsonResult { get; set; }
    }
}
