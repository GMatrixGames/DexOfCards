﻿using System.IO;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data.Models;

public class CardSetModel
{
    public CardSetModel(string id = "???", string name = "???", string cardAmount = "???", string image = "???", string languages = "???")
    {
        SetId = id;
        SetName = name;
        CardsInSet = cardAmount;
        Languages = languages.Contains(',') ? languages.Split(',') : new[] { languages };
        SubRegion = SetId.Contains('_') ? SetId.SubstringAfterLast('_') : null;
        SetImage = $"images/Sets/{(languages != "NonAsia" ? string.IsNullOrWhiteSpace(SubRegion) ? SetId + "/" : SubRegion + "/" : "")}{image}";
    }

    public string SetId { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    public string SubRegion { get; }
    public string SetImage { get; }
    public string[] Languages { get; }

    public string Logo
    {
        get
        {
            var path = Path.Combine(FilePaths.WwwRoot, $"images\\Logos\\{SetId}.png");
            if (!File.Exists(path)) path = $"images/Logos/{SetId.SubstringBeforeLast('_')}.png"; // Return base image if region doesn't have one
            return path.SubstringAfter(FilePaths.WwwRoot);
        }
    }

    public static string GetLanguageFromSubRegion(string subRegion)
    {
        return subRegion switch
        {
            "F" => "CN",
            "I" => "ID",
            "T" => "TH",
            _ => default
        };
    }

    public static string GetSubRegionFromLanguage(string language)
    {
        return language switch
        {
            "CN" => "F",
            "ID" => "I",
            "T" => "TH",
            _ => default
        };
    }
}