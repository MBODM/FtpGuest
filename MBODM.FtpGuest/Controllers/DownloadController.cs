using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest
{
    public sealed class DownloadController : Controller
    {
        [HttpGet]
        [BasicAuth]
        public ActionResult Index()
        {
            var model = new DownloadViewModel();

            model.DownloadData = GetDownloadData();

            return View(model);
        }

        [HttpGet]
        [BasicAuth]
        public ActionResult DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return Content($"Error: Parameter '{nameof(file)}' was null or empty.");
            }

            var request = (FtpWebRequest)WebRequest.Create(AppInfos.FtpServer + "/" + file);

            request.Credentials = new NetworkCredential(AppInfos.ValidUsernames.First(), AppInfos.Password);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = AppInfos.TrashFolder + "/" + file;
            request.UseBinary = true;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                var statusCode = response.StatusCode;

                response.Close();

                if (statusCode == FtpStatusCode.FileActionOK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content($"Error: FTP StatusCode was {statusCode}.");
                }
            }
        }

        private IEnumerable<DownloadData> GetDownloadData()
        {
            var result = new List<DownloadData>();

            var request = (FtpWebRequest)WebRequest.Create(AppInfos.FtpServer);

            request.Credentials = new NetworkCredential(AppInfos.ValidUsernames.First(), AppInfos.Password);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.UseBinary = true;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        while (true)
                        {
                            var line = streamReader.ReadLine();

                            if (line != null)
                            {
                                var downloadData = ParseLine(line);

                                if (downloadData != null)
                                {
                                    result.Add(downloadData);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamReader.Close();
                        response.Close();

                        return result;
                    }
                }
            }
        }

        private DownloadData ParseLine(string line)
        {
            var textAfterMinutes = line.Substring(line.IndexOf(':'));
            var fileName = textAfterMinutes.Substring(4);

            if (fileName.ToLower() == AppInfos.TrashFolder.ToLower())
            {
                return null;
            }

            var lineParts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var fileSize = lineParts.ElementAt(4);

            // Some browsers have problems with @ in username, but this works.
            var username = AppInfos.ValidUsernames.First().Replace("@", "%40");

            var loginPart = $"{username}:{AppInfos.Password}@";
            var downloadLink = $"{AppInfos.FtpServer.Insert(6, loginPart)}/{fileName}";

            return new DownloadData(fileName, long.Parse(fileSize), downloadLink);
        }
    }
}
