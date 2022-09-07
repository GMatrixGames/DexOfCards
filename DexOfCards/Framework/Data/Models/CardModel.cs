using System.Collections.Generic;

namespace DexOfCards.Framework.Data.Models;

public class CardModel
{
    public CardModel(string name, string set, string number, string image, string language, string style)
    {
        CardName = name;
        CardSet = set;
        CardNumber = number;
        Image = image;
        Language = language;
        Styles.Add(string.IsNullOrWhiteSpace(style) ? "Default" : style, CardImage);
    }

    public string CardName { get; }
    public string CardSet { get; }
    public string CardNumber { get; }
    private string Image { get; }
    public string Language { get; }
    public Dictionary<string, string> Styles { get; } = new();

    public string CardImage => $"images/Cards/{CardSet}/{Image}";

    public void AddToStyles(string styleName, string imageUrl)
    {
        Styles.Add(styleName, $"images/Cards/{CardSet}/{imageUrl}");
    }
}