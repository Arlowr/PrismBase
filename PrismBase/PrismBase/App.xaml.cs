using Prism.Ioc;
using Prism.Modularity;
using PrismBase.Modules.Main;
using PrismBase.Modules.Details;
using PrismBase.Services;
using PrismBase.Services.Interfaces;
using PrismBase.Views;
using System.Windows;

namespace PrismBase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<DetailsModule>();
        }
    }
}
