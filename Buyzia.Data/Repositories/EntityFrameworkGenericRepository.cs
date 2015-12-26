namespace Buyzia.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
          where TEntity : class
    {
        private readonly IBuyziaDbContext data;

        private readonly IDbSet<TEntity> set;

        public EntityFrameworkRepository()
            : this(new BuyziaDbContext())
        {
        }

        public EntityFrameworkRepository(IBuyziaDbContext data)
        {
            this.data = data;
            this.set = data.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return this.set;
        }

        public TEntity FindById(object id)
        {
            return this.set.Find(id);
        }

        public void Add(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }


        public void Remove(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        private void ChangeState(TEntity entity, EntityState state)
        {
            var dbEntry = this.data.Entry(entity);
            dbEntry.State = state;
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
