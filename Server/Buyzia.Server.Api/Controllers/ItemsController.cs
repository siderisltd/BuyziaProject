namespace Buyzia.Server.Api.Controllers
{
    using System.Web.Http;
    using Data.Models;
    using Models.Items;
    using Services.Contracts;

    public class ItemsController : ApiController
    {
        private IItemService itemService;
        private IPictureService pictureService;

        public ItemsController(IItemService itemService, IPictureService pictureService)
        {
            this.itemService = itemService;
            this.pictureService = pictureService;
        }

        public IHttpActionResult Post(ItemBindingModel model)
        {
            Item itemToAdd = model.ToModel(model);

            foreach (var itemFeature in model.Features)
            {
                itemToAdd.Features.Add(new Feature() {ItemId = itemToAdd.Id, Content = itemFeature});
            }

            this.itemService.Add(itemToAdd);

            foreach (var pictureModel in model.Pictures)
            {
                this.pictureService.Add(itemToAdd.Id, pictureModel.Url, pictureModel.isMainPicture);
            }

            return this.Ok(itemToAdd.Id);
        }
    }
}