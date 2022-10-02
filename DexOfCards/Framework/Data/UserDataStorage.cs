using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class UserDataStorage
{
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

    public static async Task<Tuple<string, int>> UpdateCard(OwnedCardModel oldCard, OwnedCardModel newCard)
    {
        oldCard ??= new OwnedCardModel(newCard.CardSet, newCard.CardNumber, newCard.Style, newCard.Amount);

        await using var conn = SQLite.GetUserDataSql();
        conn.ExecuteNoQuery($"UPDATE owned_cards SET amount = @amount WHERE cardSet = '{oldCard.CardSet}' AND cardNumber = '{oldCard.CardNumber}' AND type = '{oldCard.Style}'", new[]
        {
            new SQLiteParameter("@amount", newCard.Amount)
        });

        return new Tuple<string, int>(newCard.Style, newCard.Amount);
    }
}