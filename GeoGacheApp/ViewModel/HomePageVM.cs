using GalaSoft.MvvmLight;
using Geocache.Enums;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.ViewModel
{
    public class HomePageVM : ViewModelBase
    {
        public HomePageVM(UserDataService userData)
        {
            UserData = userData;
        }

        #region fields
        private TreasureType selectedTreasureType;
        private TreasureSizes selectedTreasureSize;
        #endregion

        #region Parameters
        public UserDataService UserData { get; }

        //welcome message
        public string Welcome
        {
            get
            {
                return "Welcome " + UserData.GetUser().FirstName;
            }
        }

        public TreasureType SelectedTreasureType
        {
            get { return selectedTreasureType; }
            set
            {
                selectedTreasureType = value;
                RaisePropertyChanged("SelectedTreasureType");
            }
        }

        public IEnumerable<TreasureType> TreasureTypes
        {
            get
            {
                return Enum.GetValues(typeof(TreasureType))
                    .Cast<TreasureType>();
            }
        }

        public TreasureSizes SelectedTreasureSize
        {
            get { return selectedTreasureSize; }
            set
            {
                selectedTreasureSize = value;
                RaisePropertyChanged("SelectedTreasureSize");
            }
        }

        public IEnumerable<TreasureSizes> TreasureSizes
        {
            get
            {
                return Enum.GetValues(typeof(TreasureSizes))
                    .Cast<TreasureSizes>();
            }
        }

        #endregion

        #region Commands

        #endregion

    }
}
