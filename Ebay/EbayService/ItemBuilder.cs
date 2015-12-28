namespace EbayService
{
    using System;
    using Buyzia.Data.Models;
    using Buyzia.Services.Contracts;
    using Common;
    using eBay.Service.Core.Soap;
    using eBay.Service.Call;
    using eBay.Service.Core.Sdk;

    internal class ItemBuilder
    {

        private readonly ApiContext context;

        private readonly IPictureService pictureService;

        private readonly IItemService itemService;

        public ItemBuilder(ApiContext context, IPictureService pictureService, IItemService itemService)
        {
            this.itemService = itemService;
            this.pictureService = pictureService;
            this.context = context;
        }

        //TODO: check if item exist in the DB ---> http://developer.ebay.com/devzone/xml/docs/reference/ebay/GetItem.html#Samples
        internal ItemType BuildItem(Item itemToAdd)
        {

            ItemType item = new ItemType();

            //TODO: Build proper title
            // item title
            item.Title = itemToAdd.OriginalName;
            // item description
            //TODO: Get description || DONE!
            item.Description = this.itemService.GetItemDescriptionById(itemToAdd.Id);

            // listing type
            item.ListingType = ListingTypeCodeType.FixedPriceItem;
            // listing price
            item.Currency = CurrencyCodeType.USD;

            item.StartPrice = new AmountType();
            item.StartPrice.Value = (double)itemToAdd.SellingPrice;
            item.StartPrice.currencyID = CurrencyCodeType.USD;

            // listing duration
            item.ListingDuration = "GTC"; //Days_30 //Days_14 // Days_21

            // item location and country
            item.Location = "Whitestown";
            item.Country = CountryCodeType.US;

            // listing category, 
            CategoryType category = new CategoryType();
            var topKeywordForThisItem = "chopping board";
            var mostPopularCategory = GetMostPopularCategory(this.context, topKeywordForThisItem);

            //TODO: CHECK CATEGORY CORECTNESS 
            category.CategoryID = mostPopularCategory.Category.CategoryID; //"11104"; //CategoryID = 11104 (CookBooks) , Parent CategoryID=267(Books)
            item.PrimaryCategory = category;

            //TODO: implement quantity in siderisitem ! eg. to parse from file
            // item quality
            //TODO: Quantity on half ?
            item.Quantity = itemToAdd.AvailableQuantity / 2;

            // item condition, New
            // TODO: add enumeration
            item.ConditionID = 1000;

            //TODO: Check!!!
            // item specifics
            item.ItemSpecifics = this.GenerateItemFeatures();

            StringCollection collectionOfImages = new StringCollection();
             
            //TODO: Check for first picture ??? should be first
            foreach (var pictureUrl in this.pictureService.GetAllPictureUrlsForGivenItem(itemToAdd.Id))
            {
                collectionOfImages.Add(pictureUrl);
            }

            item.PictureDetails = new PictureDetailsType() { ExternalPictureURL = collectionOfImages };

            // payment methods
            item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
            item.PaymentMethods.AddRange(
                new BuyerPaymentMethodCodeType[] { BuyerPaymentMethodCodeType.PayPal }
                );
            // email is required if paypal is used as payment method
            item.PayPalEmailAddress = Constants.EMAIL;

            // handling time is required
            item.DispatchTimeMax = 3;
            // shipping details
            item.ShippingDetails = this.BuildShippingDetails();

            // return policy
            item.ReturnPolicy = new ReturnPolicyType();
            item.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";
            //item.ReturnPolicy.ExtendedHolidayReturnsSpecified = true;
            //item.ReturnPolicy.ExtendedHolidayReturns = true;
            item.ReturnPolicy.ShippingCostPaidBy = "Buyer";
            item.ReturnPolicy.ReturnsWithin = "Days_14";

            //item Start Price
            AmountType amount = new AmountType();
            amount.Value = (double)itemToAdd.SellingPrice;
            amount.currencyID = CurrencyCodeType.USD;
            item.StartPrice = amount;

            return item;
        }

        /// <summary>
        /// Build sample shipping details
        /// </summary>
        /// <returns>ShippingDetailsType object</returns>
        private ShippingDetailsType BuildShippingDetails()
        {
            // Shipping details
            ShippingDetailsType sd = new ShippingDetailsType();

            sd.ApplyShippingDiscount = true;
            AmountType amount = new AmountType();
            amount.Value = 0;
            amount.currencyID = CurrencyCodeType.USD;
            //sd.PaymentInstructions = "Use paypal";

            // Shipping type and shipping service options
            sd.ShippingType = ShippingTypeCodeType.Flat;

            ShippingServiceOptionsType shippingOptions = new ShippingServiceOptionsType();
            shippingOptions.ShippingService = ShippingServiceCodeType.ShippingMethodStandard.ToString(); //TODO: wtf

            amount = new AmountType();
            amount.Value = 0;
            amount.currencyID = CurrencyCodeType.USD;
            shippingOptions.ShippingServiceAdditionalCost = amount;
            amount = new AmountType();
            amount.Value = 0;
            amount.currencyID = CurrencyCodeType.USD;
            shippingOptions.ShippingServiceCost = amount;
            shippingOptions.ShippingServicePriority = 0;
            amount = new AmountType();
            amount.Value = 0;
            amount.currencyID = CurrencyCodeType.USD;
            shippingOptions.ShippingInsuranceCost = amount;

            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(new ShippingServiceOptionsType[] { shippingOptions });

            return sd;
        }

        /// <summary>
        /// Build sample item specifics
        /// </summary>
        /// <returns>ItemSpecifics object</returns>
        private NameValueListTypeCollection GenerateItemFeatures()
        {
            //TODO: Check documentation for building item specifics min 1h

            //create the content of item specifics
            NameValueListTypeCollection nvCollection = new NameValueListTypeCollection();
            NameValueListType nv1 = new NameValueListType();
            nv1.Name = "Platform";
            StringCollection nv1Col = new StringCollection();
            String[] strArr1 = new string[] { "Microsoft Xbox 360" };
            nv1Col.AddRange(strArr1);
            nv1.Value = nv1Col;
            NameValueListType nv2 = new NameValueListType();
            nv2.Name = "Genre";
            StringCollection nv2Col = new StringCollection();
            String[] strArr2 = new string[] { "Simulation" };
            nv2Col.AddRange(strArr2);
            nv2.Value = nv2Col;
            nvCollection.Add(nv1);
            nvCollection.Add(nv2);
            return nvCollection;
        }

        private static SuggestedCategoryType GetMostPopularCategory(ApiContext context, string topKeywordForThisItem)
        {
            GetSuggestedCategoriesRequestType categoryRequest = new GetSuggestedCategoriesRequestType();
            GetSuggestedCategoriesCall categoriesCall = new GetSuggestedCategoriesCall(context);
            SuggestedCategoryTypeCollection suggestedCategories = categoriesCall.GetSuggestedCategories(topKeywordForThisItem);


            SuggestedCategoryType mostPopularCategory = new SuggestedCategoryType();
            var count = 0;
            foreach (SuggestedCategoryType category in suggestedCategories)
            {
                if (count == 0)
                {
                    mostPopularCategory = category;
                }
                count++;
                if (category.PercentItemFound > mostPopularCategory.PercentItemFound)
                {
                    mostPopularCategory = category;
                }
            }
            return mostPopularCategory;
        }
    }
}
