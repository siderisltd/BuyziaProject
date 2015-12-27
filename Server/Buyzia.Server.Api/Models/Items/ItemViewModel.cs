namespace Buyzia.Server.Api.Models.Items
{
    using System;
    using System.Linq.Expressions;
    using Data.Models;
    using Services.Helpers;
    using System.Linq;

    public class ItemViewModel
    {
        public static Expression<Func<Item, ItemViewModel>> FromModel
        {
            get
            {
                return it => new ItemViewModel()
                { 
                     OriginalName = it.OriginalName,
                     OriginalPrice = it.OriginalPrice,
                     OriginalUrl = it.OriginalUrl,   
                     SellingPrice = it.SellingPrice,
                     Profit = it.Profit,
                     MainPicture = it.Pictures.FirstOrDefault(x => x.IsMainPicture).Content
                };
            }
        }

        public string OriginalName { get; set; }

        public decimal OriginalPrice { get; set; }

        public string OriginalUrl { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Profit { get; set; }

        public byte[] MainPicture { get; set; }
    }
}