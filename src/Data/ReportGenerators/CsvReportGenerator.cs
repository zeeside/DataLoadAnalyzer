using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.QueryDefinitions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CsvHelper;
using System;
using DataLoadAnalyzer.Common;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using DataLoadAnalyzer.Data.QueryDefinitions;

namespace DataLoadAnalyzer.Data.ReportGenerators
{
    public class CsvReportGenerator
    {
        private readonly QueryOutputConfig _settings;
        private readonly ILogger _logger;

        public CsvReportGenerator(ILogger logger, QueryOutputConfig settings)
        {
            _settings = settings ?? throw new ArgumentNullException($"{nameof(QueryOutputConfig)} cannot be null");
            _logger = logger;
        }

        public async Task GenerateReport(List<QueryExecutionResult> queryResults)
        {
            if (queryResults?.Any() == true)
            {
                if (_settings.Summarize)
                {
                    await GenerateSummary(queryResults);                    
                }
                else
                {
                    await GenerateFull(queryResults);
                }

                _logger.LogInformation("Report generation complete.");

            }
            else
            {
                _logger.LogInformation("No reports to generate. Query results empty.");
            }
        }

        private async Task GenerateSummary(List<QueryExecutionResult> queryResults)
        {
            _logger.LogInformation("Begin summarizing query results.");

            var summary = queryResults.Summarize();

            var filePath = GetFilePath("summary");

            _logger.LogInformation($"Report will be published to {filePath}");

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await Task.Run(() => csv.WriteRecords(summary));

            if (_settings.OutputToConsole)
            {
                WriteToConsole(summary);
            }
        }

        private async Task GenerateFull(List<QueryExecutionResult> queryResults)
        {
            _logger.LogInformation("Begin generating full report");

            var filePath = GetFilePath("full");

            _logger.LogInformation($"Report will be published to {filePath}");

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await Task.Run(() => csv.WriteRecords(queryResults.SelectMany(r => r.Metrics)));
        }

        private string GetFilePath(string reportName)
        {
            var extension = string.Concat(".", _settings.Format);

            var fileNameFormat = _settings.FilenameFormat.IndexOf(extension) > -1 ?
                _settings.FilenameFormat : 
                string.Concat(_settings.FilenameFormat, extension);

            var fileName = string.Format(fileNameFormat, reportName.StripSpecialCharacters());

            if (!_settings.Overwrite)
            {
                fileName = fileName.Replace(extension, string.Concat("_",DateTime.Now.ToString("yyyMMdd-hhmmss"), extension));
            }

            return string.Concat(_settings.FilePath, "\\", fileName);
        }

        private void WriteToConsole(List<QueryExecutionSummary> summary)
        {
            if (summary?.Any() == true)
            {
                PrintRow(new string[] { "Name", "Description", "Iterations", "AverageRunTime(ms)", "RecordsFetched" });

                foreach(var s in summary)
                {
                    PrintLine();
                    PrintRow(new string[] { s.Name, s.Description, s.Iterations.GetValueOrDefault().ToString(), s.AverageRunTimeInMilliseconds.GetValueOrDefault().ToString(), s.RecordsFetched.GetValueOrDefault().ToString() });
                }


            }
        }

        private void PrintRow(params string[] columns)
        {
            var tableWidth = 100;
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            _logger.LogInformation(row);
        }

        private void PrintLine()
        {
            var tableWidth = 100;
            Console.WriteLine(new string('-', tableWidth));
        }

        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
