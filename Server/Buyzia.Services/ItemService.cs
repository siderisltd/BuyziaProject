namespace Buyzia.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data.Models;
    using Data.Repositories;
    using Common;
    using Helpers;

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
            this.itemsRepo.Add(itemToAdd);
            this.itemsRepo.SaveChanges();

            return itemToAdd.Id;
        }

        public string GetItemDescriptionById(object id)
        {
            var foundItem = this.itemsRepo.FindById(id);

            var descriptionHelper = new DescriptionHelper(foundItem, ObjectFactory.Get<IPictureService>());

            var itemDescription = descriptionHelper.GetHtmlDescription();

            return itemDescription;
        }

        public Item GetItemById(Guid id)
        {
            return this.itemsRepo.FindById(id);
        }
    }
}
