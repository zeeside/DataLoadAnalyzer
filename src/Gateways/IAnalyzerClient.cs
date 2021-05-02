using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Data;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Gateways
{
    public interface IAnalyzerClient<T>
    {
        IDataClient<T> GetDataLoader(DataConfiguration source);
        IDataClient<T> GetDataPublisher(DataConfiguration dest);
        List<IQueryDefinition<T>> GetQueryDefinitions(List<QueryConfig> queryConfigs);
        Task Run(CancellationToken stopToken);
        
    }
}
