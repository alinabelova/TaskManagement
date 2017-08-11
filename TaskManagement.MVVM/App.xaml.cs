using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using TaskManagement.MVVM.Resources.Localization;
using Telerik.Windows.Controls;

namespace TaskManagement.MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("В программе возникла ошибка: " + e.Exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU"); ;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU"); ;

            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            LocalizationManager.Manager = new LocalizationManager
            {
                ResourceManager = RadResources.ResourceManager
            };
            base.OnStartup(e);
        }
    }
}
