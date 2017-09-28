using Autofac;
using ImageUploader.Views;
using Prism.Autofac;
using System.Windows;

namespace ImageUploader
{
    public class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell() =>
            Container.Resolve<Shell>();
        
        protected override void InitializeShell() =>
            Application.Current.MainWindow.Show();

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);
            builder.RegisterTypeForNavigation<ImageUploaderView>("ImageUploaderView");
        }
    }
}