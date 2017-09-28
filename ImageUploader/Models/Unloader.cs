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

        internal delegate void DownloadProgress(double position, double size);

        private DownloadProgress _download;
        /// <summary>
        /// Отображает процесс загрузки
        /// </summary>
        internal event DownloadProgress DownloadProgressChanged
        {
            add { AddObserver(value); }
            remove { RemoveObserver(value); }
        }

        /// <summary>
        /// Начинает загрузку картинки по указанному url
        /// </summary>
        /// <param name="url">Путь до картинки</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadImage(string url)
        {
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
        public void AbortDownloading() =>
            _client?.CancelAsync();

        #region private area
        private void NotifyObservers(object sender, DownloadProgressChangedEventArgs e)
        {
            var handler = _download;
            if (handler != null)
                handler.Invoke(e.BytesReceived, e.TotalBytesToReceive);
        }

        private void AddObserver(DownloadProgress value)
        {
            lock (this)
            {
                _download += value;
                _client.DownloadProgressChanged += NotifyObservers;
            }
        }

        private void RemoveObserver(DownloadProgress value)
        {
            lock (this)
            {
                _download -= value;
                _client.DownloadProgressChanged -= NotifyObservers;
            }
        } 
        #endregion

        public Uploader()
        {
            _client = new WebClient();
        }

        ~Uploader()
        {
            _client?.Dispose();
        }
    }
}
