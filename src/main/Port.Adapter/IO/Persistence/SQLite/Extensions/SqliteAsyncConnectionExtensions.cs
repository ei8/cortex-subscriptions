using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Linq.Expressions;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Extensions
{
    public static class SqliteAsyncConnectionExtensions
    {
        public static Task<List<T>> GetAllWithChildren<T>(this SQLiteAsyncConnection conn, Expression<Func<T, bool>> filter = null, bool recursive = false) where T : new()
        {
            return Task.Factory.StartNew(delegate
            {
                var internalConnection = conn.GetConnection();

                using (internalConnection.Lock())
                {
                    return internalConnection.GetAllWithChildren(filter, recursive);
                }
            }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }
    }
}
