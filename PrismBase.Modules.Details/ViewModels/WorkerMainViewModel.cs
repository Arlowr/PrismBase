﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismBase.Modules.Details.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismBase.Modules.Details.ViewModels
{
    public class WorkerMainViewModel : BindableBase, INavigationAware
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

        private Worker _worker;
        public Worker Worker
        {
            get { return _worker; }
            set { SetProperty(ref _worker, value); }
        }

        #endregion

        public WorkerMainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }
        #region Navigation Controls
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Worker"))
            {
                Worker = navigationContext.Parameters.GetValue<Worker>("Worker");
                TabTitle = Worker.WorkerID + "." + Worker.FirstName.Substring(0, 1) + "." + Worker.LastName.Substring(0, 1);
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var newWorkerPage = navigationContext.Parameters.GetValue<Worker>("Worker");

            if (newWorkerPage != null)
            {
                if (Worker.FullName != newWorkerPage.FullName)
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
