using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest.WebApp
{
    [Authorize]
    [BasicAuthFilter]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new HomeViewModel());
        }
    }
}
