using System.Collections.Generic;
using System.Linq;

namespace DexOfCards.Framework.Data.Models;

public class CardSetModel
{
    public CardSetModel(string id, string name, string cardAmount, string image, string languages)
    {
        SetId = id;
        SetName = name;
        CardsInSet = cardAmount;
        Image = image;
        LanguageStrings = languages.Contains(',') ? languages.Split(',') : new[] { languages };
    }

    public string SetId { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    private string Image { get; }
    private string[] LanguageStrings { get; }
    private bool IsAsia => !LanguageStrings.Contains("NonAsia");

    public IList<string> SetImages => LanguageStrings.Select(lang => $"images/Sets/{(IsAsia ? "Asia" : "NonAsia")}/{Image.Replace("{LANG}", lang)}").ToList();
}