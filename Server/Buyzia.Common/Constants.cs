namespace Buyzia.Common
{
    using System.Drawing.Imaging;
    using System.Web;

    public class Constants
    {
        public const int PICTURE_LONGEST_SIDE = 600;

        public const decimal PRICE_OVERCHARGE_PERCENTAGE = 0.1m;

        public static readonly ImageFormat IMAGE_FORMAT = ImageFormat.Jpeg;

        public const decimal AMAZON_TAX_PERCENTAGE = 0.08m;

        public const decimal EBAY_AND_PAYPAL_TAX_PERCENTAGE = 0.152m;

        public const int MINIMUM_KEYWORD_LENGTH = 4;

        public const string SERVER_URL_PREFIX = "http://localhost:52255";

        public const string PICTURES_ROUTE_URL = "/api/pictures/";

        public const string DESCRIPTION_TEMPLATE_FILE_PATH = "//Files//Description//template.txt";
    }
}
