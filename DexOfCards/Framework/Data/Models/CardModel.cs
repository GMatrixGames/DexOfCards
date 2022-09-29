using System.Collections.Generic;
using System.Linq;

namespace DexOfCards.Framework.Data.Models;

public class CardModel
{
    public CardModel(string name, string set, string number, string image, string language, IEnumerable<string> styles)
    {
        CardName = name;
        CardSet = DataStorage.GetSet(set);
        CardNumber = number;
        Image = image;
        Language = language;
        Styles.Add("Default");

        foreach (var style in styles.Where(style => !string.IsNullOrWhiteSpace(style)))
        {
            Styles.Add(style);
        }
    }

    public string CardName { get; }
    public CardSetModel CardSet { get; }
    public string CardNumber { get; }
    private string Image { get; }
    public string Language { get; }
    public List<string> Styles { get; } = new();

    public string CardImage => $"images/Cards/{CardSet.Language}/{CardSet.SetId}/{Image}";

    public bool IsEnergy => CardNumber
        is "PSY" or "DAR" or "FIG" or "FIR" or "GRA" or "LIG" or "MET" or "WAT" or "FAI";
}