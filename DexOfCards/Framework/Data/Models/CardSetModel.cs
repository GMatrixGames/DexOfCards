using System.Collections.Generic;
using System.Linq;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data.Models;

public class CardSetModel
{
    public CardSetModel(string id, string name, string cardAmount, string image, string languages)
    {
        SetId = id;
        SetName = name;
        CardsInSet = cardAmount;
        SetImage = image;
        Languages = languages.Contains(',') ? languages.Split(',') : new[] { languages };
        SubRegion = id.SubstringAfterLast('_');
    }

    public string SetId { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    public string SubRegion { get; }
    public string SetImage { get; }
    public string[] Languages { get; }
}