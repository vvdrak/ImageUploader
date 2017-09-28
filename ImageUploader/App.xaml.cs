using System.Windows;

namespace ImageUploader
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstapper = new Bootstrapper();
            bootstapper.Run();
        }
    }
}
