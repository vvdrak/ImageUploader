using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ImageUploader.ViewModels
{
    public sealed class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public ShellViewModel(IRegionManager manager)
        {
            _regionManager = manager;
            _regionManager.RegisterViewWithRegion("Uploader1", typeof(Views.ImageUploaderView));
            _regionManager.RegisterViewWithRegion("Uploader2", typeof(Views.ImageUploaderView));
            _regionManager.RegisterViewWithRegion("Uploader3", typeof(Views.ImageUploaderView));
        }

        private DelegateCommand _startAll;
        /// <summary>
        /// Запуск загрузки всех картинок
        /// </summary>
        public DelegateCommand StartAllCommand =>
            _startAll ??
                (_startAll = new DelegateCommand(
                    () =>
                    {
                        
                    }));
    }
}
