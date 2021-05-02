using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataLoadAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;
                    services.Configure<Config>(config.GetSection(Config.AnalyzerSettings));
                    services.AddHostedService<DataAnalyzerService>();
                });
    }
}
