using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Data;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Gateways
{
    public class CompletedOrderProductsClient : AnalyzerClientBase<CompletedOrderProductDto>
    {
        private const int IndexBatchSize = 10000;
        public CompletedOrderProductsClient(
            ILogger logger,
            AnalyzerSettings settings) : base(logger, settings){}               

        public override async Task ExecuteMigration(CancellationToken stopToken)
        {
            if (!Settings.SkipMigration)
            {
                Logger.LogInformation($"Begin migration in {nameof(CompletedOrderProductsClient)}.{nameof(ExecuteMigration)}");

                var loader = (CsvClient<CompletedOrderProductDto>)this.DataLoader;

                var publisher = (ElasticSearchClient<CompletedOrderProductDto>)this.DataPublisher;

                var publisherContainsData = await PublisherContainsData();

                if (!(Settings.SkipMigrationIfDataExists && publisherContainsData) )
                {
                    var records = await loader.Load(stopToken);

                    Logger.LogInformation($"{records.Count} records loaded from CSV. Begin publish to Elastic Search");

                    await publisher.Publish(records, stopToken);
                }
                else
                {
                    Logger.LogInformation("Data already exiists on publisher. Skip migration");
                }                
            }
            else
            {
                Logger.LogInformation("Skip migration configured");
            }
        }
        
        private async Task<bool> PublisherContainsData()
        {
            bool dataExistsOnPublisher = false;

            try
            {
                dataExistsOnPublisher = (await ((ElasticSearchClient<CompletedOrderProductDto>)this.DataPublisher).Instance.CountAsync<CompletedOrderProductDto>(s => s.Query(q => q.MatchAll()))).Count > 0;
            }
            catch
            {
                dataExistsOnPublisher = false;
            }

            return dataExistsOnPublisher;
        }
    }
}
