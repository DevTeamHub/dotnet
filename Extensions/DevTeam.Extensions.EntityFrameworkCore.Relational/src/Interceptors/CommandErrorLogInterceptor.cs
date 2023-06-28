using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevTeam.Extensions.EntityFrameworkCore.Relational.Interceptors;

public class CommandErrorLogInterceptor : DbCommandInterceptor
{
    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        AddSQLInfoToException(command, eventData);
        base.CommandFailed(command, eventData);
    }

    public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        AddSQLInfoToException(command, eventData);
        return base.CommandFailedAsync(command, eventData, cancellationToken);
    }

    private static void AddSQLInfoToException(DbCommand command, CommandErrorEventData eventData)
    {
        if (eventData.Exception != null)
        {
            var parameters = command.Parameters.Cast<DbParameter>().Select(x => $"{x.ParameterName}: {x.Value}");

            eventData.Exception.Data.Add("SQL", command.CommandText);
            eventData.Exception.Data.Add("Parameters", string.Join(", ", parameters));
            if (command.Transaction != null)
                eventData.Exception.Data.Add("Isolation level", command.Transaction.IsolationLevel.ToString());
        }
    }
}

