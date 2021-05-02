using System.Collections.Generic;

namespace DataLoadAnalyzer.Configuration
{
    public class Config
    {
        public const string AnalyzerSettings = "Config";

        public List<AnalyzerSettings> Analyzers { get; set; } = new List<AnalyzerSettings>();
    }
}
