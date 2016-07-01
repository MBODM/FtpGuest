using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class FtpClient : IFtpClient
    {
        private const string TrashFolderName = "trash";
        private const string ArgumentIsNullOrEmptyMessage = "Argument is null or empty.";

        private readonly string ftpServer;

        public FtpClient(string ftpServer)
        {
            if (string.IsNullOrEmpty(ftpServer))
            {
                throw new ArgumentException(ArgumentIsNullOrEmptyMessage, nameof(ftpServer));
            }

            this.ftpServer = ftpServer;
        }

        public Credentials Credentials
        {
            get;
            set;
        }

        public async Task UploadFileAsync(string file, int contentLength, Stream inputStream, Action<int> progress)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException(ArgumentIsNullOrEmptyMessage, nameof(file));
            }

            if (file.ToLower() == TrashFolderName.ToLower())
            {
                throw new ArgumentException($"Upload of a file named '{TrashFolderName}' is not allowed.");
            }

            if (contentLength <= 0)
            {
                throw new ArgumentException($"The argument '{nameof(contentLength)}' must be greater than 0.");
            }

            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            var request = CreateRequest(file, WebRequestMethods.Ftp.UploadFile);

            request.ContentLength = contentLength;

            using (var requestStream = await request.GetRequestStreamAsync())
            {
                var buffer = new byte[4096];

                var readBytes = 0;

                while (true)
                {
                    readBytes = await inputStream.ReadAsync(buffer, 0, buffer.Length);

                    if (readBytes == 0)
                    {
                        break;
                    }

                    await requestStream.WriteAsync(buffer, 0, readBytes);

                    progress?.Invoke(readBytes);
                }

                inputStream.Close();

                requestStream.Close();
            }
        }

        public async Task<IEnumerable<DownloadData>> GetDownloadDataAsync()
        {
            var request = CreateRequest(null, WebRequestMethods.Ftp.ListDirectoryDetails);

            using (var response = (FtpWebResponse)await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var result = new List<DownloadData>();

                        while (true)
                        {
                            var line = await streamReader.ReadLineAsync();

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

        public async Task<bool> DeleteFileAsync(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException(ArgumentIsNullOrEmptyMessage, nameof(file));
            }

            if (file == TrashFolderName)
            {
                throw new ArgumentException($"Deletion of a file named '{TrashFolderName}' is not allowed.");
            }

            var request = CreateRequest(file, WebRequestMethods.Ftp.Rename);

            request.RenameTo = TrashFolderName + "/" + file;

            using (var response = (FtpWebResponse)await request.GetResponseAsync())
            {
                response.Close();

                return response.StatusCode == FtpStatusCode.FileActionOK;
            }
        }

        private FtpWebRequest CreateRequest(string file, string method)
        {
            var url = $"ftp://{ftpServer}{(string.IsNullOrEmpty(file) ? string.Empty : $"/{file}")}";

            var request = (FtpWebRequest)WebRequest.Create(url);

            request.Credentials = new NetworkCredential(Credentials.Username, Credentials.Password);
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.KeepAlive = false;
            request.UseBinary = true;
            request.Method = method;
            request.Timeout = 5000;
            request.Proxy = null;

            return request;
        }

        private DownloadData ParseLine(string line)
        {
            var textAfterMinutes = line.Substring(line.IndexOf(':'));
            var fileName = textAfterMinutes.Substring(4);

            if (fileName.ToLower() == TrashFolderName.ToLower())
            {
                return null;
            }

            var lineParts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var fileSize = lineParts.ElementAt(4);

            var usernameEncoded = Uri.EscapeDataString(Credentials.Username);
            var passwordEncoded = Uri.EscapeDataString(Credentials.Password);
            var fileNameEncoded = Uri.EscapeDataString(fileName);

            var downloadLink = $"ftp://{usernameEncoded}:{passwordEncoded}@{ftpServer}/{fileNameEncoded}";

            return new DownloadData(fileName, long.Parse(fileSize), downloadLink);
        }
    }
}
