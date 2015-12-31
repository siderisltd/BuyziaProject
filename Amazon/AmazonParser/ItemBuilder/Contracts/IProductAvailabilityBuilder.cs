namespace AmazonParser.Parsers.Contracts
{
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface IProductAvailabilityBuilder
    {
        string GetAvailability(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
