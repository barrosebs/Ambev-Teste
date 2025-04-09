using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Threading;

namespace Ambev.Infrastructure.HealthChecks
{
    public class ExternalServicesHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        public ExternalServicesHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Lista de serviços externos para verificar
                var services = new Dictionary<string, string>
                {
                    { "AuthService", "https://auth-service.example.com/health" },
                    { "PaymentService", "https://payment-service.example.com/health" }
                };

                var results = new Dictionary<string, object>();
                var unhealthyServices = new List<string>();

                foreach (var service in services)
                {
                    try
                    {
                        var startTime = DateTime.UtcNow;
                        var response = await _httpClient.GetAsync(service.Value, cancellationToken);
                        var responseTime = DateTime.UtcNow - startTime;

                        results.Add(service.Key, new
                        {
                            status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                            response_time_ms = responseTime.TotalMilliseconds,
                            status_code = (int)response.StatusCode
                        });

                        if (!response.IsSuccessStatusCode)
                        {
                            unhealthyServices.Add(service.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        results.Add(service.Key, new
                        {
                            status = "Unhealthy",
                            error = ex.Message
                        });
                        unhealthyServices.Add(service.Key);
                    }
                }

                if (unhealthyServices.Any())
                {
                    return HealthCheckResult.Degraded(
                        $"Some external services are unhealthy: {string.Join(", ", unhealthyServices)}",
                        null,
                        results);
                }

                return HealthCheckResult.Healthy("All external services are healthy", results);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Failed to check external services", ex);
            }
        }
    }
} 