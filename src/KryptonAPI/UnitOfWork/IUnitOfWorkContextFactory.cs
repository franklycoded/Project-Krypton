namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// Factory to create IUnitOfWorkContext-s
    /// </summary>
    public interface IUnitOfWorkContextFactory
    {
        /// <summary>
        /// Creates or retrieves a cached IUnitOfWorkContext of the specified type
        /// </summary>
        /// <returns>The IUnitOfWorkContext of the specified type</returns>
        IUnitOfWorkContext GetContext<TContext>();
    }
}
