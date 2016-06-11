using System;
using System.Collections.Generic;

namespace KryptonAPI.Models.JobScheduler
{
    public class Job
    {
        public long JobId { get; set; }
        public long UserId { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public object FinalResult { get; set; }
        public List<Task> Tasks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
}
