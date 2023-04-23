using Application.Common.Models.Mail;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}
