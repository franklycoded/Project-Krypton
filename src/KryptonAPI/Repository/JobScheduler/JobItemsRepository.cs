using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.UnitOfWork;

namespace KryptonAPI.Repository.JobScheduler
{
    public class JobItemsRepository : Repository<JobItem>, IJobItemsRepository
    {
        public JobItemsRepository(IUnitOfWorkContext unitOfWorkContext) : base(unitOfWorkContext)
        {
        
        }

        public void TestOperation()
        {
            throw new NotImplementedException();
        }
    }
}
