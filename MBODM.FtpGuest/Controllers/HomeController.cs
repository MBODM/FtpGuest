using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest
{
    public sealed class HomeController : Controller
    {
        [HttpGet]
        [BasicAuth]
        public ActionResult Index()
        {
            return View(new HomeViewModel());
        }
    }
}
