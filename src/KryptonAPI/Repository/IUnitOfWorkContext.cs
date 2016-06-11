namespace KryptonAPI.Repository
{
    public interface IUnitOfWorkContext<TContext>
    {
        TContext GetContext();
        void SaveChanges();
    }
}
