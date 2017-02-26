using System.Linq;

namespace Liquid.Data
{
    public interface IDataContext
    {
        DataContextTransaction BeginTransaction();
        void CommitChanges();
        void Delete<T>(T obj) where T : IPersistentObject;
        bool HasPendingChanges();
        T Load<T>(object id) where T : IPersistentObject;
        IQueryable<T> Query<T>() where T : IPersistentObject;
        void Save(IPersistentObject obj);
        void InvalidateChanges();
    }
}
