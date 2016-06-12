using System.Threading.Tasks;

namespace KryptonAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUnitOfWorkContext GetContext<TContext>();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
