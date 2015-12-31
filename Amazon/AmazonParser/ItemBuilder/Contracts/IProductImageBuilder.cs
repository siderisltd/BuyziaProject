namespace AmazonParser.Parsers.Contracts
{
    using System.Collections.Generic;
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface IProductImageBuilder
    {
        List<string> GetImages(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
