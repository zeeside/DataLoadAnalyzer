using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.QueryDefinitions
{
    public class QueryExecutionMetrics
    {
        public string Name { get; set; }

        public int Id { get; set; }
        
        public long Duration { get; set; }

        public long RecordsFetched { get; set; }
    }
}
