namespace AmazonParser.SeleniumExtensions
{
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public interface ICustomWebDriver : IWebDriver
    {
        ItemDetailsObject ParseAmazonProduct(string url, WebDriverWait wait);
    }
}
