using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KryptonAPI.UnitOfWork
{
    /// <summary>
    /// Class to cache and manage the lifetime of resolved UnitOfWorkContexts
    /// </summary>
    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        private readonly Dictionary<Type, IUnitOfWorkContext> _contextDictionary;
        private readonly IUnitOfWorkContextFactory _unitOfWorkContextFactory;
        private readonly object _contextLock = new object();

        /// <summary>
        /// Creates a new instance of the UnitOfWork context cache
        /// </summary>
        /// <param name="unitOfWorkContextFactory">The factory to be used to create UnitOfWorkContexts</param>
        public UnitOfWorkScope(IUnitOfWorkContextFactory unitOfWorkContextFactory)
        {
            if(unitOfWorkContextFactory == null) throw new ArgumentNullException(nameof(UnitOfWorkContextFactory));
            
            _contextDictionary = new Dictionary<Type, IUnitOfWorkContext>();
            _unitOfWorkContextFactory = unitOfWorkContextFactory;
        }

        /// <summary>
        /// <see cref="IUnitOfWork.GetContext" />
        /// </summary>
        public IUnitOfWorkContext GetContext<TContext>()
        {
            lock(_contextLock){
                if(!_contextDictionary.ContainsKey(typeof(TContext))){
                    var newContext = _unitOfWorkContextFactory.GetContext<TContext>();
                    _contextDictionary.Add(typeof(TContext), newContext);
                    return newContext;
                }
                
                return _contextDictionary[typeof(TContext)];
            }
        }

        /// <summary>
        /// <see cref="IUnitOfWork.SaveChanges" />
        /// </summary>
        public void SaveChanges(){
            lock(_contextLock){
                foreach (var context in _contextDictionary)
                {
                    context.Value.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <see cref="IUnitOfWork.SaveChangesAsync" />
        /// </summary>
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_contextDictionary != null){
                        foreach (var unityContext in _contextDictionary)
                        {
                            unityContext.Value.Context.Dispose();
                        }

                        _contextDictionary.Clear();
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
