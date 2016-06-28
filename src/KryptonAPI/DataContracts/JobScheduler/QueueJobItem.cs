namespace KryptonAPI.DataContracts.JobScheduler
{
    public class QueueJobItem
    {
        public long Id { get; private set; }
        
        public QueueJobItem(long id)
        {
            Id = id;
        }
    }
}
