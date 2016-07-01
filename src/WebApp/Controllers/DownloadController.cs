using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApp
{
    [Authorize]
    [BasicAuthFilter]
    public class DownloadController : Controller
    {
        private readonly IBasicAuth basicAuth;

        public DownloadController(IBasicAuth basicAuth)
        {
            if (basicAuth == null)
            {
                throw new ArgumentNullException(nameof(basicAuth));
            }

            this.basicAuth = basicAuth;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    using (var response = await httpClient.GetAsync("api/values"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var downloadData = await response.Content.ReadAsAsync<IEnumerable<DownloadData>>();

                            return View(new DownloadViewModel() { DownloadData = downloadData });
                        }
                        else
                        {
                            return View(new DownloadViewModel() { Error = CreateErrorText(true, response) });
                        }
                    }
                }
            }
            catch
            {
                return View(new DownloadViewModel() { Error = CreateErrorText(false, null) });
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteFile(string file)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("api/values?file=" + file))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View(new DownloadViewModel() { Error = CreateErrorText(true, response) });
                        }
                    }
                }
            }
            catch
            {
                return View(new DownloadViewModel() { Error = CreateErrorText(false, null) });
            }
        }

        private HttpClient CreateHttpClient()
        {
            var credentials = new Credentials(User.Identity.Name, StaticData.CachedPassword);
            var parameter = basicAuth.ConvertCredentialsToParameter(credentials);

            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("http://mbodm.com/FtpGuestApi/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(basicAuth.SchemeName, parameter);

            return httpClient;
        }

        private string CreateErrorText(bool isWebApiError, HttpResponseMessage response)
        {
            if (isWebApiError)
            {
                return $"Fehler: Die Web Api hat mit HTTP-Status-Code {response.StatusCode} geantwortet.";
            }
            else
            {
                return "Fehler: Im Controller ist eine Exception aufgetreten.";
            }
        }
    }
}
