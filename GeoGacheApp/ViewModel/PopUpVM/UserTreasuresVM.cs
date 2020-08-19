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
        List<Treasure> treasures;
        public UserTreasuresVM(UserDataService userData)
        {

            UserData = userData;
            MessengerInstance.Register<object>(this, "RefreshUserTreasures", obj => { Treasures = (UserData.GetUserTreasures()); });
            Treasures = (UserData.GetUserTreasures());
        }

        public UserDataService UserData { get;  set; }
        public List<Treasure> Treasures { get => treasures; set => treasures = value; }

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
                              Treasures.Remove(x);
                              if (x.IsChained)
                              {
                                  var temp= unitOfWork.ChainedTreasures.Find(
                                      ct => ct.Treasure1_ID == x.ID || ct.Treasure2_ID == x.ID).ToList();
                                  if (temp.Count == 2)
                                  {
                                      //temp[0].Treasure_1 set the first to false and second too
                                      //unitOfWork.ChainedTreasures
                                  }
                                  else
                                  {
                                      //just first one
                                  }
                              }
                                  
                              unitOfWork.Markers.Remove_Quicker(x.MarkerInfo);
                              unitOfWork.Treasures.Remove_Quicker(x);
                              unitOfWork.Complete();
                              
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
