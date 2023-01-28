using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels.ClientWindows
{
    public class ClientNotesViewModel : BindableBase, INavigationAware
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
        private List<Note> _currentNotes;
        public List<Note> CurrentNotes
        {
            get { return _currentNotes; }
            set { SetProperty(ref _currentNotes, value); }
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
        private int _openNoteID;
        public int OpenNoteID
        {
            get { return _openNoteID; }
            set { SetProperty(ref _openNoteID, value); }
        }
        private string _openNoteTitle;
        public string OpenNoteTitle
        {
            get { return _openNoteTitle; }
            set 
            { 
                SetProperty(ref _openNoteTitle, value);
                if (!String.IsNullOrEmpty(OpenNoteTitle))
                    SaveNoteCommand.RaiseCanExecuteChanged();
            }
        }
        private string _openNoteText;
        public string OpenNoteText
        {
            get { return _openNoteText; }
            set { SetProperty(ref _openNoteText, value); }
        }
        private string _openNoteType;
        public string OpenNoteType
        {
            get { return _openNoteType; }
            set { SetProperty(ref _openNoteType, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand NewNoteCommand { get; private set; }
        private void OpenNewNote()
        {
            if (CurrentClient.Notes == null)
                CurrentClient.Notes= new List<Note>();

            OpenNoteID = CurrentClient.Notes.Count() + 1;
            OpenNoteTitle = "";
            OpenNoteText = "";
            OpenNoteType = "Misc";
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
            OpenNoteID = SelectedNote.NoteID;
            OpenNoteTitle = SelectedNote.Title;
            OpenNoteText = SelectedNote.Text;
            OpenNoteType = SelectedNote.Type;
            IsNoteOpened = true;
        }

        public DelegateCommand SaveNoteCommand { get; private set; }
        private bool CanSaveNote()
        {
            if (OpenNoteTitle != null)
                return true;
            else
                return false;
        }
        private void SaveNote()
        {
            if (CurrentClient.Notes.Exists(x => x.NoteID == OpenNoteID && x.ClientId == CurrentClient.ClientId))
            {
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenNoteID && x.ClientId == CurrentClient.ClientId).Title = OpenNoteTitle;
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenNoteID && x.ClientId == CurrentClient.ClientId).Text = OpenNoteText;
                CurrentClient.Notes.FirstOrDefault(x => x.NoteID == OpenNoteID && x.ClientId == CurrentClient.ClientId).Type = OpenNoteType;
                _eventAggregator.GetEvent<ClientUpdatedEvent>().Publish(CurrentClient);
            } 
            else
            {
                CurrentClient.Notes.Add(new Note()
                {
                    NoteID = OpenNoteID,
                    ClientId = CurrentClient.ClientId,
                    Title = OpenNoteTitle,
                    Text = OpenNoteText,
                    Type = OpenNoteType
                });
            }
            RefreshNotesList();
        }

        #endregion

        public ClientNotesViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            SaveNoteCommand = new DelegateCommand(SaveNote, CanSaveNote);
            OpenNoteCommand = new DelegateCommand(OpenNote, CanOpenNote);
            NewNoteCommand = new DelegateCommand(OpenNewNote);

            IsNoteOpened = false;

            NoteTypes = new List<string>() { "Misc", "Important", "Update" };
        }

        private void RefreshNotesList()
        {
            if(CurrentClient.Notes == null) 
                CurrentClient.Notes = new List<Note>();

            var tempSelectedNoteID = SelectedNote.NoteID;
            var tempNoteList = new List<Note>();
            CurrentNotes = new List<Note>();

            foreach (var note in CurrentClient.Notes)
            {
                tempNoteList.Add(note);
            }

            CurrentNotes = tempNoteList;
            SelectedNote = CurrentNotes.FirstOrDefault(x => x.NoteID == tempSelectedNoteID);
        }

        #region Navigation Controls
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Client"))
            {
                CurrentClient = navigationContext.Parameters.GetValue<Client>("Client");
            }
            RefreshNotesList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) { return true; }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}