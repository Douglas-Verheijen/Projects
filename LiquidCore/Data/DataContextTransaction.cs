using System;

namespace Liquid.Data
{
    public class DataContextTransaction : IDisposable
    {
        private readonly IDataContext _dataContext;

        public DataContextTransaction(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Dispose()
        {
            if (_dataContext.HasPendingChanges())
                _dataContext.CommitChanges();
        }
    }
}
