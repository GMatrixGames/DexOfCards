namespace DexOfCards.Framework.Data;

public enum UserDataStorageVersion
{
    Initial,

    LatestPlusOne,
    Latest = LatestPlusOne - 1
}