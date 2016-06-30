using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApi
{
    [Authorize]
    [BasicAuthFilter]
    public class ValuesController : ApiController
    {
        private readonly IFtpClient ftpClient;

        public ValuesController(IFtpClient ftpClient)
        {
            if (ftpClient == null)
            {
                throw new ArgumentNullException(nameof(ftpClient));
            }

            this.ftpClient = ftpClient;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]string value)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ReadAll()
        {
            try
            {
                SetCredentials();

                var result = await ftpClient.GetDownloadDataAsync();

                return Ok(result);
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Read(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            try
            {
                SetCredentials();

                var result = await ftpClient.GetDownloadDataAsync();

                if (id < result.Count())
                {
                    return Ok(result.ElementAt(id));
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Read(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return BadRequest();
            }

            try
            {
                SetCredentials();

                var all = await ftpClient.GetDownloadDataAsync();

                var result = all.Where(x => x.FileName == file.Trim('"')).FirstOrDefault();

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public IHttpActionResult Update()
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return BadRequest();
            }

            try
            {
                SetCredentials();

                if (await ftpClient.DeleteFileAsync(file))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return InternalServerError();
            }
        }

        private void SetCredentials()
        {
            ftpClient.Credentials = new Credentials(User.Identity.Name, StaticData.CachedPassword);
        }
    }
}
