using System;
using System.Threading.Tasks;

namespace MyHealth.Observations.Utility
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        { }
    }
}
