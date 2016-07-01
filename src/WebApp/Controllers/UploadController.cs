using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApp
{
    [Authorize]
    [BasicAuthFilter]
    public class UploadController : Controller
    {
        private readonly IFtpClient ftpClient;

        public UploadController(IFtpClient ftpClient)
        {
            if (ftpClient == null)
            {
                throw new ArgumentNullException(nameof(ftpClient));
            }

            this.ftpClient = ftpClient;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new UploadViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file)
        {
            try
            {
                ftpClient.Credentials = new Credentials(User.Identity.Name, StaticData.CachedPassword);

                Action<int> action = (readBytes) =>
                {
                    HttpContext.Response.Write(readBytes.ToString() + "D"); // D = Delimiter
                    HttpContext.Response.Flush();
                };

                await ftpClient.UploadFileAsync(Path.GetFileName(file.FileName.Trim()), file.ContentLength, file.InputStream, action);

                return Content("F"); // F = Finished
            }
            catch
            {
                return Content("E"); // E = Error
            }
        }
    }
}
