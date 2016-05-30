using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts
{
    [DataContract]
    public class JobResultDto
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public string Status { get; set; }
        
        [DataMember]
        public object Result { get; set; }
    }
}
