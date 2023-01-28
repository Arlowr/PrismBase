using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Core.Mvvm;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static PrismBase.Modules.Details.Models.Client;

namespace PrismBase.Modules.Details.ViewModels.WorkerWindows
{
    public class WorkerTasksViewModel : BindableBase, INavigationAware
    {
        protected readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        #region Properties

        private Worker _currentWorker;
        public Worker CurrentWorker
        {
            get { return _currentWorker; }
            set { SetProperty(ref _currentWorker, value); }
        }

        private List<Task> _shownTasks;
        public List<Task> ShownTasks
        {
            get { return _shownTasks; }
            set { SetProperty(ref _shownTasks, value); }
        }
        private bool _filterForWorker;
        public bool FilterForWorker
        {
            get { return _filterForWorker; }
            set 
            { 
                SetProperty(ref _filterForWorker, value);
                RefreshTasks();
            }
        }
        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask != null ? _selectedTask : new Task(); }
            set 
            { 
                SetProperty(ref _selectedTask, value);
                if (SelectedTask != null)
                    OpenTaskCommand.RaiseCanExecuteChanged();
            }
        }
        private bool _isTaskOpen;
        public bool IsTaskOpen
        {
            get { return _isTaskOpen; }
            set { SetProperty(ref _isTaskOpen, value); }
        }
        private int _openTaskID;
        public int OpenTaskID
        {
            get { return _openTaskID; }
            set { SetProperty(ref _openTaskID, value); }
        }
        private bool _isTaskAssignedToWorker;
        public bool IsTaskAssignedToWorker
        {
            get { return _isTaskAssignedToWorker; }
            set 
            { 
                SetProperty(ref _isTaskAssignedToWorker, value);
                UpdatesDone = true;
            }
        }
        private string _openTaskTitle;
        public string OpenTaskTitle
        {
            get { return _openTaskTitle; }
            set 
            { 
                SetProperty(ref _openTaskTitle, value);
                UpdatesDone = true;
            }
        }
        private string _openTaskDescription;
        public string OpenTaskDescription
        {
            get { return _openTaskDescription; }
            set 
            { 
                SetProperty(ref _openTaskDescription, value);
                UpdatesDone = true;
            }
        }

        private bool _updatesDone;
        public bool UpdatesDone
        {
            get { return _updatesDone; }
            set 
            { 
                SetProperty(ref _updatesDone, value);
                SaveTaskCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand OpenTaskCommand { get; private set; }
        private bool CanOpenTask()
        {
            if (SelectedTask.TaskTitle != null)
                return true;
            else
                return false;
        }
        private void OpenTask()
        {
            OpenTaskID = SelectedTask.TaskID;
            IsTaskAssignedToWorker = SelectedTask.WorkerIDs.Exists(x => x == CurrentWorker.WorkerID);
            OpenTaskTitle = SelectedTask.TaskTitle;
            OpenTaskDescription = SelectedTask.Description;
            IsTaskOpen = true;
            UpdatesDone = false;
        }
        public DelegateCommand SaveTaskCommand { get; private set; }
        private bool CanUpdateTask()
        {
            if (UpdatesDone)
                return true;
            else
                return false;
        }
        private void UpdateTask()
        {
            AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).TaskTitle = OpenTaskTitle;
            AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).Description = OpenTaskDescription;
            if (IsTaskAssignedToWorker)
            {
                if(!AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).WorkerIDs.Contains(CurrentWorker.WorkerID))
                {
                    AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).WorkerIDs.Add(CurrentWorker.WorkerID);
                }
            }
            else
            {
                if (AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).WorkerIDs.Contains(CurrentWorker.WorkerID))
                {
                    AllTasks.ListOfTasks.FirstOrDefault(x => x.TaskID == OpenTaskID).WorkerIDs.Remove(CurrentWorker.WorkerID);
                }
            }
            RefreshTasks();
        }

        #endregion
        public WorkerTasksViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            OpenTaskCommand = new DelegateCommand(OpenTask, CanOpenTask);
            SaveTaskCommand = new DelegateCommand(UpdateTask, CanUpdateTask);
            FilterForWorker = true;
        }

        public void RefreshTasks()
        {
            var TempTaskList = new List<Task>();
            var TempSelectedTask = SelectedTask.TaskID;
            ShownTasks = new List<Task>();
            if (CurrentWorker != null)
            {
                foreach (var task in AllTasks.ListOfTasks)
                {
                    if (FilterForWorker)
                    {
                        if (task.WorkerIDs.Contains(CurrentWorker.WorkerID))
                        {
                            TempTaskList.Add(task);
                        }
                    }
                    else
                        TempTaskList.Add(task);
                }
            }
            ShownTasks = TempTaskList;
            SelectedTask = ShownTasks.FirstOrDefault(x => x.TaskID == TempSelectedTask);
        }

        #region Navigation Controls
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Worker"))
            {
                CurrentWorker = navigationContext.Parameters.GetValue<Worker>("Worker");
                RefreshTasks();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) { return true; }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
