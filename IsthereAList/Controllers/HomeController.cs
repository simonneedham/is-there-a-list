using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsThereAList.Controllers
{
    [Authorize]
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        [Route()]
        //[Route("/")]
        [Route("~/")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}