using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KryptonAPI.UnitOfWork
{
    public interface IUnitOfWorkContext
    {
        DbContext Context { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
