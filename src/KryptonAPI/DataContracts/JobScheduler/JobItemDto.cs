using System;
using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts.JobScheduler
{
    /// <summary>
    /// Data transfer object representing a JobItem entity.
    /// </summary>
    [DataContract]
    public class JobItemDto : ICRUDDto
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public long StatusId { get; set; }
        
        [DataMember]
        public string JsonResult { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string JsonData { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
        
        [DataMember]
        public long JobId { get; set; }
        
        [DataMember]
        public DateTime CreatedUTC { get; set; }
        
        [DataMember]
        public DateTime ModifiedUTC { get; set; }
    }
}
