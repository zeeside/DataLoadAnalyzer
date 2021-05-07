using DataLoadAnalyzer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using DataLoadAnalyzer.Common;
using Microsoft.Extensions.Logging;

namespace DataLoadAnalyzer.Data
{
    public class SqlServerClient<T> : IDataClient<T>
    {
        public readonly string ConnectionString;

        public SqlServerClient(ILogger logger, DataConfiguration settings)
        {
            Check.IsNotNull<ILogger>(logger);
            Check.IsNotNull<List<ConnectionKey>>(settings?.ConfigDictionary, $"Invalid configs for {nameof(SqlServerClient<T>)}");

            var keys = settings.ConfigDictionary;

            if (!keys.Any(k => k.Key == Constants.SqlConnectionKey)) throw new ArgumentNullException($"Missing config key \"Url\" for {nameof(SqlServerClient<T>)}");

            ConnectionString = keys.Single(k => k.Key == Constants.SqlConnectionKey).Value;
        }

        public async Task<List<T>> Load(CancellationToken stopToken)
        {
            throw new NotImplementedException();
        }

        public Task Publish(List<T> data, CancellationToken stopToken)
        {
            throw new NotImplementedException();
        }

        public Task PublishAs<TOut>(List<TOut> records, CancellationToken stopToken) where TOut : class
        {
            throw new NotImplementedException();
        }
    }
}
