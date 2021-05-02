using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Gateways;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Services
{
    public class DataAnalyzerService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly Config _options;

        public DataAnalyzerService(
            IOptions<Config> options,
            ILogger<DataAnalyzerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Begin {nameof(ExecuteAsync)} execution");

            _logger.LogInformation("DataAnalyzerService running at: {time}", DateTimeOffset.Now);

            try
            {
                var client = new CompletedOrderProductsClient(_logger, _options.Analyzers.First());

                await client.Run(stoppingToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Unhadled exception in {nameof(DataAnalyzerService)}.{nameof(StartAsync)}");
            }

            _logger.LogInformation($"End {nameof(ExecuteAsync)} execution");
        }
    }
}
