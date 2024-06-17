using InvestManagerSystem.Interfaces.Email;

namespace InvestManagerSystem.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto content);
    }
}
