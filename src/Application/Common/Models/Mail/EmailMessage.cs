namespace Application.Common.Models.Mail;

public class EmailMessage
{
    public string? From { get; set; }
    public string? DisplayName { get; set; }
    public string[] To { get; set; }
    public string[]? Cc { get; set; }
    public string[]? Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
}
