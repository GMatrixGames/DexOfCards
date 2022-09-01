using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Storage;

public static class SQLite
{
    private static SQLiteConnection _storageConnection;
    private static SQLiteConnection _userDataConnection;

    public static SQLiteConnection GetStorageSql()
    {
        if (_storageConnection is { State: ConnectionState.Open }) return _storageConnection;
        _storageConnection = new SQLiteConnection($"Data Source={Path.Combine(FilePaths.Resources, "storage.s3db")};");

        try
        {
            _storageConnection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite Storage connection failed to open!");
        }

        return _storageConnection;
    }

    public static SQLiteConnection GetUserDataSql()
    {
        if (_userDataConnection is { State: ConnectionState.Open }) return _userDataConnection;
        _userDataConnection = new SQLiteConnection($"Data Source={Path.Combine(FilePaths.AppData, "user_store.s3db")};");

        try
        {
            _userDataConnection.Open();
        }
        catch
        {
            Console.WriteLine("SQLite UserData connection failed to open!");
        }

        return _userDataConnection;
    }
}