using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismBase.Core;
using PrismBase.Modules.Main.Views;

namespace PrismBase.Modules.Main
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "WelcomePageView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WelcomePageView>();
        }
    }
}