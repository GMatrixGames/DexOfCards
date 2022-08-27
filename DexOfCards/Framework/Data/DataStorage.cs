using System.Collections.Generic;
using System.Data;
using System.Linq;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class DataStorage
{
    public static readonly List<CardModel> Cards = new();
    public static readonly List<CardSetModel> CardSets = new();

    public static async void Init()
    {
        await using var conn = SQLite.GetSql();
        var read = await conn.ExecuteReaderAsync("SELECT * FROM cards");
        while (await read.ReadAsync())
        {
            Cards.Add(new CardModel(
                read.GetString("cardName"),
                read.GetString("cardSet"),
                read.GetString("cardNumber"),
                read.GetString("cardImage"),
                read.GetString("language")
            ));
        }

        read = await conn.ExecuteReaderAsync("SELECT * FROM sets");
        while (await read.ReadAsync())
        {
            CardSets.Add(new CardSetModel(
                read.GetString("setid"),
                read.GetString("setname"),
                read.GetString("cardsInSet"),
                read.GetString("setImage"),
                read.GetString("language")
            ));
        }
    }

    public static CardSetModel GetModel(CardModel model) => CardSets.FirstOrDefault(a => a.SetId == model.CardSet && a.Languages.Any(b => b == model.Language));
    public static List<CardModel> GetCards(CardSetModel model) => Cards.Where(a => a.CardSet == model.SetId).ToList();
}