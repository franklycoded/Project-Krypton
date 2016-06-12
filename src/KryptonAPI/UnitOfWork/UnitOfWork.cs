using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KryptonAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, IUnitOfWorkContext> _contextDictionary;
        private readonly IUnitOfWorkContextFactory _unitOfWorkContextFactory;
        private readonly object _contextLock = new object();

        public UnitOfWork(IUnitOfWorkContextFactory unitOfWorkContextFactory)
        {
            _contextDictionary = new Dictionary<Type, IUnitOfWorkContext>();
            _unitOfWorkContextFactory = unitOfWorkContextFactory;
        }

        public IUnitOfWorkContext GetContext<TContext>()
        {
            lock(_contextLock){
                if(_contextDictionary.ContainsKey(typeof(TContext))){
                    var newContext = _unitOfWorkContextFactory.GetContext<TContext>();
                    _contextDictionary.Add(typeof(TContext), newContext);
                    return newContext;
                }

                return _contextDictionary[typeof(TContext)];
            }
        }

        public void SaveChanges(){
            lock(_contextLock){
                foreach (var context in _contextDictionary)
                {
                    context.Value.SaveChanges();
                }
            }
        }

        public Task SaveChangesAsync(){
            var taskList = new List<Task>();
            
            lock(_contextLock){
                foreach (var context in _contextDictionary)
                {
                    taskList.Add(Task.Factory.StartNew(() => context.Value.SaveChangesAsync()));
                }
            }

            return Task.WhenAll(taskList);
        }
    }
}
