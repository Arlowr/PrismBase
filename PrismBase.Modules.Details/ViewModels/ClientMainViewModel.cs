using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels
{
    public class ClientMainViewModel : BindableBase
    {
        protected readonly IEventAggregator _eventAggregator;
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public ClientMainViewModel(IEventAggregator eventAggregator)
        {
            //_eventAggregator = eventAggregator;
            //_eventAggregator.GetEvent<ClientUpdatedEvent>().Subscribe((Client) =>
            //{
            //    if (this.Client.ClientId == Client.ClientId)
            //        if (!Client.FirstName.ToUpper().Contains("Something"))
            //            this.Client = Client;
            //        else if (NPC.Name.ToUpper().Contains("NEW") && (TabTitle == Client.Name))
            //            this.Client = Client;

            //    UpdateText = "Uncommited changes";
            //    UpdatedNPC = false;
            //});
        }
    }
}
