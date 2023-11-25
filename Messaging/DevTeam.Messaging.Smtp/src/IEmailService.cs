using System.Threading.Tasks;
using System.Threading;

namespace DevTeam.Messaging.Emails;

public interface IEmailService
{
    Task Send(EmailMessageModel emailMessage, CancellationToken cancellationToken = default);
}
