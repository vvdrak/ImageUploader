using ImageUploader.Helpers;
using ImageUploader.Models;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.ComponentModel;
using System.Media;

namespace ImageUploader.ViewModels
{
    public sealed class ImageUploaderViewModel : BindableBase, IDataErrorInfo
    {
        private Uploader _uploader;

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

        private byte[] _imageData;
        /// <summary>
        /// Массив байт загруженной картинки
        /// </summary>
        public byte[] ImageData
        {
            get { return _imageData; }
            set { SetProperty(ref _imageData, value); }
        }

        private string _url;
        /// <summary>
        /// Путь до картинки
        /// </summary>
        public string URL
        {
            get { return _url; }
            set
            {
                SetProperty(ref _url, value);
                // добавляем команду к глобальной если соблюдены все условия ее выполнения. В противном случае удаляем из глобальной
                if (Error == null && !GlobalCommands.StartAllCommand.RegisteredCommands.Contains(StartCommand))
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
            _start ?? (_start = new DelegateCommand(ExecuteStart, () => !IsBusy && Error == null)
                .ObservesProperty(() => URL)
                .ObservesProperty(() => IsBusy));

        private DelegateCommand _stop;
        /// <summary>
        /// Остановка загрузки картинки
        /// </summary>
        public DelegateCommand StopCommand =>
            _stop ?? (_stop = new DelegateCommand(ExecuteStop, () => IsBusy)
                .ObservesProperty(() => IsBusy));

        public ImageUploaderViewModel()
        {
            _uploader = new Uploader();   
        }

        #region Private area
        private async void ExecuteStart()
        {
            try
            {
                IsBusy = true;
                ImageData = await _uploader.DownloadImage(URL);
            }
            catch (Exception e) { SystemSounds.Beep.Play(); }
            finally { IsBusy = false; }
        }

        private void ExecuteStop()
        {
            _uploader?.AbortDownloading();
            IsBusy = false;
        }
        #endregion

        #region IDataErrorInfo implementation
        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                switch (columnName)
                {
                    case "URL":
                        if (string.IsNullOrWhiteSpace(URL))
                            errorMessage = @"Поле ""URL"" не может быть пустым";
                        break;

                }
                return errorMessage;
            }
        }

        public string Error => this["URL"];
        #endregion
    }
}
