namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.OldPrice
{
    using System;
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstOldPriceBuilder : IOldPriceBuilder
    {
        public string GetOldPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("div#price_feature_div");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElement(selector));
            var pricesElement = driver.FindElement(selector);
            var oldPrice = pricesElement.Text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)[0];

            item.OldPrice = oldPrice;

            return oldPrice;
        }
    }
}
