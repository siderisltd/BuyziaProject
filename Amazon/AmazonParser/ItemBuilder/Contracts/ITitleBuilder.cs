namespace AmazonParser.Parsers.Contracts
{
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface ITitleBuilder
    {
        string GetTitle(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
