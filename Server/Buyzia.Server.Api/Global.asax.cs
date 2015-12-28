namespace Buyzia.Server.Api
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    using Newtonsoft.Json.Serialization;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DatabaseConfig.Initialize();
        }
    }
}
