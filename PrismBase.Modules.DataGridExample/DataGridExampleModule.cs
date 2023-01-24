using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismBase.Modules.DataGridExample.Views;

namespace PrismBase.Modules.DataGridExample
{
    public class DataGridExampleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DataGridView>();
        }
    }
}