namespace Buyzia.Server.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Data.Models;
    using EbayService.Contracts;
    using Models.Items;
    using Services.Contracts;

    //TODO: Add validations
    [RoutePrefix("api/items")]
    public class ItemsController : ApiController
    {
        private IItemService itemService;
        private IPictureService pictureService;
        private IEbayItemService ebayItemService;

        public ItemsController(IItemService itemService, IPictureService pictureService, IEbayItemService ebayItemService)
        {
            this.itemService = itemService;
            this.pictureService = pictureService;
            this.ebayItemService = ebayItemService;
        }

        public IHttpActionResult Post(ItemBindingModel model)
        {
            Item itemToAdd = model.ToModel(model);

            foreach (var itemFeature in model.Features)
            {
                itemToAdd.Features.Add(new Feature() {ItemId = itemToAdd.Id, Content = itemFeature});
            }
            //Should be added in order to add pictures with existing itemID
            this.itemService.Add(itemToAdd);

            foreach (var pictureModel in model.Pictures)
            {
                this.pictureService.Add(itemToAdd.Id, pictureModel.Url, pictureModel.isMainPicture);
            }

            return this.Ok(itemToAdd.Id);
        }
         
        [Route("getDescriptionById")]
        public IHttpActionResult Get(string id)
        {
            var itemGuid = new Guid(id);
            var description = this.itemService.GetItemDescriptionById(itemGuid);
            return this.Ok(description);
        }

        [HttpGet]
        [Route("listInEbayByItemId")]
        public IHttpActionResult ListItemToEbay(string id)
        {
            //TODO: Add ViewModel To Display fees
            this.ebayItemService.ListItem(id);

            return this.Ok();
        }
    } 
}