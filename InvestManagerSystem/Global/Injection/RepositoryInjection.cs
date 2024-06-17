using InvestManagerSystem.Repositories.FinancerProductRepository;
using InvestManagerSystem.Repositories.InvestmentRepository;
using InvestManagerSystem.Repositories.TransactionRepository;
using InvestManagerSystem.Repositories.UserRepository;

namespace InvestManagerSystem.Global.Injection
{
    public static class RepositoryInjection
    {
        public static IServiceCollection RepositoryLayerInjection(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFinancerProductRepository, FinancerProductRepository>();
            services.AddScoped<IInvestmentRepository, InvestmentRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
