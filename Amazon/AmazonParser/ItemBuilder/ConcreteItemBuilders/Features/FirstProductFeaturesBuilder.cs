namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Features
{
    using System.Collections.Generic;
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstProductFeaturesBuilder : IProductFeaturesBuilder
    {
        public List<string> GetFeatures(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("div#feature-bullets ul li");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElements(selector));
            var featuresCollection = driver.FindElements(selector);

            List<string> productFeatures = new List<string>();
            foreach (var feat in featuresCollection)
            {
                if (feat.Text != string.Empty)
                {
                    productFeatures.Add(feat.Text);
                }
            }

            item.Features = productFeatures;

            return productFeatures;
        }
    }
}
