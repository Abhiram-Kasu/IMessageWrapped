using System;
using System.Collections.Generic;
using System.IO;
using IMessageWrapped.Models;
using SQLite;

namespace IMessageWrapped.Services;

public class DbService : IDisposable
{
    private readonly SQLiteConnection? _connection;
    private readonly string _dbFilePath;

    private readonly List<IQuery> _queries =
    [
        new NumMessagesQuery()
    ];

    private DbService(string dbFilePath)
    {
        _dbFilePath = dbFilePath;
        _connection = new SQLiteConnection(dbFilePath);
    }


    public void Dispose()
    {
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }


    public static ResultTuple<DbService, string> CreateDbService(string dbFilePath)
    {
        if (!File.Exists(dbFilePath))
            return (null, "File does not exist");

        if (Path.GetExtension(dbFilePath) != ".db")
            return (null, "File is not a .db file");

        try
        {
            File.OpenRead(dbFilePath).Close();

            return (new DbService(dbFilePath), null);
        }
        catch (IOException e)
        {
            return (null, e.Message);
        }
    }

    public async IAsyncEnumerable<QueryResult> GetQueriesAsync()
    {
        foreach (var query in _queries) yield return await query.Run(_connection!);
    }
}