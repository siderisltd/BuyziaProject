namespace Buyzia.Server.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using System.Linq;
    using Models.Pictures;
    using Services.Contracts;

    [RoutePrefix("api/pictures")]
    public class PicturesController : ApiController
    {
        private readonly IPictureService pictureService;

        private readonly IItemService itemService;

        public PicturesController(IPictureService pictureService, IItemService itemService)
        {
            this.itemService = itemService;
            this.pictureService = pictureService;
        }

        public HttpResponseMessage Get(int id)
        {
            byte[] pictureAsByteArr = this.pictureService.GetPictureById(id);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(pictureAsByteArr);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            var disposition = new ContentDispositionHeaderValue("attachment");
            //TODO: Add item name as fileName
            disposition.FileName = "ImageDocument.jpeg";
            response.Content.Headers.ContentDisposition = disposition;

            return response;
        }

        [HttpGet]
        [Route("getAllPicturesByItemId")]
        public IHttpActionResult GetAllPictures(string id)
        {
            var itemGuid = new Guid(id);
            ICollection<string> pictureUrls = this.pictureService.GetAllPictureUrlsForGivenItem(itemGuid);
            return this.Ok(pictureUrls);
        }
        
        [HttpGet]
        [Route("getMainPictureByItemId")]
        public IHttpActionResult GetMainPicture(string id)
        {
            var itemGuid = new Guid(id);
            var mainPicture = this.pictureService.GetMainPictureLinkByItemId(itemGuid);

            return this.Ok(mainPicture);
        }
    }
}