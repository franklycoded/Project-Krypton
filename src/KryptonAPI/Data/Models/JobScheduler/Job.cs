using System;
using System.Collections.Generic;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class Job : IEntity
    {
        public long JobId { get; set; }
        public long UserId { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public byte[] FinalResult { get; set; }
        public List<JobItem> JobItems { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }

        public long Id
        {
            get
            {
                return this.JobId;
            }
        }
    }
}
