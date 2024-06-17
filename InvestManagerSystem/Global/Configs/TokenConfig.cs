namespace InvestManagerSystem.Global.Configs
{
    public class TokenConfig
    {
        public string SecretToken { get; set; } = String.Empty;
        public int TokenValidityDays { get; set; } = 1;
    }
}
