using KryptonAPI.Data.Models.JobScheduler;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.Data
{
    public class KryptonAPIContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<JobItem> Tasks { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlite("Filename=./kryptonapi.db");
        }
    }
}
