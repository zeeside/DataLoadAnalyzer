using System.Collections.Generic;

namespace DataLoadAnalyzer.Configuration
{
    public class AnalyzerSettings
    {

        public string Name { get; set; }
        public string ServiceClass { get; set; }
        public bool SkipMigration { get; set; }
        public bool SkipMigrationIfDataExists { get; set; }
        public DataConfiguration Source { get; set; }
        public DataConfiguration Destination { get; set; }
        public List<QueryConfig> QueryDefinitions { get; set; }
    }
}
