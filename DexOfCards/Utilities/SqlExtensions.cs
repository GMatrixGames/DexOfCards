using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DexOfCards.Utilities;

public static class SqlExtensions
{
    public static async Task<SqliteDataReader> ExecuteReaderAsync(this SqliteConnection connection, string command, SqliteParameter[] parameters = null)
    {
        var ex = new SqliteCommand(command, connection);
        if (parameters is { Length: > 0}) ex.Parameters.AddRange(parameters);
        var reader = await ex.ExecuteReaderAsync();
        ex.Dispose();
        return reader;
    }

    public static void ExecuteNoQuery(this SqliteConnection connection, string command, SqliteParameter[] parameters = null)
    {
        var ex = new SqliteCommand(command, connection);
        if (parameters is { Length: > 0}) ex.Parameters.AddRange(parameters);
        ex.ExecuteNonQuery();
        ex.Dispose();
    }
}