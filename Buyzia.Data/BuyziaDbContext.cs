namespace Buyzia.Data
{
    using System.Data.Entity;
    using Models;

    public class BuyziaDbContext : DbContext, IBuyziaDbContext
    {
        public BuyziaDbContext()
            : base("BuyziaDb")
        {
        }

        public IDbSet<Item> Items { get; set; }

        public IDbSet<Picture> Pictures { get; set; }

        public IDbSet<Feature> Features { get; set; }
    }
}
