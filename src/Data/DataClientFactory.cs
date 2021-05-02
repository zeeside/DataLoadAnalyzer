using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace DataLoadAnalyzer.Data
{
    public class DataClientFactory
    {
        public static IDataClient<T> Create<T>(ILogger logger, DataConfiguration config) where T : class
        {
            Check.IsNotNull<ILogger>(logger);
            Check.IsNotNull<DataConfiguration>(config);

            logger.LogInformation($"Begin new data-client creation of type {config.Type}");

            return config.Type.ToLower() switch
            {
                "elasticsearch" => new ElasticSearchClient<T>(logger, config),
                "mssql"         => new SqlServerClient<T>(logger, config),
                "csv"           => new CsvClient<T>(logger, config),
                 _              => throw new ArgumentException("Invalid DataSource Type")

            };
        }
    }
}
