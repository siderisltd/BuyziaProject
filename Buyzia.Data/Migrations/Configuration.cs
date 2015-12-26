namespace Buyzia.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<Buyzia.Data.BuyziaDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BuyziaDbContext context)
        {

            context.Items.AddOrUpdate(
                it => it.Id,
                new Item
                {
                    Features = new List<Feature> {new Feature { Content = "testFeature1"}, new Feature { Content = "testFeature2"} },
                    AvailableQuantity = 3,
                    OriginalDiscount = 80,
                    OriginalName = "MyFirst Test item",
                    OriginalPrice = 11.12m,
                    OriginalUrl = "http://www.google.bg",
                    OverchargePercentage = 10,
                    SellingPrice = 14,
                    Pictures = new List<Picture> { new Picture { Content = new byte[] { 0, 1, 1, 1, 0 } } }
                });
        }
    }
}
