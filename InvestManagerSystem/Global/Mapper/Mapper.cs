using AutoMapper;
using InvestManagerSystem.Interfaces.FinancerProduct;
using InvestManagerSystem.Interfaces.Investment;
using InvestManagerSystem.Interfaces.Transaction;
using InvestManagerSystem.Interfaces.User;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Global.Mapper
{
    public static class Mapper
    {
        private static readonly IMapper _mapper;

        static Mapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Financer Product
                cfg.CreateMap<FinancerProduct, FinancerProductDto>().ReverseMap();
                cfg.CreateMap<FinancerProductCreateDto, FinancerProduct>().ReverseMap();

                // User
                cfg.CreateMap<UserSaveDto, User>().ReverseMap();
                cfg.CreateMap<UserSaveResponseDto, User>().ReverseMap();
                
                // Investment
                cfg.CreateMap<Investment, InvestmentDto>().ReverseMap();

                // Transaction
                cfg.CreateMap<Transaction, TransactionDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }


}
