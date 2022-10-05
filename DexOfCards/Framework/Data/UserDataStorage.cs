using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class UserDataStorage
{
    public static async void Setup()
    {
        var isValid = true;
        if (File.Exists(Path.Combine(FilePaths.AppData, "user_store.s3db")))
        {
            await using var conn1 = SQLite.GetUserDataSql();

            var version = (UserDataStorageVersion) conn1.ExecuteScalar<long>("SELECT version FROM data");
            if (version is < UserDataStorageVersion.Initial or > UserDataStorageVersion.Latest)
            {
                isValid = false;
                goto invalidOrNew;
            }

            return;
        }

        invalidOrNew:
        if (!isValid) File.Delete(Path.Combine(FilePaths.AppData, "user_store.s3db"));
        await using var conn2 = SQLite.GetUserDataSql();
        conn2.ExecuteNoQuery("CREATE TABLE IF NOT EXISTS owned_cards (cardSet VARCHAR(150), cardNumber VARCHAR(20), type VARCHAR(200), amount INTEGER(5), language VARCHAR(5))");
        conn2.ExecuteNoQuery("CREATE TABLE IF NOT EXISTS data (version INTEGER)");
        conn2.ExecuteNoQuery($"INSERT INTO data (version) VALUES ({(int) UserDataStorageVersion.Latest})");
    }

    public static async Task<List<OwnedCardModel>> GetOwnedCardsFromSet(CardSetModel set)
    {
        var ret = new List<OwnedCardModel>();

        await using var conn = SQLite.GetUserDataSql();
        var read = await conn.ExecuteReaderAsync($"SELECT * FROM owned_cards WHERE cardSet = '{set.SetId}' AND language = '{set.Language}'");
        while (await read.ReadAsync())
        {
            ret.Add(new OwnedCardModel(
                read.GetString("cardSet"),
                read.GetString("cardNumber"),
                read.GetString("language"),
                read.GetString("type"),
                read.GetInt32("amount")
            ));
        }

        return ret;
    }

    public static async Task<(string, int)> UpdateCard(OwnedCardModel oldCard, OwnedCardModel newCard)
    {
        oldCard ??= new OwnedCardModel(newCard.CardSet, newCard.CardNumber, newCard.Style, newCard.Amount);

        await using var conn = SQLite.GetUserDataSql();
        conn.ExecuteNoQuery($"UPDATE owned_cards SET amount = @amount WHERE cardSet = '{oldCard.CardSet}' AND cardNumber = '{oldCard.CardNumber}' AND type = '{oldCard.Style}'", new[]
        {
            new SQLiteParameter("@amount", newCard.Amount)
        });

        return (newCard.Style, newCard.Amount);
    }
}