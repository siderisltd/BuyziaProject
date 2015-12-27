namespace Buyzia.Server.Api.Controllers
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Models.Pictures;
    using Services.Contracts;

    public class PicturesController : ApiController
    {
        private readonly IPictureService pictureService;

        public PicturesController(IPictureService pictureService)
        {
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

            // MemoryStream pictureAsMemoryStream = new MemoryStream(pictureAsByteArr.ToArray());

            //// var fileStreamResult = new FileStreamResult(pictureAsMemoryStream, "image/jpeg");

            // var response = new HttpResponseMessage();
            // response.Content = new StreamContent(pictureAsMemoryStream);
            // response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            // //return this.Ok(pictureAsByteArr);
            // return response;
        }

        public IHttpActionResult Post(PicturesBindingModel model)
        {
            var resultId = this.pictureService.Add(model.ToItemId, model.Url);

            return this.Ok(resultId);
        }
    }
}