using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Geocache.ViewModel.PopUpVM
{
    public class UserTreasuresVM:ViewModelBase
    {
        List<Treasure> treasures;
        public UserTreasuresVM(UserDataService userData)
        {
            
            UserData = userData;
            Treasures = UserData.GetUserTreasures();
        }

        public UserDataService UserData { get; private set; }
        public List<Treasure> Treasures { get => treasures; set => treasures = value; }

        #region Commands
        ICommand deleteTreasure;
        ICommand viewTreasure;

        public ICommand ViewTreasure
        {
            get
            {
                return viewTreasure ?? (viewTreasure =
                  new RelayCommand<Treasure>((x =>
                  {
                      
                      MessengerInstance.Send<Treasure>(x, "ShowTreasure");
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
                      using(var unitOfWork= new UnitOfWork(new GeocachingContext()))
                      {
                          unitOfWork.Treasures.Remove(x);
                      }
                  })
                ));

            }
        }

        #endregion
    }
}
