namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.ProductInfo
{
    using Helpers;
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    internal class FirstProductInfoBuilder : IProductInfoBuilder
    {
        public ProductInformation GetProductInfo(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            var selector = By.CssSelector("table#productDetails_detailBullets_sections1 > tbody > tr > td");

            var elementExist = DriverHelper.Instance.CheckIfElementExists(driver, selector);
            if (!elementExist) { return null; }

            wait.Until(x => x.FindElements(selector));
            var productInformationElements = driver.FindElements(selector);

            ProductInformation productInfo = new ProductInformation();
            productInfo.ProductDimensions = productInformationElements[0].Text;
            productInfo.ItemWeight = productInformationElements[1].Text;
            productInfo.ShippingWeight = productInformationElements[2].Text;
            productInfo.Department = productInformationElements[3].Text;
            productInfo.Manufacturer = productInformationElements[4].Text;
            productInfo.ASIN = productInformationElements[5].Text;
            productInfo.DomesticShipping = productInformationElements[6].Text;
            productInfo.InternationalShiping = productInformationElements[7].Text;
            productInfo.Origin = productInformationElements[8].Text;
            productInfo.ShippingAdvisory = productInformationElements[9].Text;
            productInfo.ItemModelNumber = productInformationElements[10].Text;
            productInfo.CustomerReviews = productInformationElements[11].Text;
            productInfo.BestSellerRate = productInformationElements[12].Text;

            item.ProductInfo = productInfo;
            return productInfo;
        }
    }
}
