using System;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class JobItem : IEntity
    {
        public long JobItemId { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public string JsonResult { get; set; }
        public long JobId { get; set; }
        public Job Job { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }

        public long Id
        {
            get
            {
                return JobItemId;
            }
        }
    }
}
