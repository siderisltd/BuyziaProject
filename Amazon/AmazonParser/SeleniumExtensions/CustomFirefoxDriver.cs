namespace AmazonParser.SeleniumExtensions
{
    using ItemBuilder.Directors;
    using Models;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;

    public class CustomFirefoxDriver : FirefoxDriver, ICustomWebDriver
    {
        public CustomFirefoxDriver(FirefoxProfile profile)
            : base(profile)
        {
        }

        public ItemDetailsObject ParseAmazonProduct(string url, WebDriverWait wait)
        {

            ItemBuilder builder = new AmazonItemBuilder(this, url, wait);
            var item = builder.BuildItem();

            return item;
        }
    }
}
