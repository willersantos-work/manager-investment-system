using InvestManagerSystem.Global.Helpers;

namespace InvestManagerSystem.Enums
{
    public enum ProductTypeEnum
    {
        [EnumDescription("Ações")]
        Stocks,
        [EnumDescription("Títulos")]
        Bonds,
        [EnumDescription("Fundos")]
        Funds,
        [EnumDescription("Imóveis")]
        RealEstate,
        [EnumDescription("Commodities")]
        Commodities,
        [EnumDescription("Câmbio")]
        ForeignExchange
    }
}
