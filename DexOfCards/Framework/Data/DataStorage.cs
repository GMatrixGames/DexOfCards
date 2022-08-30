using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class DataStorage
{
    public static List<CardModel> Cards = new();
    public static List<CardSetModel> CardSets = new();

    public static void Init()
    {
        InitCards();
        InitSets();
    }

    public static async void InitCards()
    {
        Cards.Clear();
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
    }

    public static async void InitSets()
    {
        CardSets.Clear();
        await using var conn = SQLite.GetSql();
        var read = await conn.ExecuteReaderAsync("SELECT * FROM sets");
        while (await read.ReadAsync())
        {
            CardSets.Add(new CardSetModel(
                read.GetString("setid"),
                read.GetString("setname"),
                read.GetString("cardsInSet"),
                read.GetString("setImage"),
                read.GetString("language"),
                DateTime.Parse(read.GetString("releaseDate"))
            ));
        }

        CardSets = CardSets.OrderByDescending(a => a.ReleaseDate).ToList();
    }

    public static CardSetModel GetSet(CardModel model) => CardSets.FirstOrDefault(a => a.SetId == model.CardSet);
    public static CardSetModel GetSet(string model) => CardSets.FirstOrDefault(a => a.SetId == model);
    public static Dictionary<string, CardSetModel> GetRegionSets(CardSetModel orig)
    {
        // Lang, Model
        var ret = new Dictionary<string, CardSetModel>();

        foreach (var set in CardSets)
        {
            if (!string.IsNullOrWhiteSpace(set.SubRegion) && set.SetId.SubstringBefore('_') == orig.SetId)
            {
                ret.Add(CardSetModel.GetLanguageFromSubRegion(set.SubRegion), set);
            }
            else if (set.SetId.SubstringBefore('_') == orig.SetId && orig.Languages.Contains("KO"))
            {
                ret.Add("KO", orig);
            }
        }

        return ret;
    }
    public static List<CardModel> GetCards(CardSetModel model)
    {
        var allCards = Cards.Where(a => a.CardSet == model.SetId).ToList();
        var allNormal = allCards.Where(a => int.TryParse(a.CardNumber, out _));
        var allExtra = allCards.Where(a => !int.TryParse(a.CardNumber, out _));

        allNormal = allNormal.OrderBy(a => int.Parse(a.CardNumber));
        allExtra = allExtra.OrderBy(a => int.Parse(a.CardNumber
            .Replace("SV", "")
            .Replace("TG", "")
            .Replace("RC", "")
            .Replace("SWSH", "")));
        
        var cards = allNormal.ToList();
        cards.AddRange(allExtra);
        return cards;
    }
}