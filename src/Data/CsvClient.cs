using CsvHelper;
using CsvHelper.Configuration;
using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Data
{
    public class CsvClient<T> : IDataClient<T>
    {
        public readonly string FilePath;
        private readonly ILogger _logger;

        public CsvClient(ILogger logger, DataConfiguration settings)
        {
            Check.IsNotNull<ILogger>(logger);
            Check.IsNotNull<List<ConnectionKey>>(settings?.ConfigDictionary, $"Invalid configs for {nameof(CsvClient<T>)}");

            _logger = logger;
            var keys = settings.ConfigDictionary;

            if (!keys.Any(k => k.Key == Constants.FilePath)) throw new ArgumentNullException($"Missing config key \"filepath\" for {nameof(CsvClient<T>)}");

            FilePath = keys.Single(k => k.Key == Constants.FilePath).Value;
        }

        public async Task<List<T>> Load(CancellationToken stopToken)
        {
            _logger.LogInformation($"Load CSV from path {FilePath}");

            using var reader = new StreamReader(FilePath);

            using var csv = new CsvReader(reader, GetCsvConfiguration());

            _logger.LogInformation("Data loaded. Begin publish");

            var records = await Task.Run(() => csv.GetRecords<T>().ToList());

            return records;
        }

        public Task Publish(List<T> data, CancellationToken stopToken)
        {
            throw new NotImplementedException();
        }

        private CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = "\t",
                BadDataFound = null
            };
        }
    }
}
