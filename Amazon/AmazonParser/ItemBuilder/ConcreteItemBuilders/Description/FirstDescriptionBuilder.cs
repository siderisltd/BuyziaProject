namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Description
{
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstDescriptionBuilder : IProductDescriptionBuilder
    {
        public string GetDescription(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.Id("product-description-iframe");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            driver.SwitchTo().Frame(driver.FindElement(selector));
            var expression = By.ClassName("content");

            wait.Until(x => x.FindElement(expression));
            var frameElement = driver.FindElement(expression);

            var productDescription = frameElement.Text;
            item.ProductDescription = productDescription;



            return productDescription;
        }
    }
}
