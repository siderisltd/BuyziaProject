namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Title
{
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstTitleBuilder : ITitleBuilder
    {
        public string GetTitle(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.Id("productTitle");
            driver.SwitchTo().Window(innitialWindowHtml);


            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElement(selector));

            var itemTitle = driver.FindElement(selector).Text;
            item.Title = itemTitle;

            return itemTitle;
        }
    }
}
