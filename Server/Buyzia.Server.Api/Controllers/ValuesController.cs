namespace Buyzia.Server.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Linq;
    using System.Web.Helpers;
    using Services;

    public class ValuesController : ApiController
    {
        private IItemService itemService;

        public ValuesController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var result = this.itemService
                             .All()
                             .FirstOrDefault();

            return this.Json(new
            {
                Id = result.Id,
                OriginalName = result.OriginalName
            });
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
