using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using DexOfCards.Framework.Data.Models;
using DexOfCards.Framework.Storage;
using DexOfCards.Utilities;

namespace DexOfCards.Framework.Data;

public static class UserDataStorage
{
    // public static async Task<int> AddNewCard(CardModel model, string style)
    // {
    //     var currentAmount = await GetOwnedAmount(model);
    //     var addedOne = currentAmount + 1;
    //     await using var conn = SQLite.GetUserDataSql();
    //     conn.ExecuteNoQuery($"UPDATE owned_cards SET amount = @amount WHERE cardSet = '{model.CardSet}' AND cardNumber = '{model.CardNumber}'", new[]
    //     {
    //         new SQLiteParameter("@amount", addedOne)
    //     });
    //     return addedOne;
    // }
    //
    // public static async Task<int> RemoveCard(CardModel model, string style)
    // {
    //     var currentAmount = await GetOwnedAmount(model);
    //     var removedOne = currentAmount - 1;
    //     await using var conn = SQLite.GetUserDataSql();
    //     conn.ExecuteNoQuery($"UPDATE owned_cards SET amount = @amount WHERE cardSet = '{model.CardSet}' AND cardNumber = '{model.CardNumber}'", new[]
    //     {
    //         new SQLiteParameter("@amount", removedOne)
    //     });
    //     return removedOne;
    // }
    //
    // public static async Task<int> GetOwnedAmount(CardModel model, string style)
    // {
    //     var amount = 0;
    //     if (model == null) return amount;
    //     await using var conn = SQLite.GetUserDataSql();
    //     var read = await conn.ExecuteReaderAsync($"SELECT * FROM owned_cards WHERE cardSet = '{model.CardSet}' AND cardNumber = '{model.CardNumber}' ");
    //     if (await read.ReadAsync()) amount = read.GetInt32("amount");
    //     return amount;
    // }
}