using System;

namespace DexOfCards.Framework.Data.Models;

public class OwnedCardModel
{
    public OwnedCardModel(string set, string number, string language, string variant, int amount)
    {
        Language = Enum.Parse<CardLanguage>(language);
        CardSet = DataStorage.GetSet(set, Language);
        CardNumber = number;
        Style = variant;
        Amount = amount;
    }

    public CardSetModel CardSet { get; }
    public string CardNumber { get; }
    public CardLanguage Language { get; }
    public string Style { get; }
    public int Amount { get; }
}