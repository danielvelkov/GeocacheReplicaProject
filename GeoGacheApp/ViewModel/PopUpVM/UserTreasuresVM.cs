using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using Geocache.ViewModel.BrowserVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel.PopUpVM
{
    public class UserTreasuresVM : ViewModelBase
    {
        ObservableCollection<Treasure> treasures;
        public UserTreasuresVM(UserDataService userData)
        {
            UserData = userData;
            Treasures = new ObservableCollection<Treasure>(UserData.GetUserTreasures());
            MessengerInstance.Register<object>(this, "RefreshUserTreasures", obj => 
            {
                Treasures = new ObservableCollection<Treasure>(UserData.GetUserTreasures());
            });
            
        }

        public UserDataService UserData { get;  set; }
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
        ICommand deleteTreasure;
        ICommand viewTreasure;
        ICommand changeTreasure;
        ICommand hideTreasure;
        ICommand closeWindow;
        public ICommand ViewTreasure
        {
            get
            {
                return viewTreasure ?? (viewTreasure =
                  new RelayCommand<Treasure>((x =>
                  {
                      if (x.MarkerInfo != null)
                          MessengerInstance.Send<Treasure>(x, "ShowTreasure");
                      MessengerInstance.Send<CloseWindowEventArgs>(new CloseWindowEventArgs());
                  })
                ));

            }
        }
        public ICommand DeleteTreasure
        {
            get
            {
                return deleteTreasure ?? (deleteTreasure =
                  new RelayCommand<Treasure>((x =>
                  {
                      using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                      {
                          var result = MessageBox.Show("are you sure you wanna " +
                              "delete the treasure? its permanent",
                              "Confirmation", MessageBoxButton.YesNo);
                          if (result == MessageBoxResult.Yes)
                          {
                              //delete all records of people finding the treasure
                              unitOfWork.FoundTreasures.RemoveRange(unitOfWork.FoundTreasures.Find(ft => ft.TreasureID == x.ID));
                              //delete all user comments on said treasure
                              foreach (Treasures_Comments tc in unitOfWork.TreasureComments.Find(tc=>tc.TreasureID==x.ID).ToList())
                              {
                                  //var comment = unitOfWork.TreasureComments.Get(tc.ID);
                                  //unitOfWork.TreasureComments.Remove(comment); 
                                  //unitOfWork.Complete();
                                  unitOfWork.TreasureComments.Remove(tc);
                              }
                              //delete all treasure_chains connected to it
                              if (x.Chained_Treasure1.Count > 0)
                              {
                                  Chained_Treasures ct1 = x.Chained_Treasure1.First();
                                  if ((ct1 = unitOfWork.ChainedTreasures.SingleOrDefault(ct => ct.Treasure1_ID == x.ID)) != null)
                                  {
                                      unitOfWork.ChainedTreasures.Remove(ct1);

                                      unitOfWork.Complete();
                                  }
                              }
                              if (x.Chained_Treasure2.Count > 0)
                              {
                                  foreach (var tc in x.Chained_Treasure2)
                                  {
                                      Chained_Treasures ct2;
                                      if ((ct2 = unitOfWork.ChainedTreasures.SingleOrDefault(ct => ct.Id == tc.Id)) != null)
                                      {
                                          int fixId = (int)ct2.Treasure1_ID;
                                          unitOfWork.ChainedTreasures.Remove(ct2);
                                          unitOfWork.ChainedTreasures.UnchainTreasure(fixId); //unchain the connected treasure
                                          unitOfWork.Complete();
                                      }
                                  }
                              }
                              //remove the marker info
                              //unitOfWork.Markers.Remove_Quicker(x.MarkerInfo);
                              //unitOfWork.Complete();

                              var tres = unitOfWork.Treasures.Get(x.ID);
                              unitOfWork.Treasures.Remove(tres);
                              unitOfWork.Complete();
                              Treasures.Remove(x);
                              RaisePropertyChanged("Treasures");
                              MessageBox.Show(string.Format("Treasure [{0}] has been removed.", x.Name));
                              MessengerInstance.Send<object>(new object(), "Refresh"); //update leaderboard
                          }
                      }
                  })
                ));

            }
        }
        public ICommand ChangeTreasure
        {
            get
            {
                return changeTreasure ?? (changeTreasure =
                  new RelayCommand<Treasure>(x =>
                  {
                      if (!SimpleIoc.Default.IsRegistered<HideTreasurePageVM>())
                          SimpleIoc.Default.Register<HideTreasurePageVM>();
                      MessengerInstance.Send<CloseWindowEventArgs>(new CloseWindowEventArgs());
                      MessengerInstance.Send<Type>(typeof(HideTreasurePageVM), "ChangePage");
                      MessengerInstance.Send<Treasure>(x, "ChangeTreasure");
                  }));
                #endregion
            }
        }
        public ICommand HideTreasure
        {
            get
            {
                return hideTreasure ?? (hideTreasure =
                  new RelayCommand(() =>
                  {
                      MessengerInstance.Send<CloseWindowEventArgs>(new CloseWindowEventArgs());
                      MessengerInstance.Send<Type>(typeof(HideTreasurePageVM), "ChangePage");
                  }));
            }
        }
        public ICommand CloseWindow
        {
            get
            {
                if (closeWindow == null)
                    closeWindow = new RelayCommand(() =>
                    {
                        Messenger.Default.Unregister(this);
                        Messenger.Default.Send(new CloseWindowEventArgs());
                    });
                return closeWindow;
            }
        }
    }
} 
