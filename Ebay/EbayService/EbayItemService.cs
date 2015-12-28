namespace EbayService
{
    using System;
    using Buyzia.Common;
    using Buyzia.Services.Contracts;
    using eBay.Service.Call;
    using eBay.Service.Core.Sdk;
    using eBay.Service.Core.Soap;

    public class EbayItemService
    {
        private IPictureService pictureService;
        private IItemService itemService;

        public EbayItemService(IPictureService pictureService, IItemService itemService)
        {
            this.pictureService = pictureService;
            this.itemService = itemService;
        }

        public void ListItem(string id)
        {
            ApiContext apiContext = EbayConnector.GetApiContext(SiteCodeType.US);

            ItemBuilder itemBuilder = new ItemBuilder(apiContext, this.pictureService, this.itemService);

            //TODO: make it for multiple items
            //TODO: add item details to build item from
            var itemGuid = new Guid(id);
            ItemType item = itemBuilder.BuildItem(this.itemService.GetItemById(itemGuid));


            AddItemCall addItemApiCall = new AddItemCall(apiContext);
            Console.WriteLine("Begin to call eBay API, please wait ...");
            FeeTypeCollection fees = addItemApiCall.AddItem(item);
            Console.WriteLine("End to call eBay API, show call result ...");
            Console.WriteLine();

            //[Step 4] Handle the result returned
            Console.WriteLine("The item was listed successfully!");
            double listingFee = 0.0;
            foreach (FeeType fee in fees)
            {
                if (fee.Name == "ListingFee")
                {
                    listingFee = fee.Fee.Value;
                }
            }
            Console.WriteLine(string.Format("Listing fee is: {0}", listingFee));
            Console.WriteLine(string.Format("Listed Item ID: {0}", item.ItemID));
        }
      
    }
}
