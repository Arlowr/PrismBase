using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Core;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrismBase.Modules.Details.ViewModels.ClientWindows
{
    public class ClientMainViewModel : BindableBase, INavigationAware
    {
        protected readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        #region Properties

        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        private string _clientSubViewName;
        public string ClientSubViewName
        {
            get { return _clientSubViewName; }
            set { SetProperty(ref _clientSubViewName, value); }
        }

        #endregion

        public ClientMainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        #region Navigation Controls
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Client"))
            {
                Client = navigationContext.Parameters.GetValue<Client>("Client");
                TabTitle = Client.ClientId + "." + Client.FirstName.Substring(0, 1) + "." + Client.LastName.Substring(0, 1);
            }

            ClientSubViewName = Client.ClientId + "SubWindow";

            var navParams = new NavigationParameters();
            navParams.Add("Client", Client);
            _regionManager.RequestNavigate(ClientSubViewName, "ClientNotesView", navParams);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var newClientPage = navigationContext.Parameters.GetValue<Client>("Client");

            if (newClientPage != null)
            {
                if (Client.FullName != newClientPage.FullName)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}