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
    public static CardSetModel GetSet(CardModel model, CardLanguage lang = CardLanguage.NonAsia) => CardSets.FirstOrDefault(a => a.SetId == model.CardSet.SetId && a.Language == lang);
    public static CardSetModel GetSet(string model, CardLanguage lang = CardLanguage.NonAsia) => CardSets.FirstOrDefault(a => a.SetId == model && a.Language == lang);
    public static Dictionary<CardLanguage, List<CardSetModel>> GetSetsByLanguage()
    {
        var ret = new Dictionary<CardLanguage, List<CardSetModel>>();

        foreach (var value in Enum.GetValues<CardLanguage>())
        {
            var setsByLanguage = GetAllSets().Where(a => a.Language == value).ToList();

            if (setsByLanguage.Count <= 0) continue;
            ret.Add(value, setsByLanguage);
        }

        return ret;
    }
    public static List<CardSetModel> GetSetByLanguage(CardLanguage lang)
    {
        GetSetsByLanguage().TryGetValue(lang, out var ret);
        ret ??= new List<CardSetModel>();
        return ret;
    }
    public static List<CardModel> GetAllCards() => Cards;
    public static List<CardModel> GetCards(string set, CardLanguage lang = CardLanguage.NonAsia) => GetCards(GetSet(set, lang));
    public static List<CardModel> GetCards(CardSetModel model)
    {
        var allCards = GetAllCards().Where(a => a.CardSet?.SetId == model.SetId && a.Language == model.Language).ToList();
        var allNormal = allCards
            .Where(a => int.TryParse(a.CardNumber.Replace("a", ""), out _))
            .OrderBy(a => int.Parse(a.CardNumber.Replace("a", "")));
        var allExtra = allCards.Where(a => !int.TryParse(a.CardNumber.Replace("a", ""), out _)).ToList();
        var allEnergyNoNumber = allExtra.Where(a => a.IsEnergy);
        var allUnparsed = allExtra.Where(a => !a.IsEnergy && !int.TryParse(a.CardNumber
                .Replace("SV", "")
                .Replace("TG", "")
                .Replace("RC", "")
                .Replace("CC", "")
                .Replace("BW", "")
                .Replace("XY", "")
                .Replace("SM", "")
                .Replace("SWSH", ""), out _))
            .OrderBy(a => a.CardNumber);
        allExtra = allExtra.Where(a => !a.IsEnergy && int.TryParse(a.CardNumber
                .Replace("SV", "")
                .Replace("TG", "")
                .Replace("RC", "")
                .Replace("CC", "")
                .Replace("BW", "")
                .Replace("XY", "")
                .Replace("SM", "")
                .Replace("SWSH", ""), out _))
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
        cards.AddRange(allUnparsed);
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