using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismBase.Modules.Details.Views;
using PrismBase.Modules.Details.Views.ClientWindows;
using PrismBase.Modules.Details.Views.WorkerWindows;

namespace PrismBase.Modules.Details
{
    public class DetailsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Important for enabling views to be displayed
            containerRegistry.RegisterForNavigation<DetailsMainView>();
            containerRegistry.RegisterForNavigation<ClientMainView>();
            containerRegistry.RegisterForNavigation<ClientNotesView>();
            containerRegistry.RegisterForNavigation<WorkerMainView>();
            containerRegistry.RegisterForNavigation<WorkerTasksView>();

        }
    }
}