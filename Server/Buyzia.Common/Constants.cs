namespace Buyzia.Common
{
    using System.Drawing.Imaging;

    public class Constants
    {
        public const int PICTURE_LONGEST_SIDE = 600;

        public const decimal PRICE_OVERCHARGE_PERCENTAGE = 0.1m;

        public static readonly ImageFormat IMAGE_FORMAT = ImageFormat.Jpeg;

        public const decimal AMAZON_TAX_PERCENTAGE = 0.08m;

        public const decimal EBAY_AND_PAYPAL_TAX_PERCENTAGE = 0.152m;
    }
}
