using System;
using System.IO;
using DexOfCards.Utilities;
using Microsoft.Data.Sqlite;

namespace DexOfCards.Framework.Storage;

public static class SQLite
{
    public static SqliteConnection GetSql()
    {
        var connection = new SqliteConnection(@$"Data Source={Path.Combine(FilePaths.Resources, "data.s3db")};");

        try
        {
            connection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite connection failed to open!");
        }

        return connection;
    }
}