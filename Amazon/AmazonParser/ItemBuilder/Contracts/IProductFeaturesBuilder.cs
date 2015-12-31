namespace AmazonParser.Parsers.Contracts
{
    using System.Collections.Generic;
    using Models;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    public interface IProductFeaturesBuilder
    {
        List<string> GetFeatures(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
