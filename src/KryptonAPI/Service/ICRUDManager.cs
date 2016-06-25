using System.Threading.Tasks;
using KryptonAPI.Data.Models;

namespace KryptonAPI.Service
{
    /// <summary>
    /// Generic service class for CRUD operations
    /// </summary>
    public interface ICRUDManager<TEntity, TDto> where TEntity : class, IEntity 
    where TDto: class
    {
        Task<TDto> GetById(long id);
        Task<TDto> Add(TDto dto);
        Task<TDto> Update(TDto dto);
        void Delete(long id);
    }
}
