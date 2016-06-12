namespace KryptonAPI.UnitOfWork
{
    public interface IUnitOfWorkContextFactory
    {
        IUnitOfWorkContext GetContext<TContext>();
    }
}
