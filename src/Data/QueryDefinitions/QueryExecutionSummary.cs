namespace DataLoadAnalyzer.Data.QueryDefinitions
{
    public class QueryExecutionSummary
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Iterations { get; set; }
        public double? AverageRunTimeInMilliseconds { get; set; }
        public long? RecordsFetched { get; set; }

    }
}
