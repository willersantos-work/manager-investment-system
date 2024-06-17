using InvestManagerSystem.Services.AuthService;
using InvestManagerSystem.Services.EmailService;
using InvestManagerSystem.Services.FinancerProductService;
using InvestManagerSystem.Services.InvestmentService;
using InvestManagerSystem.Services.TokenService;
using InvestManagerSystem.Services.TransactionService;
using InvestManagerSystem.Services.UserService;

namespace InvestManagerSystem.Global.Injection
{
    public static class ServiceInjection
    {
        public static IServiceCollection ServiceLayerInjection(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFinancerProductService, FinancerProductService>();
            services.AddScoped<IInvestmentService, InvestmentService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
