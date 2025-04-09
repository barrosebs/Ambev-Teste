namespace Ambev.Infrastructure.Configurations
{
    public class CorsConfiguration
    {
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
        public string[] AllowedMethods { get; set; } = Array.Empty<string>();
        public string[] AllowedHeaders { get; set; } = Array.Empty<string>();
        public string[] ExposedHeaders { get; set; } = Array.Empty<string>();
        public bool AllowCredentials { get; set; }
        public int MaxAge { get; set; }
    }
} 