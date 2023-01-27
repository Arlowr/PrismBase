using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Core;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels
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
        private Note _selectedNote;
        public Note SelectedNote
        {
            get { return _selectedNote != null ? _selectedNote : new Note(); }
            set 
            { 
                SetProperty(ref _selectedNote, value);
                if (SelectedNote != null)
                    OpenNoteCommand.RaiseCanExecuteChanged();
            }
        }
        private Note _openedNote;
        public Note OpenedNote
        {
            get { return _openedNote != null ? _openedNote : new Note(); }
            set 
            { 
                SetProperty(ref _openedNote, value);
                if (OpenedNote != null)
                    SaveNoteCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isNoteOpened;
        public bool IsNoteOpened
        {
            get { return _isNoteOpened; }
            set { SetProperty(ref _isNoteOpened, value); }
        }

        private List<string> _noteTypes;
        public List<string> NoteTypes
        {
            get { return _noteTypes; }
            set { SetProperty(ref _noteTypes, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand NewNoteCommand { get; private set; }
        private void OpenNewNote()
        {
            OpenedNote = new Note() { ClientId = Client.ClientId, NoteID = (Client.Notes.Count() + 1) };
            IsNoteOpened = true;
        }
        public DelegateCommand OpenNoteCommand { get; private set; }
        private bool CanOpenNote()
        {
            if (SelectedNote.Title != null)
                return true;
            else
                return false;
        }
        private void OpenNote()
        {
            OpenedNote = new Note() 
            { ClientId = SelectedNote.ClientId
            , NoteID = SelectedNote.NoteID
            , Title = SelectedNote.Title
            , Text= SelectedNote.Text
            , Type= SelectedNote.Type
            };
            IsNoteOpened = true;
        }

        public DelegateCommand SaveNoteCommand { get; private set; }
        private bool CanSaveNote()
        {
            if (OpenedNote.Title != null)
                return true;
            else
                return false;
        }
        private void SaveNote()
        { 
            if(Client.Notes.Exists(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId))
            {
                Client.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Title = OpenedNote.Title;
                Client.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Text = OpenedNote.Text;
                Client.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Type = OpenedNote.Type;
                _eventAggregator.GetEvent<ClientUpdatedEvent>().Publish(Client);
            }
        }

        #endregion

        public ClientMainViewModel(IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            SaveNoteCommand = new DelegateCommand(SaveNote, CanSaveNote);
            OpenNoteCommand = new DelegateCommand(OpenNote, CanOpenNote);
            NewNoteCommand = new DelegateCommand(OpenNewNote);

            NoteTypes = new List<string>() { "Misc", "Important", "Update" };

            IsNoteOpened = false;
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Client"))
            {
                Client = navigationContext.Parameters.GetValue<Client>("Client");
                TabTitle = Client.ClientId + "." + Client.FirstName.Substring(0,1) + "." + Client.LastName.Substring(0, 1);
            }
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
    }
}
