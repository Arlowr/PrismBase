using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PrismBase.Modules.Main.ViewModels
{
    public class WelcomePageViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> LetsGoCommand { get; private set; }
        public WelcomePageViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            LetsGoCommand = new DelegateCommand<string>(LetsGoCommandHandler);
            _regionManager = regionManager;
        }

        private void LetsGoCommandHandler(string selectedRPG)
        {
            string PageToGoTo = "";
            switch (selectedRPG)
            {
                case "Main":
                    PageToGoTo = "DetailsMainView";
                    break;
                case "Settings":
                    PageToGoTo = "DataSettingsView";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(PageToGoTo))
                _regionManager.RequestNavigate(RegionNames.ContentRegion, PageToGoTo);
        }
    }
}
