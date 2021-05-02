using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.QueryDefinitions
{
    public interface IQueryDefinition<T>
    {
        Task<List<QueryExecutionResult>> Execute(IDataClient<T> dataClient, CancellationToken stopToken);

        QueryConfig GetConfiguration();
    }
}
