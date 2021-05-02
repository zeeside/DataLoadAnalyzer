namespace DataLoadAnalyzer.Configuration
{
    public class QueryConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string  ClassName { get; set; }
        public int Iterations { get; set; }
        public QueryOutputConfig OutputConfig { get; set; }
    }
}
