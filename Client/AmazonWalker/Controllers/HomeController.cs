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
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            var url = "http://www.amazon.com/b/ref=lp_1063310_ln_1?node=3733331&ie=UTF8&qid=1451301590";
            var whatIs = "http://whatismyipaddress.com/";

            var amlink =
                "http://www.amazon.com/Altra-Furniture-Carson-50-Inches-Espresso/dp/B005KUVZEA/ref=sr_1_1?s=home-garden&ie=UTF8&qid=1451487776&sr=1-1&keywords=tv";


            TorRequester torRequester = new TorRequester();
            torRequester.BrowserRequest(amlink);


            //var tr = new TorRequests();
            //tr.StartTor(whatIs);

            //sw.Stop();
            //Console.WriteLine();

            return View();
        }
    }
}