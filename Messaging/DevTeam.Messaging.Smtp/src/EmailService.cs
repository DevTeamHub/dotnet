using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace DevTeam.Messaging.Emails;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpSettings> _emailSettings;

    public EmailService(IOptions<SmtpSettings> emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task Send(EmailMessageModel emailMessage, CancellationToken cancellationToken = default)
    {
        var settings = _emailSettings.Value;

        var message = new MimeMessage();
        message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(emailMessage.Sender, x)));
        message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(emailMessage.Sender, x)));

        message.Subject = emailMessage.Subject;

        message.Body = new TextPart(TextFormat.Html)
        {
            Text = emailMessage.Content
        };

        using var emailClient = new SmtpClient();

        await emailClient.ConnectAsync(settings.Server, settings.Port, SecureSocketOptions.StartTls, cancellationToken);

        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

        await emailClient.AuthenticateAsync(settings.Username, settings.Password, cancellationToken);

        await emailClient.SendAsync(message, cancellationToken);

        await emailClient.DisconnectAsync(true, cancellationToken);
    }
}