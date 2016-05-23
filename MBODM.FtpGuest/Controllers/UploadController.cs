using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest
{
    public sealed class UploadController : Controller
    {
        [HttpGet]
        [BasicAuth]
        public ActionResult Index()
        {
            return View(new UploadViewModel());
        }

        [HttpPost]
        [BasicAuth]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if ((file == null) || (file.ContentLength <= 0) || string.IsNullOrEmpty(file.FileName))
            {
                return Content($"Error: Parameter '{nameof(file)}' was null or empty.");
            }

            // Internet Explorer needs Path.GetFileName()
            var fileName = Path.GetFileName(file.FileName);

            if (fileName.ToLower() == AppInfos.TrashFolder.ToLower())
            {
                return Content($"Error: Upload of a file named '{AppInfos.TrashFolder}' is not allowed.");
            }

            try
            {
                var request = (FtpWebRequest)WebRequest.Create($"{AppInfos.FtpServer}/{fileName}");

                request.Credentials = new NetworkCredential(AppInfos.ValidUsernames.First(), AppInfos.Password);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.ContentLength = file.ContentLength;
                request.UseBinary = true;

                using (var inputStream = file.InputStream)
                {
                    using (var requestStream = request.GetRequestStream())
                    {
                        var buffer = new byte[4096];
                        var readBytes = 0;

                        while (true)
                        {
                            readBytes = inputStream.Read(buffer, 0, buffer.Length);

                            if (readBytes == 0)
                            {
                                break;
                            }

                            requestStream.Write(buffer, 0, readBytes);

                            HttpContext.Response.Write(readBytes.ToString() + "D");
                            HttpContext.Response.Flush();
                        }

                        inputStream.Close();
                        requestStream.Close();
                    }
                }

                return Content("E");
            }
            catch (Exception e)
            {
                return Content("Error: " + e.Message);
            }
        }
    }
}
