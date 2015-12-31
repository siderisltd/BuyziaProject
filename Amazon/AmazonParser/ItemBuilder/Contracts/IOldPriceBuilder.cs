namespace AmazonParser.Parsers.Contracts
{
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface IOldPriceBuilder
    {
        string GetOldPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
