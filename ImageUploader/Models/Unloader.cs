using System;
using System.Net;
using System.Threading.Tasks;

namespace ImageUploader.Models
{
    /// <summary>
    /// Реализация загрузчика
    /// </summary>
    public sealed class Uploader
    {
        private WebClient _client;
        /// <summary>
        /// Начинает загрузку картинки по указанному url
        /// </summary>
        /// <param name="url">Путь до картинки</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadImage(string url)
        {
            try
            {
                _client = new WebClient();
                return await _client.DownloadDataTaskAsync(url);
            }
            catch (WebException e)
            {
                if (e.Status.Equals(WebExceptionStatus.RequestCanceled))
                    return null;
                throw;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Отменяет загрузку картинки
        /// </summary>
        public void AbortDownloading() =>
            _client?.CancelAsync();

        ~Uploader()
        {
            _client?.Dispose();
        }
    }
}
