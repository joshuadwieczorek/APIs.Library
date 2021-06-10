using System.Collections.Generic;

namespace APIs.Library.Caching
{
    public static class GoogleAccountsCache
    {
        public static List<Database.Accounts.Domain.accounts.Google> Accounts { get; } =
            new List<Database.Accounts.Domain.accounts.Google>();
    }
}