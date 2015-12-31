namespace AmazonParser.ItemBuilder.Directors
{
    using Models;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    public class AmazonItemBuilder : ItemBuilder
    {
        public AmazonItemBuilder(ICustomWebDriver driver, string url, WebDriverWait wait)
            : base(driver, url, wait)
        {
        }

        protected override ItemDetailsObject GetTitle(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (ITitleBuilder titleBuilder in this.ProductTitleBuilders)
            {
                string title = titleBuilder.GetTitle(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(title))
                {
                    //the title is parsed
                    item.Title = title;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetOldPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IOldPriceBuilder oldPriceBuilder in this.ProductOldPriceBuilders)
            {
                string oldPrice = oldPriceBuilder.GetOldPrice(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(oldPrice))
                {
                    item.OldPrice = oldPrice;
                    return item;
                }
            }
            return item;
        }

        protected override ItemDetailsObject GetActualPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IActualPriceBuilder actualPriceBuilder in this.ProductActualPriceBuilders)
            {
                string actualPrice = actualPriceBuilder.GetActualPrice(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(actualPrice))
                {
                    item.Price = actualPrice;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetSaveRate(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (ISavedRateBuilder savedRateBuilder in this.ProductSavedRateBuilders)
            {
                string savedRate = savedRateBuilder.GetSavedRate(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(savedRate))
                {
                    item.SaveRate = savedRate;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetProductDesctiption(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IProductDescriptionBuilder productDescriptionBuilder in this.ProductDescriptionBuilders)
            {
                string productDescription = productDescriptionBuilder.GetDescription(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(productDescription))
                {
                    item.ProductDescription = productDescription;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetProductAvailability(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IProductAvailabilityBuilder productAvailabilityBuilder in this.ProductAvailabilityBuilders)
            {
                string productAvailability = productAvailabilityBuilder.GetAvailability(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (!string.IsNullOrEmpty(productAvailability))
                {
                    item.Availability = productAvailability;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetProductFeatures(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IProductFeaturesBuilder productFeaturesBuilder in this.ProductFeaturesBuilders)
            {
                var features = productFeaturesBuilder.GetFeatures(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (features != null)
                {
                    item.Features = features;
                    return item;
                }
            }

            return item;
        }

        protected override ItemDetailsObject GetProductImages(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IProductImageBuilder productImageBuilder in this.ProductImagesBuilders)
            {
                var images = productImageBuilder.GetImages(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (images != null)
                {
                    item.Images = images;
                    return item;
                }
            }

            return item;
        }

        //TODO: Check it
        protected override ItemDetailsObject GetProductInfo(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml)
        {
            foreach (IProductInfoBuilder productInfoBuilder in this.ProductInfoBuilders)
            {
                var productInfo = productInfoBuilder.GetProductInfo(ref item, ref driver, wait, url, ref innitialWindowHtml);

                if (productInfo != null)
                {
                    item.ProductInfo = productInfo;
                    return item;
                }
            }

            return item;
        }
    }
}
