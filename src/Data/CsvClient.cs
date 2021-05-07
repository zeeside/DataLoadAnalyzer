using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DataLoadAnalyzer.Common;
using DataLoadAnalyzer.Configuration;
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
        public readonly string FilePathType;
        private readonly ILogger _logger;

        public CsvClient(ILogger logger, DataConfiguration settings)
        {
            Check.IsNotNull<ILogger>(logger);
            Check.IsNotNull<List<ConnectionKey>>(settings?.ConfigDictionary, $"Invalid configs for {nameof(CsvClient<T>)}");

            _logger = logger;
            var keys = settings.ConfigDictionary;

            if (!keys.Any(k => k.Key == Constants.FilePath)) throw new ArgumentNullException($"Missing config key \"filepath\" for {nameof(CsvClient<T>)}");

            FilePath = keys.Single(k => k.Key == Constants.FilePath).Value;
            FilePathType = keys.Single(k => k.Key == Constants.FilePathType).Value ?? "absolute";

            Check.IsValidFilePathType(FilePathType);
        }

        public async Task<List<T>> Load(CancellationToken stopToken)
        {
            _logger.LogInformation($"Load CSV from {FilePathType} path {FilePath}");

            using var reader = new StreamReader(FilePath);

            using var csv = new CsvReader(reader, GetCsvConfiguration());

            _logger.LogInformation("Data loaded. Begin publish");

            var records = await csv.GetRecordsAsync<T>().ToListAsync();

            return records;
        }

        public Task Publish(List<T> data, CancellationToken stopToken)
        {
            throw new NotImplementedException();
        }

        public Task PublishAs<TOut>(List<TOut> records, CancellationToken stopToken) where TOut : class
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
