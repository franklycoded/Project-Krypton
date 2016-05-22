using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts
{
    [DataContract]
    public class JobDto
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public string Status { get; set; }
        
        [DataMember]
        public string Task { get; set; }
        
        [DataMember]
        public object Result { get; set; }
    }
}
