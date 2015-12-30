namespace AmazonWalker.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TorProject;

    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var url = "http://www.amazon.com/b/ref=lp_1063310_ln_1?node=3733331&ie=UTF8&qid=1451301590";
            var whatIs = "http://whatismyipaddress.com/";

            var amlink =
                "http://www.amazon.com/Cuisinart-SS-700-Single-Brewing-System/dp/B0034J6QIY/ref=pd_sim_79_4?ie=UTF8&dpID=41n8xkin0sL&dpSrc=sims&preST=_AC_UL160_SR125%2C160_&refRID=1QRV0FMVGT2X6D2M8KH5";

            var tr = new TorRequests();
            tr.StartTor(whatIs);

            sw.Stop();
            Console.WriteLine();

            return View();
        }
    }
}