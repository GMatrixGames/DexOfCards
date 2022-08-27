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
        Image = image;
        LanguageStrings = languages.Contains(',') ? languages.Split(',') : new[] { languages };
        SubRegion = id.SubstringAfterLast('_');
    }

    public string SetId { get; }
    public string SetName { get; }
    public string CardsInSet { get; }
    public string SubRegion { get; }
    private string Image { get; }
    private string[] LanguageStrings { get; }

    public IList<string> SetImages
    {
        get
        {
            var ret = new List<string>();

            
            
            return ret;
        }
    }
}