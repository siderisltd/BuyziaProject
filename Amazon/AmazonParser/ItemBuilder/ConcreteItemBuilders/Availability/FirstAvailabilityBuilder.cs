namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Availability
{
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstAvailabilityBuilder : IProductAvailabilityBuilder
    {
        public string GetAvailability(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.Id("availability");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElement(selector));
            var availability = driver.FindElement(selector).Text;

            item.Availability = availability;

            return availability;
        }
    }
}
