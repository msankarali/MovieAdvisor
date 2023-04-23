using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;
using Application.Common.Models.Mail;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly IOptions<EmailSettings> _options;

    public SmtpEmailService(IOptions<EmailSettings> options)
    {
        var settings = options.Value;

        _smtpClient = new SmtpClient(host: options.Value.Host,
                                     port: options.Value.Port)
        {
            Credentials = new NetworkCredential(settings.UserName, settings.Password),
            EnableSsl = settings.UseSSL,
            DeliveryMethod = settings.UseStartTls ? SmtpDeliveryMethod.Network : SmtpDeliveryMethod.SpecifiedPickupDirectory
        };
        _options = options;
    }

    public async Task SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        MailMessage message = new MailMessage();
        message.Subject = emailMessage.Subject;
        message.From = new MailAddress(emailMessage.From ?? _options.Value.From);
        message.Sender = new MailAddress(emailMessage.DisplayName ?? _options.Value.DisplayName);
        for (int i = 0; i < emailMessage.To.Count(); i++)
        {
            message.To.Add(emailMessage.To[i]);
        }
        for (int i = 0; i < emailMessage.Cc?.Count(); i++)
        {
            message.CC.Add(emailMessage.Cc[i]);
        }
        for (int i = 0; i < emailMessage.Bcc?.Count(); i++)
        {
            message.Bcc.Add(emailMessage.Bcc[i]);
        }
        message.IsBodyHtml = emailMessage.IsBodyHtml;
        message.Body = emailMessage.Body;

        await _smtpClient.SendMailAsync(message, cancellationToken);
    }
}
