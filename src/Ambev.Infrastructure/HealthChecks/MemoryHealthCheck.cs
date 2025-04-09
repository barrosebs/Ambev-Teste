using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ambev.Infrastructure.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly long _thresholdBytes;

        public MemoryHealthCheck(long thresholdBytes = 500 * 1024 * 1024) // 500MB por padrão
        {
            _thresholdBytes = thresholdBytes;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var memoryInfo = GC.GetGCMemoryInfo();
            var totalMemory = GC.GetTotalMemory(false);
            var allocatedMemory = totalMemory;

            var data = new Dictionary<string, object>
            {
                { "allocated_bytes", allocatedMemory },
                { "total_allocated_bytes", totalMemory },
                { "gen0_collections", GC.CollectionCount(0) },
                { "gen1_collections", GC.CollectionCount(1) },
                { "gen2_collections", GC.CollectionCount(2) }
            };

            if (allocatedMemory > _thresholdBytes)
            {
                return Task.FromResult(HealthCheckResult.Degraded(
                    "Memory usage is high",
                    null,
                    data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Memory usage is normal", data));
        }
    }
} 