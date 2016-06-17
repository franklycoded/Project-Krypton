using System;
using KryptonAPI.Data;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// <see cref="IUnitOfWorkContextFactory" />
    /// </summary>
    public class UnitOfWorkContextFactory : IUnitOfWorkContextFactory
    {
        /// <summary>
        /// <see cref="IUnitOfWorkContextFactory.GetContext" />
        /// </summary>
        public IUnitOfWorkContext GetContext<TContext>()
        {
            switch (typeof(TContext).ToString()){
                case "KryptonAPIContext":
                    return new UnitOfWorkContext(new KryptonAPIContext());
                default:
                    throw new Exception("Can't create UnitOfWorkContext for context type " + typeof(TContext).ToString());
            }
        }
    }
}
