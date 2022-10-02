using System;
using System.IO;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data.Models;

public class CardSetModel
{
    public CardSetModel(string id = "???", string name = "???", string cardAmount = "???", string image = "???", string language = "???", DateTime? releaseDate = default)
    {
        SetId = id;
        Language = Enum.Parse<CardLanguage>(language);
        IsAsia = Language != CardLanguage.NonAsia;
        SetNameNoLang = name;
        SetName = $"{SetNameNoLang} {(IsAsia ? $"({Language})" : "")}";
        CardsInSet = cardAmount;
        SetImage = image is "Promo_Asia.png" or "Promo.png" ? $"images/Sets/{image}" : $"images/Sets/{Language}/{image}";
        if (IsAsia && !File.Exists(Path.Combine(FilePaths.WwwRoot, SetImage.Replace("/", @"\"))))
            SetImage = $"images/Sets/JP/{image}";
        ReleaseDate = releaseDate ?? DateTime.Now;
    }

    public string SetId { get; }
    public string SetNameNoLang { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    public string SetImage { get; }
    public CardLanguage Language { get; }
    public DateTime ReleaseDate { get; }
    public bool IsAsia { get; }

    public string Logo
    {
        get
        {
            var path = Path.Combine(FilePaths.WwwRoot, $"images\\Logos\\{Language}\\{SetId}.png");
            if (IsAsia && !File.Exists(path)) path = $"images/Logos/JP/{SetId}.png"; // Return JP image if region doesn't have one
            return path.SubstringAfter(FilePaths.WwwRoot);
        }
    }
}