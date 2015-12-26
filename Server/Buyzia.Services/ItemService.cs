namespace Buyzia.Services
{
    using System.Linq;
    using Data.Models;
    using Data.Repositories;

    public class ItemService : IItemService
    {
        private readonly IRepository<Item> itemsRepo;

        public ItemService(IRepository<Item> itemsRepo)
        {
            this.itemsRepo = itemsRepo;
        }

        public IQueryable<Item> All()
        {
            return this.itemsRepo.All();
        }
    }
}
