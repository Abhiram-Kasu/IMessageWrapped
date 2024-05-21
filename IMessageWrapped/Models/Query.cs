using System.Threading.Tasks;
using SQLite;

namespace IMessageWrapped.Models;

public record QueryResult(string Title, string Result);

public interface IQuery
{
    public ValueTask<QueryResult> Run(SQLiteConnection connection);
}

public record NumMessagesQuery : IQuery
{
    public ValueTask<QueryResult> Run(SQLiteConnection connection)
    {
        var result =
            connection.ExecuteScalar<int>("SELECT COUNT(*) FROM message WHERE text IS NOT NULL ORDER BY date DESC ");
        return ValueTask.FromResult(new QueryResult("Number of messages", result.ToString()));
    }
}