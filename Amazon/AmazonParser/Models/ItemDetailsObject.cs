using System.Collections.Generic;

namespace AmazonParser.Models
{
    public class ItemDetailsObject
    {
        public ItemDetailsObject()
        {
            this.Features = new List<string>();
            this.Images = new List<string>();
            this.ProductInfo = new ProductInformation();
        }

        public string Title { get; set; }

        public string OldPrice { get; set; }

        public string Price { get; set; }

        public string SaveRate { get; set; }

        public string ProductDescription { get; set; }

        public string Availability { get; set; }

        public List<string> Features { get; set; }

        public List<string> Images { get; set; }

        public ProductInformation ProductInfo { get; set; }
    }
}
