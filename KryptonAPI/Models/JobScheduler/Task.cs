using System;

namespace KryptonAPI.Models.JobScheduler
{
    public class Task
    {
        public long TaskId { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public object Result { get; set; }
        public long JobId { get; set; }
        public Job Job { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
}
