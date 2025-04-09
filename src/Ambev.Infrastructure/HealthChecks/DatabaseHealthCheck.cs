using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Ambev.Infrastructure.Data.Context;

namespace Ambev.Infrastructure.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly AmbevContext _context;

        public DatabaseHealthCheck(AmbevContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Verifica se consegue se conectar ao banco de dados
                await _context.Database.CanConnectAsync(cancellationToken);

                // Verifica o tempo de resposta
                var startTime = DateTime.UtcNow;
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
                var responseTime = DateTime.UtcNow - startTime;

                var data = new Dictionary<string, object>
                {
                    { "response_time_ms", responseTime.TotalMilliseconds },
                    { "database", _context.Database.GetDbConnection().Database },
                    { "server", _context.Database.GetDbConnection().DataSource }
                };

                return HealthCheckResult.Healthy("Database is healthy", data);
            }
            catch (DbException ex)
            {
                return HealthCheckResult.Unhealthy("Database is unhealthy", ex);
            }
        }
    }
} 