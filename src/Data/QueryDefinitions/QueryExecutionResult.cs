using System.Collections.Generic;

namespace DataLoadAnalyzer.QueryDefinitions
{
    public class QueryExecutionResult
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<QueryExecutionMetrics> Metrics { get; set; } = new List<QueryExecutionMetrics>();

        
    }
}
