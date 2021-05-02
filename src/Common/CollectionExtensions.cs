using DataLoadAnalyzer.Data.QueryDefinitions;
using DataLoadAnalyzer.QueryDefinitions;
using System.Collections.Generic;
using System.Linq;

namespace DataLoadAnalyzer.Common
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(
           this IEnumerable<TSource> source, int batchSize)
        {
            var items = new TSource[batchSize];
            var count = 0;
            foreach (var item in source)
            {
                items[count++] = item;
                if (count == batchSize)
                {
                    yield return items;
                    items = new TSource[batchSize];
                    count = 0;
                }
            }
            if (count > 0)
                yield return items.Take(count);
        }

        public static List<QueryExecutionSummary> Summarize(this List<QueryExecutionResult> results)
        {
            if (results?.Any() == true)
            {
                var summaries = new List<QueryExecutionSummary>();
                foreach(var r in results)
                {
                    var s = new QueryExecutionSummary
                    {
                        Name = r.Name,
                        Description = r.Description,
                        Iterations = r.Metrics.Count,
                        AverageRunTimeInMilliseconds = r.Metrics.Average(m => m.Duration),
                        RecordsFetched = r.Metrics.FirstOrDefault().RecordsFetched
                    };

                    summaries.Add(s);
                }
                return summaries;

            }

            return null;
        }
    }
}
