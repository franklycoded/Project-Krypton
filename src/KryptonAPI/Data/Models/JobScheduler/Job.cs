using System;
using System.Collections.Generic;
using Crud.Net.Core.DataModel;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class Job : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public byte[] FinalResult { get; set; }
        public List<JobItem> JobItems { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
}
