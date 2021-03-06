﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel.PopUpVM
{
    public class ModerateTreasuresVM:ViewModelBase
    {
        public ModerateTreasuresVM(UserDataService userdata)
        {
            UserData = userdata;
            RefreshTreasures();
            MessengerInstance.Register<object>(this, "Refresh", obj => { RefreshTreasures(); });
        }

        public UserDataService UserData { get;  set; }
        private ObservableCollection<Treasure> treasures;
        public ObservableCollection<Treasure> Treasures
        {
            get
            {
                return treasures;
            }
            set
            {
                treasures = value;
                RaisePropertyChanged("Treasures");
            }
        }

        #region Commands
        private ICommand deleteComment;
        private ICommand deleteTreasure;
        private ICommand filterTreasuresByUsername;
        private ICommand filterTreasuresByReports;
        private ICommand clearFilters;
        private ICommand closeWindow;
        public ICommand DeleteComment
        {
            get
            {
                return deleteComment ?? (deleteComment = new RelayCommand<Treasures_Comments>(x =>
                 {
                     using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                     {
                         unitOfWork.TreasureComments.Remove_Quicker(x);
                         unitOfWork.Complete();
                               
                     }
                     RefreshTreasures();
                 }));
            }
        }
        public ICommand DeleteTreasure
        {
            get
            {
                return deleteTreasure ?? (deleteTreasure = new RelayCommand<Treasure>(x =>
                {
                    using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                    {
                        unitOfWork.Markers.Remove_Quicker(unitOfWork.Markers.SingleOrDefault(m => m.TreasureId == x.ID));
                        unitOfWork.FoundTreasures.RemoveRange(unitOfWork.FoundTreasures.Find(ft => ft.TreasureID == x.ID));
                        
                        Chained_Treasures ct1;
                        if ((ct1=unitOfWork.ChainedTreasures.SingleOrDefault(ct=>ct.Treasure1_ID==x.ID))!=null)
                        {
                            unitOfWork.ChainedTreasures.Remove_Quicker(ct1);
                            unitOfWork.Complete();
                        }
                        Chained_Treasures ct2;
                        if ((ct2 = unitOfWork.ChainedTreasures.SingleOrDefault(ct => ct.Treasure2_ID == x.ID)) != null)
                        {
                            int chainedTreasureId = (int)ct2.Treasure1_ID;
                            unitOfWork.ChainedTreasures.Remove_Quicker(ct2);
                            unitOfWork.ChainedTreasures.UnchainTreasure(chainedTreasureId);
                            unitOfWork.Complete();
                        }
                        foreach (Treasures_Comments tc in x.Treasures_Comments.Where(c => c.ID != 0).ToList())
                        {
                            if (x.Treasures_Comments.Any(c => c.ID == tc.ID))  //dont get it but EF demands it
                                unitOfWork.TreasureComments.Remove_Quicker(new Treasures_Comments { ID=tc.ID });
                        }
                        unitOfWork.Complete();
                        unitOfWork.Treasures.Remove_Quicker(new Treasure {ID=x.ID });
                        unitOfWork.Complete();
                        Treasures.Remove(x);
                        RaisePropertyChanged("Treasures");
                        MessageBox.Show(string.Format("Treasure [{0}.{1}] has been removed.", x.ID, x.Name));
                        MessengerInstance.Send<object>(new object(),"Refresh"); //update leaderboard and user treasures
                    }
                }));
            }
        }

        public ICommand FilterTreasuresByUsername
        {
            get
            {
                if (filterTreasuresByUsername == null)
                    filterTreasuresByUsername = new RelayCommand<string>(username =>
                    {
                        RefreshTreasures();
                        Treasures = new ObservableCollection<Treasure>(Treasures.Where
                            (i => i.User.Username.Contains(username,StringComparison.OrdinalIgnoreCase)==true));
                    });
                return filterTreasuresByUsername;
            }
        }

        public ICommand FilterTreasuresByReports
        {
            get
            {
                if (filterTreasuresByReports == null)
                    filterTreasuresByReports = new RelayCommand(() =>
                    {
                        RefreshTreasures();
                        Treasures = new ObservableCollection<Treasure>(Treasures.OrderByDescending(i=>i.ReportsCount));
                    });
                return filterTreasuresByReports;
            }
        }

        public ICommand ClearFilters
        {
            get
            {
                if (clearFilters == null)
                    clearFilters = new RelayCommand(RefreshTreasures);
                return clearFilters;
            }
        }

        public ICommand CloseWindow
        {
            get
            {
                if (closeWindow == null)
                    closeWindow = new RelayCommand(() =>
                    {
                        Messenger.Default.Send(new CloseWindowEventArgs());
                    });
                return closeWindow;
            }
        }
        #endregion

        private void RefreshTreasures()
        {
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                Treasures = new ObservableCollection<Treasure>(unitOfWork.Treasures.GetTreasuresAndComments());
            }
            
        }
    }
}
