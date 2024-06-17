namespace InvestManagerSystem.Interfaces.Email
{
    public class EmailDto
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
