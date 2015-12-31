namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.SavedRate
{
    using System;
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstSavedRateBuilder : ISavedRateBuilder
    {
        public string GetSavedRate(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("div#price_feature_div");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElement(selector));
            var pricesElement = driver.FindElement(selector);
            var savedRate = pricesElement.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[2];

            item.SaveRate = savedRate;

            return savedRate;
        }
    }
}
