using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Core;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels
{
    public class DetailsMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        protected readonly IEventAggregator _eventAggregator;

        #region Properties

        private List<string> _people;
        public List<string> People
        {
            get { return _people; }
            set { SetProperty(ref _people, value); }
        }
        private string _selectedPerson;
        public string SelectedPerson
        {
            get { return _selectedPerson; }
            set 
            {
                SetProperty(ref _selectedPerson, value);
                if (SelectedPerson != null)
                    SelectPersonCommand.RaiseCanExecuteChanged();
            }
        }
        private List<Client> _allClients;
        public List<Client> AllClients
        {
            get { return _allClients; }
            set { SetProperty(ref _allClients, value); }
        }
        private List<Worker> _allWorkers;
        public List<Worker> AllWorkers
        {
            get { return _allWorkers; }
            set { SetProperty(ref _allWorkers, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand SelectPersonCommand { get; private set; }

        private bool CanSelectPerson()
        {
            if (SelectedPerson != null)
                return true;
            else
                return false;
        }
        private void SelectPerson()
        {
            var PersonType = "";
            var navParams = new NavigationParameters();

            if (AllClients.Exists(x => x.FullName == SelectedPerson))
            {
                PersonType = "Client";
                var tempClient = new Client();
                tempClient = AllClients.FirstOrDefault(x => x.FullName == SelectedPerson);
                navParams.Add("Client", tempClient);
            }
            else if (AllWorkers.Exists(x => x.FullName == SelectedPerson))
            {
                PersonType = "Worker";
                var tempWorker = new Worker();
                tempWorker = AllWorkers.FirstOrDefault(x => x.FullName == SelectedPerson);
                navParams.Add("Worker", tempWorker);
            }

            var WindowType = "";

            if (PersonType == "Client")
                WindowType = "ClientMainView";
            else if (PersonType == "Worker")
                WindowType = "WorkerMainView";
            _regionManager.RequestNavigate(RegionNames.DetailsMainRegion, WindowType, navParams);

        }

        #endregion

        public DetailsMainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ClientUpdatedEvent>().Subscribe((Client) =>
            {
                if (this.AllClients.Exists(x => x.ClientId == Client.ClientId))
                    this.AllClients.FirstOrDefault(x => x.ClientId == Client.ClientId).Notes = Client.Notes;

            });

            CreateExampleClients();
            CreateExampleWorkers();

            RefreshPeople();

            SelectPersonCommand = new DelegateCommand(SelectPerson, CanSelectPerson);
        }

        private void RefreshPeople() 
        {
            var tempSelected = SelectedPerson;
            People = new List<string>();

            foreach (var client in AllClients)
            {
                People.Add(client.FullName);
            }
            foreach (var worker in AllWorkers)
            {
                People.Add(worker.FullName);
            }

            SelectedPerson = tempSelected;
        }

        #region Set Up Example Data

        private void CreateExampleClients()
        {
            List<Client> clients = new List<Client>();

            clients.Add(new Client() { ClientId = 1, FirstName = "Tim", LastName = "Gibbs", Address = "Somewhere Out There", Description = "Person?" });
            clients.Add(new Client() { ClientId = 2, FirstName = "Jerry", LastName = "Smith", Address = "Smith Household", Description = "Father of Morty, Son-in-law of Rick" });
            clients.Add(new Client() { ClientId = 3, FirstName = "Morty", LastName = "Smith", Address = "Smith Household", Description = "Doesn't Know What He Is Doing",
                Notes = new List<Note>() { new Note() { ClientId = 3, NoteID = 1, Title = "Lost In Space", Text="Morty Got Lost in Space, got to find him.", Type = "Misc" },
                new Note() { ClientId = 3, NoteID = 2, Title = "Investigation has begun", Text="Rick has been sent out to find Morty.", Type= "Update" },
                new Note() { ClientId = 3, NoteID = 3, Title = "Located", Text="Morty has been found.", Type = "Update" } 
                }
            });

            AllClients = clients;
        }
        private void CreateExampleWorkers()
        {
            List<Worker> workers = new List<Worker>();

            workers.Add(new Worker() { WorkerID = 1, FirstName = "Rick", LastName = "Sanchez", Address = "Smith Household", Department = "Inventing", JobTitle = "Mad Scientist" });
            workers.Add(new Worker() { WorkerID = 2, FirstName = "Kratos", LastName = "Judge", Address = "Midgard", Department = "Slaying", JobTitle = "God Slayer" });
            workers.Add(new Worker() { WorkerID = 3, FirstName = "Atreus", LastName = "Judge", Address = "Midgard", Department = "Slaying", JobTitle = "God Slayer Assistant" });
            workers.Add(new Worker() { WorkerID = 4, FirstName = "Doom", LastName = "Guy", Address = "Hell", Department = "Slaying", JobTitle = "Demon Slayer" });

            AllWorkers = workers;
        }

        #endregion
    }
}
