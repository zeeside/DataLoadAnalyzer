using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Data;
using DataLoadAnalyzer.Data.ReportGenerators;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Gateways
{
    public abstract class AnalyzerClientBase<T> : IAnalyzerClient<T> where T : class
    {
        protected readonly ILogger Logger;
        protected readonly AnalyzerSettings Settings;
        protected readonly IDataClient<T> DataLoader;
        protected readonly IDataClient<T> DataPublisher;
        public readonly List<IQueryDefinition<T>> QueryDefinitions;

        public AnalyzerClientBase(ILogger logger, AnalyzerSettings settings)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            DataLoader = GetDataLoader( Settings.Source);
            DataPublisher = GetDataPublisher( Settings.Destination);
            QueryDefinitions = GetQueryDefinitions(Settings.QueryDefinitions);
        }

        public async Task Run(CancellationToken stopToken)
        {
            await ExecuteMigration(stopToken);

            var runData = await RunEvaluationQueries(stopToken);

            await GenerateReport(runData);
        }


        public IDataClient<T> GetDataLoader(DataConfiguration config)
        {
            Logger.LogInformation($"Begin fetching loader data client");

            if (DataLoader != null) return DataPublisher;

            Logger.LogInformation($"Initialize new data client for loader");

            return DataClientFactory.Create<T>(Logger, config);
        }

        public IDataClient<T> GetDataPublisher(DataConfiguration config)
        {
            Logger.LogInformation($"Begin fetching publisher data client");

            if (DataPublisher != null) return DataPublisher;

            Logger.LogInformation($"Initialize new data client for publisher");

            return DataClientFactory.Create<T>(Logger, config);
        }

        public abstract Task ExecuteMigration(CancellationToken stopToken);

        public async Task<List<QueryExecutionResult>> RunEvaluationQueries(CancellationToken stopToken)
        {
            Logger.LogInformation("Begin query evaluation");

            var results = new List<QueryExecutionResult>();
            foreach (var query in QueryDefinitions)
            {
                results.AddRange(await query.Execute(DataPublisher, stopToken));
            }

            Logger.LogInformation("Query evaluation complete");

            return results;
        }

        public async Task GenerateReport(List<QueryExecutionResult> runData)
        {
            var generator = new CsvReportGenerator(Logger, Settings.QueryDefinitions.First().OutputConfig);
            await generator.GenerateReport(runData);
        }

        public List<IQueryDefinition<T>> GetQueryDefinitions(List<QueryConfig> queryConfigs)
        {
            Logger.LogInformation("Begin loading query definitions");

            if (QueryDefinitions != null) return QueryDefinitions;

            var definitions = new List<IQueryDefinition<T>>();

            Logger.LogInformation("Initialize new query definitions");

            if (queryConfigs?.Any() == true)
            {
                Logger.LogInformation($"{queryConfigs.Count} query configurations were found.");

                foreach (var c in queryConfigs)
                {
                    Logger.LogInformation($"Resolving query definition with name {c.ClassName}");

                    try
                    {
                        var queryDefinition = Activator.CreateInstance(Type.GetType(c.ClassName), new object[] {c, Logger});

                        definitions.Add((IQueryDefinition<T>)queryDefinition);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Unable to activate query config instance", ex);
                    }

                }
            }
            else
            {
                Logger.LogInformation("No Queries configurations were found.");
            }

            return definitions;

        }
    }
}
