using System;

namespace Buyzia.Services.Helpers
{
    using Common;

    public class PriceHelper
    {
        private static decimal profit = decimal.MinValue;


        public static decimal CalculateSellingPrice(decimal originalPrice, decimal taxes)
        {
            var profit = CalculateProfit(originalPrice, taxes);
            var sellingPrice = originalPrice + taxes + profit;

            sellingPrice = Math.Round(sellingPrice) - 0.1m;

            if(sellingPrice < (originalPrice + taxes))
            {
                throw new InvalidOperationException(string.Format("SELLING PRICE IS LOWER THAN BUYING PRICE PLUS TAXES \r\n" +
                                                                  "sellingPrice = {0} \r\n" +
                                                                  "buyingPricePlusTaxes = {1}", sellingPrice, originalPrice + taxes));
            }

            return sellingPrice;
        }

        public static decimal CalculateProfit(decimal originalPrice, decimal taxes)
        {
            var originalPricePlusTaxes = originalPrice + taxes;
            profit = Constants.PRICE_OVERCHARGE_PERCENTAGE * originalPricePlusTaxes;

            return profit;
        }
    }
}
