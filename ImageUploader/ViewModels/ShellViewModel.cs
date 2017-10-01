using ImageUploader.Models;

using Prism.Mvvm;
using Prism.Regions;

namespace ImageUploader.ViewModels
{
    public sealed class ShellViewModel : BindableBase
    {
        public ShellViewModel(IRegionManager manager)
        {
            manager.RegisterViewWithRegion("Uploader1", typeof(Views.ImageUploaderView));
            manager.RegisterViewWithRegion("Uploader2", typeof(Views.ImageUploaderView));
            manager.RegisterViewWithRegion("Uploader3", typeof(Views.ImageUploaderView));

            Summator.GetInstance().ProgressChanged += (current) => 
            {
                if (current == 100)
                {
                    PbVisibility = false;
                    return;
                }

                if (!PbVisibility)
                    PbVisibility = true;
                PbValue = current;
            };
        }

        #region Binding area
        private bool _pbVisibitilty;
        /// <summary>
        /// Управляет видимостью Progressbar
        /// </summary>
        public bool PbVisibility
        {
            get { return _pbVisibitilty; }
            private set { SetProperty(ref _pbVisibitilty, value); }
        }

        private double _pbValue;
        /// <summary>
        /// Текущее значение Progressbar
        /// </summary>
        public double PbValue
        {
            get { return _pbValue; }
            private set { SetProperty(ref _pbValue, value); }
        }
        #endregion
    }
}
