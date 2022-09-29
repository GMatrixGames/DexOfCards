using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class DataStorage
{
    private static readonly List<CardModel> Cards = new();
    private static readonly List<CardSetModel> CardSets = new();

    public static void Init()
    {
        InitSets();
        InitCards();
    }

    public static async void InitCards()
    {
        Cards.Clear();
        await using var conn = SQLite.GetStorageSql();
        var read = await conn.ExecuteReaderAsync("SELECT * FROM cards");
        while (await read.ReadAsync())
        {
            Cards.Add(new CardModel(
                read.GetString("cardName"),
                read.GetString("cardSet"),
                read.GetString("cardNumber"),
                read.GetString("cardImage"),
                read.GetString("language"),
                JsonSerializer.Deserialize<string[]>(read.GetString("styles"))
            ));
        }
    }

    public static async void InitSets()
    {
        CardSets.Clear();
        await using var conn = SQLite.GetStorageSql();
        var read = await conn.ExecuteReaderAsync("SELECT * FROM sets");
        while (await read.ReadAsync())
        {
            CardSets.Add(new CardSetModel(
                read.GetString("setid"),
                read.GetString("setname"),
                read.GetString("cardsInSet"),
                read.GetString("setImage"),
                read.GetString("language"),
                DateTimeOffset.FromUnixTimeMilliseconds(read.GetInt64("releaseDate")).Date
            ));
        }
    }

    public static List<CardSetModel> GetAllSets() => CardSets.OrderByDescending(a => a.ReleaseDate).ToList();
    public static CardSetModel GetSet(CardModel model) => CardSets.FirstOrDefault(a => a.SetId == model.CardSet.SetId);
    public static CardSetModel GetSet(string model) => CardSets.FirstOrDefault(a => a.SetId == model);
    public static Dictionary<string, List<CardSetModel>> GetSetsByLanguage()
    {
        var ret = new Dictionary<string, List<CardSetModel>>();

        foreach (var value in Enum.GetValues<CardLanguage>())
        {
            var lang = value.ToString();
            var setsByLanguage = GetAllSets().Where(a => a.Language == lang).ToList();

            if (setsByLanguage.Count <= 0) continue;
            ret.Add(lang, setsByLanguage);
        }

        return ret;
    }
    public static List<CardSetModel> GetSetByLanguage(CardLanguage lang)
    {
        GetSetsByLanguage().TryGetValue(lang.ToString(), out var ret);
        ret ??= new List<CardSetModel>();
        return ret;
    }
    public static List<CardModel> GetAllCards() => Cards;
    public static List<CardModel> GetCards(string set) => GetCards(GetSet(set));
    public static List<CardModel> GetCards(CardSetModel model)
    {
        var allCards = GetAllCards().Where(a => a.CardSet?.SetId == model.SetId).ToList();
        var allNormal = allCards
            .Where(a => int.TryParse(a.CardNumber.Replace("a", ""), out _))
            .OrderBy(a => int.Parse(a.CardNumber.Replace("a", "")));
        var allExtra = allCards.Where(a => !int.TryParse(a.CardNumber.Replace("a", ""), out _)).ToList();
        var allEnergyNoNumber = allExtra.Where(a => a.IsEnergy);
        allExtra = allExtra.Where(a => !a.IsEnergy)
            .OrderBy(a => int.Parse(a.CardNumber
                .Replace("SV", "")
                .Replace("TG", "")
                .Replace("RC", "")
                .Replace("CC", "")
                .Replace("BW", "")
                .Replace("XY", "")
                .Replace("SM", "")
                .Replace("SWSH", ""))).ToList();

        var cards = allNormal.ToList();
        cards.AddRange(allExtra);
        cards.AddRange(allEnergyNoNumber);
        return cards;
    }
    public static List<string> GetKnownStyles()
    {
        var ret = new List<string>();

        foreach (var style in Cards.SelectMany(card => card.Styles.Where(style => !ret.Contains(style) && style != "Default")))
        {
            ret.Add(style);
        }

        return ret;
    }
}