using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Data
{
    public interface IDataClient<T>
    {
        Task<List<T>> Load(CancellationToken stopToken);

        Task Publish(List<T> data, CancellationToken stopToken);
    }
}
