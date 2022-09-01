using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace DexOfCards.Utilities;

public static class SqlExtensions
{
    public static async Task<DbDataReader> ExecuteReaderAsync(this SQLiteConnection connection, string command, SQLiteParameter[] parameters = null)
    {
        var ex = new SQLiteCommand(command, connection);
        if (parameters is { Length: > 0 }) ex.Parameters.AddRange(parameters);
        var reader = await ex.ExecuteReaderAsync();
        return reader;
    }

    public static void ExecuteNoQuery(this SQLiteConnection connection, string command, SQLiteParameter[] parameters = null)
    {
        var ex = new SQLiteCommand(command, connection);
        if (parameters is { Length: > 0 }) ex.Parameters.AddRange(parameters);
        ex.ExecuteNonQuery();
    }
}