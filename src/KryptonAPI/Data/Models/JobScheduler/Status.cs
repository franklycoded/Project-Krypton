using System;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class Status : IEntity
    {
        public long StatusId { get; set; }
        public string Name { get; set; }

        public long Id
        {
            get
            {
                return StatusId;
            }
        }
    }
}
