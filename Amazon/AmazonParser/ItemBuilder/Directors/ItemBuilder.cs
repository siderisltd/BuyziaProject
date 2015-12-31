namespace AmazonParser.ItemBuilder.Directors
{
    using System.Collections.Generic;
    using ConcreteItemBuilders.ActualPrice;
    using ConcreteItemBuilders.Availability;
    using ConcreteItemBuilders.Description;
    using ConcreteItemBuilders.Features;
    using ConcreteItemBuilders.Images;
    using ConcreteItemBuilders.OldPrice;
    using ConcreteItemBuilders.ProductInfo;
    using ConcreteItemBuilders.SavedRate;
    using ConcreteItemBuilders.Title;
    using Models;
    using OpenQA.Selenium.Support.UI;
    using Parsers.Contracts;
    using SeleniumExtensions;

    public abstract class ItemBuilder
    {
        protected ItemBuilder(ICustomWebDriver driver, string url, WebDriverWait wait)
        {
            this.Driver = driver;
            this.Url = url;
            this.Wait = wait;

            this.ProductTitleBuilders = new HashSet<ITitleBuilder>()
            {
                new FirstTitleBuilder(),
            };
            this.ProductOldPriceBuilders = new HashSet<IOldPriceBuilder>()
            {
                new FirstOldPriceBuilder(),
            };
            this.ProductActualPriceBuilders = new HashSet<IActualPriceBuilder>()
            {
                new FirstActualPriceBuilder(),
            };
            this.ProductSavedRateBuilders = new HashSet<ISavedRateBuilder>()
            {
                new FirstSavedRateBuilder(),
            };
            this.ProductDescriptionBuilders = new HashSet<IProductDescriptionBuilder>()
            {
                new FirstDescriptionBuilder(),
            };
            this.ProductAvailabilityBuilders = new HashSet<IProductAvailabilityBuilder>()
            {
                new FirstAvailabilityBuilder(),
            };
            this.ProductInfoBuilders = new HashSet<IProductInfoBuilder>()
            {
                new FirstProductInfoBuilder(),
            };
            this.ProductFeaturesBuilders = new HashSet<IProductFeaturesBuilder>()
            {
                new FirstProductFeaturesBuilder(),
            };
            this.ProductImagesBuilders = new HashSet<IProductImageBuilder>()
            {
                new FirstImageBuilder(),
            };

            //TODO: Add the actual builders innitially in the collections
        }

        protected WebDriverWait Wait { get; set; }

        protected ICustomWebDriver Driver { get; set; }

        private string Url { get; set; }

        private string InnitialWindowHtml;



        protected ICollection<ITitleBuilder> ProductTitleBuilders { get; set; }

        protected ICollection<IOldPriceBuilder> ProductOldPriceBuilders { get; set; }

        protected ICollection<IActualPriceBuilder> ProductActualPriceBuilders { get; set; }

        protected ICollection<ISavedRateBuilder> ProductSavedRateBuilders { get; set; }

        protected ICollection<IProductDescriptionBuilder> ProductDescriptionBuilders { get; set; }

        protected ICollection<IProductAvailabilityBuilder> ProductAvailabilityBuilders { get; set; }

        protected ICollection<IProductFeaturesBuilder> ProductFeaturesBuilders { get; set; }

        protected ICollection<IProductImageBuilder> ProductImagesBuilders { get; set; }

        protected ICollection<IProductInfoBuilder> ProductInfoBuilders { get; set; }

        public virtual ItemDetailsObject BuildItem()
        {
            this.Driver.Navigate().GoToUrl(this.Url);
            this.InnitialWindowHtml = this.Driver.CurrentWindowHandle;

            var item = new ItemDetailsObject();

            //Does the order matter ?  maybe yes!
            var customWebDriver = this.Driver;

            item = this.GetProductDesctiption(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetTitle(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetOldPrice(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetActualPrice(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetSaveRate(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetProductAvailability(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetProductFeatures(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetProductImages(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);
            item = this.GetProductInfo(ref item, ref customWebDriver, this.Wait, this.Url, ref this.InnitialWindowHtml);

            return item;
        }

        protected abstract ItemDetailsObject GetTitle(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetOldPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetActualPrice(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetSaveRate(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetProductDesctiption(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetProductAvailability(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetProductFeatures(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetProductImages(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);

        protected abstract ItemDetailsObject GetProductInfo(ref ItemDetailsObject item, ref ICustomWebDriver driver, WebDriverWait wait, string url, ref string innitialWindowHtml);
    }
}
