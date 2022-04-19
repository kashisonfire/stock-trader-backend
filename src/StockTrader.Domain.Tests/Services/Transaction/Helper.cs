using StockTrader.Domain.Models;
using System;
using System.Collections.Generic;

namespace StockTrader.Domain.Tests.Services.Transaction
{
    public static class Helpers
    {
        public static Account CreateAccount(int balance) => new()
        {
            Balance = balance,
            AssetTransactions = new List<AssetTransaction>()
        };

        public static AssetTransaction CreateAssetTransaction(Account account, bool isPurchase, int stockPrice, string symbol, int shares) => new()
        {
            Account = account,
            Asset = new Asset()
            {
                PricePerShare = stockPrice,
                Symbol = symbol
            },
            DateProcessed = DateTime.Now,
            IsPurchase = isPurchase,
            Shares = shares
        };
    }
}