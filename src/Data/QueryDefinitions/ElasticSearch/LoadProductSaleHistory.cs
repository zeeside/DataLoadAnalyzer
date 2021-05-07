using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Models;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Data.QueryDefinitions.ElasticSearch
{
    public class LoadProductSaleHistory : IQueryDefinition<ProductSaleHistoryInput>
    {
        private readonly QueryConfig _config;
        private readonly ILogger _logger;

        public LoadProductSaleHistory(QueryConfig config, ILogger logger)
        {
            Check.IsNotNull<QueryConfig>(config);
            Check.IsNotNull<ILogger>(logger);

            _config = config;
            _logger = logger;
        }        

        public QueryConfig GetConfiguration()
        {
            return _config;
        }


        public async Task<List<QueryExecutionResult>> Execute(IDataClient<ProductSaleHistoryInput> dataClient, CancellationToken stopToken)
        {
            var clientWrapper  = (ElasticSearchClient<ProductSaleHistoryInput>)dataClient;
            var output = new List<QueryExecutionResult>();

            output.Add( await ExecuteQuery(nameof(FetchCountForAllProducts), "Fetch count of all records", clientWrapper.Instance, FetchCountForAllProducts));
            output.Add( await ExecuteQuery(nameof(FetchRecordsForProductId_4693843), "Fetch all records for single productId (4693843)", clientWrapper.Instance, FetchRecordsForProductId_4693843));
            output.Add( await ExecuteQuery(nameof(FetchAveragePriceForProductId_4693843), "Fetch average price for single productId (4693843)", clientWrapper.Instance, FetchAveragePriceForProductId_4693843));
            output.Add( await ExecuteQuery(nameof(FetchAveragePriceForAllProducts), "Fetch average price for all products", clientWrapper.Instance, FetchAveragePriceForAllProducts));

            return output;
        }

        private async Task<QueryExecutionResult> ExecuteQuery(string name, string description, ElasticClient client, Func<ElasticClient, Task<long>> queryFunction)
        {
            _logger.LogInformation($"Begin executing query with  name {name}");

            var result = new QueryExecutionResult { Name = name, Description = description };

            for (var i = 0; i < _config.Iterations; i++)
            {
                var metrics = new QueryExecutionMetrics { Id = i + 1, Name=name };

                var iterationSw = new Stopwatch();

                iterationSw.Start();

                metrics.RecordsFetched = await queryFunction(client);

                iterationSw.Stop();

                metrics.Duration = iterationSw.ElapsedMilliseconds;

                result.Metrics.Add(metrics);
            }

            return result;
        }        



        private async Task<long> FetchCountForAllProducts(ElasticClient client)
        {
            var response = await client.CountAsync<ProductSaleHistoryInput>(s => s.Query(q => q.MatchAll()));

            return response.Count;
        }

        private async Task<long> FetchRecordsForProductId_4693843(ElasticClient client)
        {
            var response = await client.SearchAsync<ProductSaleHistoryInput>(s => s.Query(q => q.Match(m => m.Field("productId").Query("4693843"))));

            return response.Total;
        }

        private async Task<long> FetchAveragePriceForProductId_4693843(ElasticClient client)
        {
            var response = await client.SearchAsync<ProductSaleHistoryInput>(d => d
                .Query(q => q.Match(m => m.Field("productId").Query("4693843")))
                .Aggregations( aggs => aggs
                .Average("AveragePrice", avg => avg.Field(p => p.PurchasePrice)))
            );

            return response.Total;
            
        }
        
        private async Task<long> FetchAveragePriceForAllProducts(ElasticClient client)
        {
            var response = await client.SearchAsync<ProductSaleHistoryInput>(d => d
                .Aggregations( aggs => aggs
                .Average("AveragePrice", avg => avg.Field(p => p.PurchasePrice)))
            );

            return response.Total;
            
        }

        

       
    }
}
