using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismBase.Modules.Details.Views;

namespace PrismBase.Modules.Details
{
    public class DetailsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DetailsMainView>();
            containerRegistry.RegisterForNavigation<ClientMainView>();

        }
    }
}