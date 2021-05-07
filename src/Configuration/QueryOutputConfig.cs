namespace DataLoadAnalyzer.Configuration
{
    public class QueryOutputConfig
    {
        public string Format { get; set; }
        public string FilenameFormat { get; set; }
        public bool Overwrite { get; set; }
        public bool Summarize { get; set; }
        public bool OutputToConsole { get; set; }
        public string FilePath { get; set; }
        public string FilePathType { get; set; }
    }
}
