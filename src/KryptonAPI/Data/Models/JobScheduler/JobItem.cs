using System;
using Crud.Net.Core.DataModel;

namespace KryptonAPI.Data.Models.JobScheduler
{
    public class JobItem : IEntity
    {
        public long Id { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public string JsonResult { get; set; }
        public string ErrorMessage { get; set; }
        public string Code { get; set; }
        public string JsonData { get; set; }
        public long JobId { get; set; }
        public Job Job { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ModifiedUTC { get; set; }
    }
}
