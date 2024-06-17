using InvestManagerSystem.Global.Configs;

namespace InvestManagerSystem.Global.Injection
{
    public static class ConfigInjection
    {
        public static IServiceCollection ConfigLayerInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfig>(configuration.GetSection("TokenConfig"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailSettings"));

            return services;
        }
    }
}
