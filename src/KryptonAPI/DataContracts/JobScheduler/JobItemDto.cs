using System;
using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts.JobScheduler
{
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
        public long JobId { get; set; }
        
        [DataMember]
        public DateTime CreatedUTC { get; set; }
        
        [DataMember]
        public DateTime ModifiedUTC { get; set; }
    }
}
