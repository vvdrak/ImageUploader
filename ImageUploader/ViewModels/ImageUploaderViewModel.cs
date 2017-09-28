using ImageUploader.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Media;

namespace ImageUploader.ViewModels
{
    public sealed class ImageUploaderViewModel : BindableBase
    {
        private Uploader _uploader;

        private bool _isBusy;
        /// <summary>
        /// Флаг, показыващий занят ли в данный момент загрузчик
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private byte[] _data;
        /// <summary>
        /// Массив байт загруженной картинки
        /// </summary>
        public byte[] ImageData
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }

        private string _url;
        /// <summary>
        /// Путь до картинки
        /// </summary>
        public string URL
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private DelegateCommand _start;
        /// <summary>
        /// Запуск загрузки картинки по указанному пути <see cref="URL"/>
        /// </summary>
        public DelegateCommand StartCommand =>
            _start ??
                (_start = new DelegateCommand(
                    async () =>
                    {
                        try
                        {
                            IsBusy = true;
                            ImageData = await _uploader.DownloadImage(URL);
                        }
                        catch (Exception) { SystemSounds.Beep.Play(); }
                        finally { IsBusy = false; }
                    },
                    () => !string.IsNullOrWhiteSpace(URL)).ObservesProperty(() => URL));

        private DelegateCommand _stop;
        /// <summary>
        /// Остановка загрузки картинки
        /// </summary>
        public DelegateCommand StopCommand =>
            _stop ??
                (_stop = new DelegateCommand(
                    () =>
                    {
                        _uploader?.AbortDownloading();
                        IsBusy = false;
                    },
                    () => IsBusy).ObservesProperty(() => IsBusy));

        public ImageUploaderViewModel()
        {
            _uploader = new Uploader();
        }
    }
}
