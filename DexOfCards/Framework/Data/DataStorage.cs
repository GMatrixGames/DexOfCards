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
    private static readonly List<CardModel> Cards = new();
    private static readonly List<CardSetModel> CardSets = new();

    public static void Init()
    {
        InitCards();
        InitSets();
    }

    public static async void InitCards()
    {
        Cards.Clear();
        await using var conn = SQLite.GetStorageSql();
        var read = await conn.ExecuteReaderAsync("SELECT * FROM cards");
        while (await read.ReadAsync())
        {
            var name = read.GetString("cardName");
            var set = read.GetString("cardSet");
            var number = read.GetString("cardNumber");
            var cardStyle = read.GetString("style");
            var image = read.GetString("cardImage");
            var language = read.GetString("language");

            // Since styles are separate entries, we need to add the styles to the card as well as image to add to the carousel
            if (Cards.Any(a => a.CardName == name && a.CardNumber == number && a.CardSet == set && a.Language == language && !a.Styles.ContainsKey(cardStyle)))
            {
                var card = Cards.Find(a => a.CardName == name && a.CardNumber == number && !a.Styles.ContainsKey(cardStyle));
                if (card != null)
                {
                    Cards.Remove(card);
                    card.AddToStyles(cardStyle, image);
                    Cards.Add(card);
                }
            }
            else
            {
                Cards.Add(new CardModel(
                    read.GetString("cardName"),
                    read.GetString("cardSet"),
                    read.GetString("cardNumber"),
                    read.GetString("cardImage"),
                    read.GetString("language"),
                    read.GetString("style")
                ));
            }
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
    public static List<CardModel> GetAllCards() => Cards;
    public static List<CardModel> GetCards(string set) => GetCards(GetSet(set));
    public static List<CardModel> GetCards(CardSetModel model)
    {
        var allCards = GetAllCards().Where(a => a.CardSet == model.SetId).ToList();
        var allNormal = allCards
            .Where(a => int.TryParse(a.CardNumber, out _))
            .OrderBy(a => int.Parse(a.CardNumber));
        var allExtra = allCards
            .Where(a => !int.TryParse(a.CardNumber, out _))
            .OrderBy(a => int.Parse(a.CardNumber
                .Replace("SV", "")
                .Replace("TG", "")
                .Replace("RC", "")
                .Replace("CC", "")
                .Replace("SWSH", "")));

        var cards = allNormal.ToList();
        cards.AddRange(allExtra);
        return cards;
    }
}