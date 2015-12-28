namespace Buyzia.Server.Api.Models.Items
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Data.Models;
    using Pictures;
    using Services.Helpers;

    public class ItemBindingModel
    {
        private decimal profit;

        private decimal taxes;

        private decimal sellingPrice;

        private decimal overChargePercentage;

        public ItemBindingModel()
        {
            this.profit = decimal.MinValue;
            this.taxes = decimal.MinValue;
            this.sellingPrice = decimal.MinValue;
            this.overChargePercentage = decimal.MinValue;
        }
        
        public Func<ItemBindingModel, Item> ToModel
        {
            get
            {
                return it => new Item()
                {
                    OriginalName = it.OriginalName,
                    OriginalPrice = it.OriginalPrice,
                    OriginalUrl = it.OriginalUrl,
                    SellingPrice = it.SellingPrice,
                    Taxes = it.Taxes,
                    DescriptionContent = it.DescriptionContent,
                    AvailableQuantity = it.AvailableQuantity,
                    OriginalDiscount = it.OriginalDiscount,
                    OverchargePercentage = it.OverchargePercentage,
                    Profit = it.Profit
                };
            }
        }

        [Required]
        [MaxLength(250)]
        public string OriginalUrl { get; set; }

        [Required]
        [MaxLength(100)]
        public string OriginalName { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        public decimal? OriginalDiscount { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        [Required]
        public ICollection<PicturesBindingModel> Pictures { get; set; }

        [Required]
        public ICollection<string> Features { get; set; }

        [Required]
        public string DescriptionContent { get; set; }

        public decimal Taxes
        {
            get
            {
                //for caching
                if (this.taxes == decimal.MinValue)
                {
                    this.taxes = TaxHelper.CalculateTaxes(this.OriginalPrice);
                }

                return this.taxes;
            }
        }

        public decimal SellingPrice
        {
            get
            {
                //for caching
                if (this.sellingPrice == decimal.MinValue)
                {
                    this.sellingPrice = PriceHelper.CalculateSellingPrice(this.OriginalPrice, this.Taxes);
                }

                return this.sellingPrice;
            }
        }

        public decimal Profit
        {
            get
            {
                //for caching
                if(this.profit == decimal.MinValue)
                {
                    this.profit = PriceHelper.CalculateProfit(this.OriginalPrice, this.Taxes);
                }

                return this.profit;
            }
        }

        public decimal OverchargePercentage
        {
            get
            {
                //for caching
                if (this.overChargePercentage == decimal.MinValue)
                {
                    this.overChargePercentage = this.Profit/(this.OriginalPrice + this.Taxes);
                }

                return this.overChargePercentage;
            }
        }
    }
}