using System;
using System.Runtime.Serialization;

namespace KryptonAPI.DataContracts
{
    public interface ICRUDDto
    {
        [DataMember]
        long Id { get; set; }
        
        [DataMember]
        DateTime CreatedUTC { get; set; }
        
        [DataMember]
        DateTime ModifiedUTC { get; set; }
    }
}
