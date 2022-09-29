using System;
using System.IO;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data.Models;

public class CardSetModel
{
    public CardSetModel(string id = "???", string name = "???", string cardAmount = "???", string image = "???", string language = "???", DateTime? releaseDate = default)
    {
        SetId = id;
        SubRegion = SetId.Contains('_') ? SetId.SubstringAfterLast('_') : null;
        SetName = name + (!string.IsNullOrWhiteSpace(SubRegion) ? $" ({GetLanguageFromSubRegion(SubRegion)})" : language == "JP" ? " (JP)" : string.Empty);
        CardsInSet = cardAmount;
        Language = language;
        IsAsia = language == "NonAsia";
        SetImage = image is "Promo_Asia.png" or "Promo.png" ? $"images/Sets/{image}" : $"images/Sets/{language}/{image}";
        ReleaseDate = releaseDate ?? DateTime.Now;
    }

    public string SetId { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    public string SubRegion { get; }
    public string SetImage { get; }
    public string Language { get; }
    public DateTime ReleaseDate { get; }
    public bool IsAsia { get; }

    public string Logo
    {
        get
        {
            var path = Path.Combine(FilePaths.WwwRoot, $"images\\Logos\\{Language}\\{SetId}.png");
            if (!File.Exists(path)) path = $"images/Logos/{Language}/{SetId.SubstringBeforeLast('_')}.png"; // Return base image if region doesn't have one
            return path.SubstringAfter(FilePaths.WwwRoot);
        }
    }

    public static CardLanguage GetLanguageFromSubRegion(string subRegion)
    {
        return subRegion switch
        {
            "C" => CardLanguage.SCN,
            "F" => CardLanguage.TCN,
            "I" => CardLanguage.ID,
            "T" => CardLanguage.TH,
            "K" => CardLanguage.KO,
            _ => default
        };
    }

    public static string GetSubRegionFromLanguage(CardLanguage language)
    {
        return language switch
        {
            CardLanguage.SCN => "C",
            CardLanguage.TCN => "F",
            CardLanguage.ID => "I",
            CardLanguage.TH => "T",
            CardLanguage.KO => "K",
            _ => default
        };
    }
}