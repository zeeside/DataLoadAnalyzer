using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Data
{
    public class ElasticSearchClient<T> : IDataClient<T> where T : class
    {
        public readonly ElasticClient Instance;
        private readonly int _publishBatchSize;

        public ElasticSearchClient(ILogger logger, DataConfiguration settings)
        {
            Check.IsNotNull<ILogger>(logger);
            Check.IsNotNull<List<ConnectionKey>>(settings?.ConfigDictionary, $"Invalid configs for {nameof(ElasticSearchClient<T>)}");

            var keys = settings.ConfigDictionary;

            if (!keys.Any(k => k.Key == Constants.ElasticSearchUrl)) throw new ArgumentNullException($"Missing config key \"Url\" for {nameof(ElasticSearchClient<T>)}");
            if (!keys.Any(k => k.Key == Constants.ElastiSearchTopic)) throw new ArgumentNullException($"Missing config key \"Topic\" for {nameof(ElasticSearchClient<T>)}");

            var url = keys.Single(k => k.Key == Constants.ElasticSearchUrl).Value;
            var index = keys.Single(k => k.Key == Constants.ElastiSearchTopic).Value;
            
            _publishBatchSize =10000;

            if (Int32.TryParse( keys.FirstOrDefault(k => k.Key == Constants.ElastiSearchPublishBatchSize).Value, out int batchSize))
            {
                _publishBatchSize = batchSize;
            }

            Check.IsNotNullOrEmpty(url);
            Check.IsNotNullOrEmpty(index);

            logger.LogInformation($"Begin initializing elasticsearch client with url {url} and index {index}");

            var ecSettings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);

            Instance = new ElasticClient(ecSettings);           
        }

        

        public Task<List<T>> Load(CancellationToken stopToken)
        {
            throw new NotImplementedException();
        }

        public async Task Publish(List<T> records, CancellationToken stopToken) 
        {
            foreach (var batch in records.Batch(_publishBatchSize))
            {
                await Instance.BulkAsync(b => b.IndexMany(batch), stopToken);
            }
        }

        public async Task PublishAs<TOut>(List<TOut> records, CancellationToken stopToken) where TOut : class
        {
            foreach (var batch in records.Batch(_publishBatchSize))
            {
                await Instance.BulkAsync(b => b.IndexMany(batch), stopToken);
            }
        }
    }
}
