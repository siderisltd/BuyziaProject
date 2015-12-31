namespace AmazonParser.Parsers.Contracts
{
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface ISavedRateBuilder
    {
        string GetSavedRate(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
