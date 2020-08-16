using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
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
        }

        public UserDataService UserData { get; private set; }
        public ObservableCollection<Treasure> Treasures { get => treasures; set => treasures = value; }

        #region Commands
        ICommand deleteTreasure;
        ICommand viewTreasure;
        ICommand changeTreasure;
        ICommand hideTreasure;
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
                      Treasures.Remove(x);
                      using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                      {
                          var result = MessageBox.Show("are you sure you wanna " +
                              "delete the treasure? its permanent",
                              "Confirmation", MessageBoxButton.YesNo);
                          if (result == MessageBoxResult.Yes)
                          {
                              Treasures.Remove(x);
                              unitOfWork.Treasures.Remove(x);
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
    }
} 
