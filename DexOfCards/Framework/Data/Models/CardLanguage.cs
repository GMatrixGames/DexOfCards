using System.ComponentModel;

namespace DexOfCards.Framework.Data.Models;

public enum CardLanguage
{
    [Description("Global")]
    // Everywhere but Asia lol
    NonAsia,

    [Description("Japanese")]
    // Japan (Japanese)
    JP,

    [Description("Chinese (Sim.)")]
    // Mainland China (Simplified Chinese)
    SCN,

    [Description("Chinese (Trad.)")]
    // Hong Kong & Taiwan (Traditional Chinese)
    TCN,

    [Description("Indonesian")]
    // Indonesia (Indonesian)
    ID,

    [Description("Thai")]
    // Thailand (Thai)
    TH,

    [Description("Korean")]
    // South Korea (Korean)
    KO
}