using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Data
{
    public interface IDataClient<T>
    {
        Task<List<T>> Load(CancellationToken stopToken);

        Task Publish(List<T> data, CancellationToken stopToken);

        Task PublishAs<TOut>(List<TOut> records, CancellationToken stopToken) where TOut : class;
    }
}
