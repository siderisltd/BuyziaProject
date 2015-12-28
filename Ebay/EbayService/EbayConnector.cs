namespace EbayService
{
    using Common;
    using eBay.Service.Core.Sdk;
    using eBay.Service.Core.Soap;

    internal static class EbayConnector
    {
        private static ApiContext apiContext = null;

        internal static ApiContext GetApiContext(SiteCodeType sitePoint)
        {
            //apiContext is a singleton,
            //to avoid duplicate configuration reading
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();

                //set Api Server Url
                apiContext.SoapApiServerUrl = Constants.API_SERVER_URL;

                //set Api Token to access eBay Api Server
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = Constants.API_TOKEN;
                apiCredential.ApiAccount.Application = Constants.APP_ID;
                apiCredential.ApiAccount.Developer = Constants.DEV_ID;
                apiCredential.ApiAccount.Certificate = Constants.CERT_ID;
                apiCredential.eBayAccount.UserName = Constants.USERNAME;
                apiCredential.eBayAccount.Password = Constants.PASS;

                apiContext.ApiCredential = apiCredential;

                //set eBay Site target to US
                apiContext.Site = sitePoint;

                return apiContext;
            }
        }
    }
}
