using InvestManagerSystem.Global.Configs;
using InvestManagerSystem.Global.Injection;
using InvestManagerSystem.Interfaces.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace InvestManagerSystem.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailConfig _emailConfig;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailConfig> emailConfig)
        {
            _logger = logger;
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmail(EmailDto content)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(SendEmail)} - Request - {JsonSerializer.Serialize(content)}");
                var from = _emailConfig.From;
                var password = _emailConfig.Password;
                var host = _emailConfig.Host;
                var port = _emailConfig.Port;
                var credentials = new NetworkCredential(from, password);
                _logger.LogInformation($"{from} {password} teste emails");

                var client = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    Credentials = credentials,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                MailMessage message = new MailMessage(from, content.To, content.Subject, content.Message);

                await client.SendMailAsync(message);
                _logger.LogInformation($"end service {nameof(SendEmail)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(SendEmail)} - Error - {JsonSerializer.Serialize(content)}");
                throw;
            }
        }
    }
}