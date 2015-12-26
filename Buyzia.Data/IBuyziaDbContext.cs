namespace Buyzia.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;
    using Models;

    public interface IBuyziaDbContext
    {
        IDbSet<Item> Items { get; set; }

        IDbSet<Picture> Pictures { get; set; }

        IDbSet<Feature> Features { get; set; }

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}
