namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.ActualPrice
{
    using System;
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstActualPriceBuilder : IActualPriceBuilder
    {
        public string GetActualPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("div#price_feature_div");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElement(selector));
            var pricesElement = driver.FindElement(selector);
            var actualPrice = pricesElement.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1];

            item.Price = actualPrice;

            return actualPrice;
        }
    }
}
