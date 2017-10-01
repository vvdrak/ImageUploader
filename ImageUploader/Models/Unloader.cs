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

        public long BytesReceived { get; private set; }
        public long TotalBytesToReceive { get; private set; }

        /// <summary>
        /// Начинает загрузку картинки по указанному url
        /// </summary>
        /// <param name="url">Путь до картинки</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadImage(string url)
        {
            BytesReceived = 0;
            TotalBytesToReceive = 0;
            Summator.GetInstance().CountFrom(this);
            try { return await _client.DownloadDataTaskAsync(url); }
            catch (WebException e)
            {
                if (e.Status.Equals(WebExceptionStatus.RequestCanceled))
                    return null;
                throw;
            }
            catch { throw; }
        }

        /// <summary>
        /// Отменяет загрузку картинки
        /// </summary>
        public void AbortDownloading()
        {
            _client?.CancelAsync();
            BytesReceived = TotalBytesToReceive;
            Summator.GetInstance().Forget(this);
        }
    

        public Uploader()
        {
            _client = new WebClient();
            _client.DownloadProgressChanged += (sender, e) =>
            {
                BytesReceived = e.BytesReceived;
                TotalBytesToReceive = e.TotalBytesToReceive;
            };                    
        }

        ~Uploader()
        {
            _client?.Dispose();
        }
    }
}
