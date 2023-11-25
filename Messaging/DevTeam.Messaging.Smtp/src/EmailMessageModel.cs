using System.Collections.Generic;

namespace DevTeam.Messaging.Emails;

public class EmailMessageModel
{
    public EmailMessageModel()
    {
        ToAddresses = new List<string>();
        FromAddresses = new List<string>();
    }

    public List<string> ToAddresses { get; set; }
    public List<string> FromAddresses { get; set; }
    public string Sender { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Content { get; set; } = null!;
}
