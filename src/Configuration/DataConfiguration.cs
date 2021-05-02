using System.Collections.Generic;

namespace DataLoadAnalyzer.Configuration
{
    public class DataConfiguration
    {
        public string Type { get; set; }

        public List<ConnectionKey> ConfigDictionary { get; set; }
    }
}
