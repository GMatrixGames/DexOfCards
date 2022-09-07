using System;
using System.Data.SQLite;
using System.IO;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Storage;

public static class SQLite
{
    public static SQLiteConnection GetStorageSql()
    {
        var connection = new SQLiteConnection($"Data Source={Path.Combine(FilePaths.Resources, "storage.s3db")};");

        try
        {
            connection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite Storage connection failed to open!");
        }

        return connection;
    }

    public static SQLiteConnection GetUserDataSql()
    {
        var connection = new SQLiteConnection($"Data Source={Path.Combine(FilePaths.AppData, "user_store.s3db")};");

        try
        {
            connection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite UserData connection failed to open!");
        }

        return connection;
    }
}