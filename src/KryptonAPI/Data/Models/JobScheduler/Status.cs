using System;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class Status : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
}
