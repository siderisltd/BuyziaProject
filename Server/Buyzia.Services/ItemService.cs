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
            //TODO: Return the ID of the added item!
            this.itemsRepo.Add(itemToAdd);
            this.itemsRepo.SaveChanges();

            return itemToAdd.Id;
        }

        public string GetMainPictureLinkByItemId(object id)
        {
            Item foundItem = this.itemsRepo.FindById(id);

            if(foundItem == null)
            {
                throw new ArgumentNullException("Found item cannot be null");
            }

            var mainPicture = foundItem.Pictures.FirstOrDefault(x => x.IsMainPicture);

            if (mainPicture == null)
            {
                throw new ArgumentNullException("Main Picture cannot be null");
            }

            var mainPictureId = mainPicture.Id;

            var pictureUrl = Constants.SERVER_URL_PREFIX + Constants.PICTURES_ROUTE_URL + mainPictureId;

            return pictureUrl;
        }

        public string GetItemDescriptionById(object id)
        {
            var foundItem = this.itemsRepo.FindById(id);

            var descriptionHelper = new DescriptionHelper(foundItem, this);

            var itemDescription = descriptionHelper.GetHtmlDescription();

            return itemDescription;
        }
    }
}
