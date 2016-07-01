using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public interface IFtpClient
    {
        Credentials Credentials
        {
            get;
            set;
        }

        Task UploadFileAsync(string file, int contentLength, Stream inputStream, Action<int> progress);
        Task<IEnumerable<DownloadData>> GetDownloadDataAsync();
        Task<bool> DeleteFileAsync(string file);
    }
}
