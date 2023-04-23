namespace Infrastructure.MessageBroker;

public sealed class MessageBrokerSettings
{
    public const string SettingsKey = "MessageBroker";

    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
