namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Images
{
    using System.Collections.Generic;
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstImageBuilder : IProductImageBuilder
    {
        public List<string> GetImages(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("span.a-button-text img");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElements(selector));
            var imgsCollection = driver.FindElements(selector);

            List<string> productImages = new List<string>();
            foreach (var img in imgsCollection)
            {
                var src = img.GetAttribute("src");
                productImages.Add(src);
            }

            item.Images = productImages;
            return productImages;
        }
    }
}
