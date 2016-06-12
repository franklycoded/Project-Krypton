using System;
using KryptonAPI.Data;

namespace KryptonAPI.UnitOfWork
{
    public class UnitOfWorkContextFactory : IUnitOfWorkContextFactory
    {
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
