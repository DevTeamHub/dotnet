namespace DevTeam.Messaging.Emails;

public class SmtpSettings
{
    public string Server { get; set; } = null!;
    public int Port { get; set; }
    public string Sender { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
