using System;
using System.Data;
using System.IO;
using DexOfCards.Utilities;
using Microsoft.Data.Sqlite;

namespace DexOfCards.Framework.Storage;

public static class SQLite
{
    private static SqliteConnection _connection;

    public static SqliteConnection GetSql()
    {
        if (_connection is { State: ConnectionState.Open }) return _connection;
        _connection = new SqliteConnection(@$"Data Source={Path.Combine(FilePaths.Resources, "storage.s3db")};");

        try
        {
            _connection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite connection failed to open!");
        }

        return _connection;
    }
}