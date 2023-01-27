using Microsoft.VisualBasic;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels
{
    public class ClientNoteViewModel : BindableBase
    {
        protected readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        #region Properties

        private Client _currentClient;
        public Client CurrentClient
        {
            get { return _currentClient; }
            set { SetProperty(ref _currentClient, value); }
        }

        private int _openedNoteID;
        public int OpenedNoteID
        {
            get { return _openedNoteID; }
            set { SetProperty(ref _openedNoteID, value); }
        }
        private string _openedNoteTitle;
        public string OpenedNoteTitle
        {
            get { return _openedNoteTitle; }
            set { SetProperty(ref _openedNoteTitle, value); }
        }
        private string _openedNoteText;
        public string OpenedNoteText
        {
            get { return _openedNoteText; }
            set { SetProperty(ref _openedNoteText, value); }
        }

        private bool _isNoteOpened;
        public bool IsNoteOpened
        {
            get { return _isNoteOpened; }
            set { SetProperty(ref _isNoteOpened, value); }
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

        #endregion

        #region Commands

        public DelegateCommand NewNoteCommand { get; private set; }
        private void OpenNewNote()
        {
            OpenedNoteID = CurrentClient.Notes.Count();
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
            {
                ClientId = SelectedNote.ClientId
            ,
                NoteID = SelectedNote.NoteID
            ,
                Title = SelectedNote.Title
            ,
                Text = SelectedNote.Text
            ,
                Type = SelectedNote.Type
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
            if (CurrentClient.Notes.Exists(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId))
            {
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Title = OpenedNote.Title;
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Text = OpenedNote.Text;
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenedNote.NoteID && x.ClientId == OpenedNote.ClientId).Type = OpenedNote.Type;
                _eventAggregator.GetEvent<ClientUpdatedEvent>().Publish(CurrentClient);
            }
        }

        #endregion

        public ClientNoteViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            SaveNoteCommand = new DelegateCommand(SaveNote, CanSaveNote);
            OpenNoteCommand = new DelegateCommand(OpenNote, CanOpenNote);
            NewNoteCommand = new DelegateCommand(OpenNewNote);

            IsNoteOpened = false;
        }

        #region Navigation Controls
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Client"))
            {
                CurrentClient = navigationContext.Parameters.GetValue<Client>("Client");
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) { return true; }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
