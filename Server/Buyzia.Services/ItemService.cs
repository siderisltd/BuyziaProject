namespace Buyzia.Services
{
    using System;
    using System.Linq;
    using Contracts;
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

        public Guid Add(Item itemToAdd)
        {
            //TODO: Return the ID of the added item!
            this.itemsRepo.Add(itemToAdd);
            this.itemsRepo.SaveChanges();

            return itemToAdd.Id;
        }
    }
}
