using System;

namespace Buyzia.Services.Helpers
{
    using Common;

    public static class TaxHelper
    {
        public static decimal CalculateTaxes(decimal originalPrice)
        {
            var amazonTaxes = originalPrice * Constants.AMAZON_TAX_PERCENTAGE;
            var ebayAndPaypalTaxes = (originalPrice * Constants.EBAY_AND_PAYPAL_TAX_PERCENTAGE);

            var allTaxes = amazonTaxes + ebayAndPaypalTaxes;

            return allTaxes;
        }
    }
}
