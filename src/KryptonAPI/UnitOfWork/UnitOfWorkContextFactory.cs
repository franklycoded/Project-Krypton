using System;
using Crud.Net.EntityFramework.UnitOfWork;
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
            if(typeof(TContext) == typeof(KryptonAPIContext)){
                return new UnitOfWorkContext(new KryptonAPIContext());
            }

            throw new Exception("Can't create UnitOfWorkContext for context type " + typeof(TContext).ToString());
        }
    }
}
