using ImageUploader.Events;

using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using System;

namespace ImageUploader.ViewModels
{
    public sealed class ShellViewModel : BindableBase
    {
        public ShellViewModel(IRegionManager manager, IEventAggregator eventAggregator)
        {
            manager.RegisterViewWithRegion("Uploader1", typeof(Views.ImageUploaderView));
            manager.RegisterViewWithRegion("Uploader2", typeof(Views.ImageUploaderView));
            manager.RegisterViewWithRegion("Uploader3", typeof(Views.ImageUploaderView));

            eventAggregator.GetEvent<PbValueEvent>().Subscribe(Update);
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

        private double _pbMaxValue;
        /// <summary>
        /// Максимальное значение Progressbar
        /// </summary>
        public double PbMaxValue
        {
            get { return _pbMaxValue; }
            private set
            {
                if (_pbMaxValue == value)
                    return;
                SetProperty(ref _pbMaxValue, value);
            }
        }
        #endregion

        /// <summary>
        /// Обновляет положение ползунка Progressbar 
        /// </summary>
        /// <param name="pbvalue">Текущее значение Progressbar </param>
        private void Update(Tuple<int, double, double> pbvalue)
        {
            double value = pbvalue.Item1;
            double maxValue = pbvalue.Item2;

            if (!PbVisibility)
                PbVisibility = true;

            PbMaxValue = maxValue;
            PbValue = value;

            if (PbValue >= PbMaxValue)
            {
                PbVisibility = false;
                return;
            }
        }
    }
}
