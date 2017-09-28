using ImageUploader.Events;
using ImageUploader.Helpers;
using ImageUploader.Models;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

using System;
using System.Media;

namespace ImageUploader.ViewModels
{
    public sealed class ImageUploaderViewModel : BindableBase
    {
        private Uploader _uploader;
        private IEventAggregator _eventAggregator;
        private BitmapWrapper _bitmapWrapper = new BitmapWrapper();

        #region Binding area
        private bool _isBusy;
        /// <summary>
        /// Флаг, показыващий занят ли в данный момент загрузчик
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value)
                    ImageData = null;
                SetProperty(ref _isBusy, value);
            }
        }

        /// <summary>
        /// Массив байт загруженной картинки
        /// </summary>
        public byte[] ImageData
        {
            get { return _bitmapWrapper.ImageData; }
            set { SetProperty(ref _bitmapWrapper.ImageData, value); }
        }

        /// <summary>
        /// Путь до картинки
        /// </summary>
        public string URL
        {
            get { return _bitmapWrapper.URL; }
            set
            {
                SetProperty(ref _bitmapWrapper.URL, value);
                // добавляем команду к глобальной если соблюдены все условия ее выполнения. В противном случае удаляем из глобальной
                if (_bitmapWrapper.Error == null && !GlobalCommands.StartAllCommand.RegisteredCommands.Contains(StartCommand))
                    GlobalCommands.StartAllCommand.RegisterCommand(StartCommand);
                else
                    GlobalCommands.StartAllCommand.RegisteredCommands.Remove(StartCommand);
            }
        }
        #endregion

        private DelegateCommand _start;
        /// <summary>
        /// Запуск загрузки картинки по указанному пути <see cref="URL"/>
        /// </summary>
        public DelegateCommand StartCommand =>
            _start ?? (_start = new DelegateCommand(ExecuteStart, () => !IsBusy && _bitmapWrapper.Error == null)
                .ObservesProperty(() => URL)
                .ObservesProperty(() => IsBusy));

        private DelegateCommand _stop;
        /// <summary>
        /// Остановка загрузки картинки
        /// </summary>
        public DelegateCommand StopCommand =>
            _stop ?? (_stop = new DelegateCommand(ExecuteStop, () => IsBusy)
                .ObservesProperty(() => IsBusy));

        public ImageUploaderViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _uploader = new Uploader();
            _uploader.DownloadProgressChanged += (position, maxvalue) =>
                _eventAggregator.GetEvent<PbValueEvent>().Publish(new Tuple<int, double, double>(GetHashCode(), position, maxvalue));
        }

        #region Private area
        private async void ExecuteStart()
        {
            try
            {
                IsBusy = true;
                ImageData = await _uploader.DownloadImage(URL);
            }
            catch (Exception) { SystemSounds.Beep.Play(); }
            finally { IsBusy = false; }
        }

        private void ExecuteStop()
        {
            _uploader?.AbortDownloading();
            IsBusy = false;
            _eventAggregator.GetEvent<PbValueEvent>().Publish(new Tuple<int, double, double>(GetHashCode(), 0, 0)); // прячем Progressbar
        }
        #endregion
    }
}
