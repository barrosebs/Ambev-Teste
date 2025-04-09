using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace Ambev.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(IConfiguration configuration)
        {
            var elasticsearchUrl = configuration["Elasticsearch:Url"];
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);

            if (!string.IsNullOrEmpty(elasticsearchUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "ambev-logs-{0:yyyy.MM.dd}"
                });
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
} 