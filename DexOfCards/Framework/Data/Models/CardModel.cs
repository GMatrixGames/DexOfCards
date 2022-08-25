namespace DexOfCards.Framework.Data.Models;

public class CardModel
{
    public CardModel(string name, string set, string number, string image, string language)
    {
        CardName = name;
        CardSet = set;
        CardNumber = number;
        Image = image;
        Language = language;
    }

    public string CardName { get; }
    public string CardSet { get; }
    public string CardNumber { get; }
    private string Image { get; }
    public string Language { get; }

    public string CardImage => $"images/Cards/{CardSet}/{Image}";
}