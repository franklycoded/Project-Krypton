using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data;
using KryptonAPI.Data.Models.JobScheduler;

namespace KryptonAPI.Repository.JobScheduler
{
    public class JobItemsRepository : IRepository<JobItem>
    {
        public JobItemsRepository()
        {
            var context = new KryptonAPIContext();
            
        }

        public JobItem Add(JobItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public JobItem GetById(long id)
        {
            throw new NotImplementedException();
        }

        public JobItem Update(long id, JobItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
